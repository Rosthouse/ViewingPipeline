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
    private GameObject[] worldObjects;

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
        worldObjects = GameObject.FindGameObjectsWithTag("WorldObject");

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
        foreach(GameObject worldObject in worldObjects)
        {
            Mesh mesh = worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            
            Matrix4x4 transformMatrix =  GetMatrix(worldObject.transform);

            Debug.Log("TransformMatrix:\n" + transformMatrix);
            for(int i = 0; i < vertices.Length; i++)
            {
                Vector3 newVertex = transformMatrix.MultiplyPoint3x4(vertices[i]);
                Debug.Log("Old/new vertex: " + vertices[i] + "/" + newVertex);
                vertices[i] = newVertex;
            }

            worldObject.transform.position = Vector3.zero;
            worldObject.transform.rotation = Quaternion.identity;
            worldObject.transform.localScale = Vector3.one;

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.transform.localScale = Vector3.one;
    }

    
    private Matrix4x4 GetMatrix(Transform transform)
    {
        Matrix4x4 M = transform.localToWorldMatrix;        
        Matrix4x4 V = Matrix4x4.identity;
        Matrix4x4 C = Matrix4x4.identity;
        Matrix4x4 P = Matrix4x4.identity;

        if (toView)
        {
            V =  Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale).inverse;
        }
        if (toClip && !toDevice)
        {
            C *= simulationCamera.cullingMatrix;
        }
        if(toDevice)
        {
            P = getProjectionMatrix(simulationCamera.fieldOfView, simulationCamera.farClipPlane, simulationCamera.nearClipPlane).inverse;
            P = GL.GetGPUProjectionMatrix(simulationCamera.projectionMatrix, false);
            deviced = true;
        }
        return P * C * V * M;
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

        foreach (GameObject worldObject in worldObjects)
        {
            Matrix4x4 transformMatrix = GetInverse(worldObject.transform);
            Mesh mesh = worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transformMatrix.MultiplyPoint3x4(vertices[i]);
                //vertices[i]= worldObject.transform.TransformPoint(vertices[i]);
            }
            mesh.vertices = vertices;
        }

        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        this.transform.localScale = startScale;

        Debug.Log("Backwards");
    }

    public Matrix4x4 getProjectionMatrix(float fieldOfView, float far, float near)
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
    }
}
