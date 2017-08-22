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
            RenderFrustum.DrawCube(Vector3.one * -1, Vector3.one, Color.magenta);
        }
    }

    public void Backward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 P = GL.GetGPUProjectionMatrix(simulationCamera.projectionMatrix, false);

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

        Clipped = false;
    }

    public void Forward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 matrix = Matrix4x4.Perspective(90, 1, -1, 1);
        float ymax = getYforClipPlane(simulationCamera.farClipPlane);
        float ymin = getYforClipPlane(simulationCamera.nearClipPlane);
        //matr
        //matrix[0, 0] = //TODO;
        matrix[0, 1] = 0;
        //matrix[0, 1] = 

        foreach(WorldObjectTransform worldObject in worldObjects)
        {
            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = matrix.MultiplyPoint(vertices[i]);
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
        Clipped = true;
    }

    private float getYforClipPlane(float clipPlane)
    {
        float b = (clipPlane * Mathf.Sin(90)) / Mathf.Sin(simulationCamera.fieldOfView / 2);
        float a = Mathf.Sqrt(Mathf.Pow(clipPlane, 2 ) - Mathf.Pow(b,2 ));
        Vector3 ymax = new Vector3(this.transform.position.x, this.transform.position.y + a, this.transform.position.z + clipPlane);
        return this.transform.position.y + a;
    }
}