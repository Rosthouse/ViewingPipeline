using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewToNormalizedCoordinate : MonoBehaviour, ViewingPipelineAction
{
    private Camera simulationCamera;
    private Boolean clipped;
    private GameObject simulationCameraBody;

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
        simulationCamera = GetComponentInChildren<Camera>();
        clipped = false;
        simulationCameraBody = GameObject.FindGameObjectWithTag("SimulationCamera");
    }

    public void Update()
    {
        if (clipped)
        {
            RenderFrustum.DrawCube(Vector3.zero, Vector3.one, Color.magenta);
        }
    }

    public void Backward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 P = GetCullingMatrix();
        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            worldObject.Reset();
            P = SimpleTransform.GetMatrix(worldObject.transform, simulationCamera, true, true, true);
            
            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = P.MultiplyPoint(vertices[i]);
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            worldObject.ToOrigin();
        }
        Clipped = false;
    }

    public void Forward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 P = GetCullingMatrix(); // GL.GetGPUProjectionMatrix(simulationCamera.projectionMatrix, false).inverse;
        foreach (WorldObjectTransform worldObject in worldObjects)
        {
            worldObject.Reset();
            P = SimpleTransform.GetMatrix(worldObject.transform, simulationCamera, true, true, true);

            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = P.MultiplyPoint(vertices[i]);
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            worldObject.ToOrigin();
        }
        Clipped = true;
        simulationCamera.orthographic = true;
        simulationCamera.nearClipPlane = 0;
        simulationCamera.farClipPlane = 1;
        simulationCamera.orthographicSize = .5f;
    }

    private Matrix4x4 GetCullingMatrix()
    {
        Matrix4x4 P = Matrix4x4.Perspective(simulationCamera.fieldOfView, simulationCamera.aspect, simulationCamera.nearClipPlane, simulationCamera.farClipPlane);
        Debug.Log("Culling matrix P: \n" + P);
        return P;
    }
}