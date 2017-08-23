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
    private CameraProperties initialCameraProperties;
    private CameraProperties ortographicProperties = new CameraProperties(45, 1, 0, 1, true);

    // Use this for initialization
    void Start () {
        COORD_TRANSF = Matrix4x4.identity;
        COORD_TRANSF[2, 2] = -1;

        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        startScale = this.transform.localScale;

        simulationCamera = GetComponentInChildren<Camera>();
        this.initialCameraProperties = new CameraProperties(simulationCamera);
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

        //Matrix4x4 V = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale).inverse;
        //foreach (WorldObjectTransform worldObject in worldObjects)
        //{
        //    Matrix4x4 M = worldObject.transform.localToWorldMatrix;
        //    Matrix4x4 MV = V * M;
        //    Vector3[] vertices = worldObject.Vertices;
        //    for (int i = 0; i < vertices.Length; i++)
        //    {
        //        vertices[i] = MV.MultiplyPoint3x4(vertices[i]);
        //    }
        //    worldObject.SetVertices(vertices);
        //    worldObject.worldObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //}
        //this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //return;


        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            Matrix4x4 transformMatrix = GetMatrix(worldObject.transform, simulationCamera, toView, toClip, toDevice);
            Vector3[] vertices = worldObject.Vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transformMatrix.MultiplyPoint3x4(vertices[i]);
            }
            worldObject.SetVertices(vertices);
            worldObject.worldObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    public static Matrix4x4 GetMatrix(Transform model, Camera simulationCamera, Boolean toView, Boolean toClip, Boolean toDevice)
    {       
        Matrix4x4 M = model.localToWorldMatrix;        
        Matrix4x4 V = Matrix4x4.identity;
        Matrix4x4 P = Matrix4x4.identity;
        Matrix4x4 D = Matrix4x4.identity;

        if (toView)
        {
            V = Matrix4x4.TRS(simulationCamera.transform.position, simulationCamera.transform.rotation, simulationCamera.transform.localScale).inverse;
        }
        if (toClip)
        {
            P = Matrix4x4.Perspective(
                simulationCamera.fieldOfView,
                simulationCamera.aspect,
                simulationCamera.nearClipPlane,
                simulationCamera.farClipPlane
            ) * COORD_TRANSF; Debug.Log("P PM: \n" + P);
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
    
    public void Backwards()
    {

        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            worldObject.Reset();
        }

        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        this.transform.localScale = startScale;

        this.simulationCamera.ApplyProperties(this.initialCameraProperties);

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
        if (toClip)
        {
            RenderFrustum.DrawCube(-Vector3.one, Vector3.one, Color.magenta);
        }


        if (toDevice)
        {
            RenderFrustum.DrawRectangle(new Vector3(0, 0, 0), new Vector3(1, 1, 0), Color.yellow);
        }
    }
}
