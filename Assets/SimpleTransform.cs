using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTransform : MonoBehaviour {

    [Header("Tranform Model to world")]
    [SerializeField]
    private bool toWorld = true;
    private bool worlded = false;

    [Header("Tranform World to view")]
    [SerializeField]
    private bool toView = false;
    private bool viewed = false;


    [Header("Tranform View to clip")]
    [SerializeField]
    private bool toClip = false;
    private bool clipped = false;


    [Header("Tranform Clip to device")]
    [SerializeField]
    private bool toDevice = false;
    private bool deviced = false;

    private Camera simulationCamera;
    private List<WorldObjectTransform> worldObjects;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private static Matrix4x4 COORD_TRANSF;
    private float amount;



    // Use this for initialization
    void Start () {
        COORD_TRANSF = Matrix4x4.identity;
        COORD_TRANSF[2, 2] = -1;

        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        startScale = this.transform.localScale;

        simulationCamera = GetComponentInChildren<Camera>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WorldObject");
        worldObjects = new List<WorldObjectTransform>();
        foreach(GameObject go in gos)
        {
            WorldObjectTransform trans = new WorldObjectTransform(
                go,
                this.transform.InverseTransformPoint(go.transform.position),
                go.transform.rotation * Quaternion.Inverse(this.transform.rotation)
            );
            worldObjects.Add(trans);
        }


        this.amount = 1;
	}
    
    public void SetToView(System.Boolean value)
    {
        Debug.Log("To View: " + value);
        toView = value;
    }

    public void SetToClip(System.Boolean value)
    {
        Debug.Log("To Clip: " + value);
        toClip = value;
    }

    public void SetToDevice(System.Boolean value)
    {
        Debug.Log("To Device: " + value);
        toDevice = value;
    }

    public void SetTransformationAmount(float value)
    {
        this.amount = value;
    }


    public void Forward()
    {
        Debug.Log("Forward");
        foreach(WorldObjectTransform worldObject in worldObjects)
        {
            Vector3[] vertices = worldObject.Vertices;
            
            Matrix4x4 transformMatrix = GetMatrix(worldObject.transform, simulationCamera, toView, toClip, toDevice);

            Debug.Log("TransformMatrix:\n" + transformMatrix);
            for(int i = 0; i < vertices.Length; i++)
            {
                Vector3 newVertex = transformMatrix.MultiplyPoint(vertices[i]);
                Debug.Log("Old/new vertex: " + vertices[i] + "/" + newVertex);
                vertices[i] = newVertex;
            }

            worldObject.ToOrigin();
            worldObject.SetVertices(vertices);
        }

        //simulationCamera.Normalize();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.transform.localScale = Vector3.one;
    }

    
    public static Matrix4x4 GetMatrix(Transform model, Camera simulationCamera, Boolean toView, Boolean toClip, Boolean toDevice)
    {
        Matrix4x4 M = model.localToWorldMatrix;        
        Matrix4x4 V = Matrix4x4.identity;
        Matrix4x4 P = Matrix4x4.identity;
        Matrix4x4 D = Matrix4x4.identity;

        if (toView)
        {
            V = simulationCamera.worldToCameraMatrix.inverse;
        }
        if (toClip)
        {
            P = simulationCamera.projectionMatrix.inverse;
            Debug.Log("P PM: \n" + P);
        }
        if (toDevice)
        {
            D = Matrix4x4.zero;
            D[0, 0] = 1;
            D[1, 1] = -1;
            D[1, 3] = 1;
            D[2, 2] = 1;
            D[3, 3] = 1;
        }
        return D * P * V * M;
    }

    private Matrix4x4 GetInverse(Transform localToWorld)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        if (toView)
        {
            matrix *= simulationCamera.cameraToWorldMatrix;
        }
        if (toClip)
        {
            matrix *= simulationCamera.cullingMatrix.inverse;
        }
        if (toDevice)
        {
            matrix *= simulationCamera.worldToCameraMatrix.inverse;
        }
        return matrix;
    }
    
    public void Backwards()
    {

        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            worldObject.Reset();
        }

        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        this.transform.localScale = startScale;

        Debug.Log("Backwards");
    }

    public static Matrix4x4 getProjectionMatrix(float fieldOfView, float far, float near)
    {
        Matrix4x4 P = Matrix4x4.zero;
        P[0, 0] = Mathf.Atan(fieldOfView / 2);
        P[1, 1] = Mathf.Atan(fieldOfView / 2);
        P[2, 2] = -1 * ((far + near) / (far - near));
        P[2, 3] = -1 * ((2 * (near * far)) / (far - near));
        P[3, 2] = -1;
        return P;
    }

    public void Update()
    {
        if (deviced)
        {
            RenderFrustum.DrawCube(new Vector3(0, 0, 0), new Vector3(3, 4, 1), Color.yellow);
        }

        if (toClip)
        {
            RenderFrustum.DrawCube(-Vector3.one, Vector3.one, Color.magenta);
        }
    }
}
