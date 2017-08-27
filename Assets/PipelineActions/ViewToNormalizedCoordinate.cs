using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewToNormalizedCoordinate : MonoBehaviour, ViewingPipelineAction
{
    private Camera simulationCamera;
    private Boolean clipped;
    private GameObject simulationCameraBody;
    private Matrix4x4 COORD_TRANSF;

    private Boolean Clipped
    {
        get { return clipped; }
        set
        {
            clipped = value;
            simulationCameraBody.SetActive( !value);
        }
    }

    public void Start()
    {

        COORD_TRANSF = Matrix4x4.identity;
        COORD_TRANSF[2, 2] = -1;
        simulationCamera = GetComponentInChildren<Camera>();
        clipped = false;
        simulationCameraBody = GameObject.FindGameObjectWithTag("SimulationCamera");
    }

    public void Update()
    {
        if (clipped)
        {
            RenderFrustum.DrawCube(-Vector3.one, Vector3.one, Color.magenta);
        }
    }

    public void Backward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 P = GetCullingMatrix();
        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            //worldObject.Reset();
            P = simulationCamera.projectionMatrix;
            
            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = P.MultiplyPoint(vertices[i]);
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            //worldObject.ToOrigin();
        }
        Clipped = false;
    }

    public void Forward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 P = GetCullingMatrix() * COORD_TRANSF; // GL.GetGPUProjectionMatrix(simulationCamera.projectionMatrix, false).inverse;
        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = P.MultiplyPoint(vertices[i]);
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
        Clipped = true;
    }

    private Matrix4x4 GetCullingMatrix()
    {
        Matrix4x4 P = Matrix4x4.Perspective(
            simulationCamera.fieldOfView, 
            simulationCamera.aspect, 
            simulationCamera.nearClipPlane, 
            simulationCamera.farClipPlane
            );
        Debug.Log("Culling matrix P: \n" + P);
        return P;
    }
}