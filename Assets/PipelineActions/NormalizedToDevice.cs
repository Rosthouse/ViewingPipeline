using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NormalizedToDevice : MonoBehaviour, ViewingPipelineAction
{
    private Camera simulationCamera;
    private Boolean normalized = false;
    private RenderTexture renderTexture;
    [SerializeField] private Rect viewPort;
    // Use this for initialization
    void Start()
    {
        this.simulationCamera = GetComponentInChildren<Camera>();
        this.renderTexture = this.simulationCamera.targetTexture;
    }

    public void Update()
    {
        if (normalized)
        {
            RenderFrustum.DrawRectangle(viewPort.min, viewPort.max, Color.cyan);

            Vector3 origin = Camera.main.ViewportToScreenPoint(new Vector3(0.25F, 0.1F, 0));
            Vector3 extent = Camera.main.ViewportToScreenPoint(new Vector3(0.5F, 0.2F, 0));
            GUI.DrawTexture(new Rect(origin.x, origin.y, extent.x, extent.y), simulationCamera.targetTexture);
        }
    }

    public void Backward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        normalized = false;
    }

    public void Forward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        Matrix4x4 D = GetDeviceMatrix();
        foreach(WorldObjectTransform worldObject in worldObjects)
        {
            Vector3[] vertices = worldObject.Vertices;
            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = D.MultiplyPoint(vertices[i]);
            }
            worldObject.SetVertices(vertices);
        }

        normalized = true;
    }

    public Matrix4x4 GetDeviceMatrix()
    {
        Matrix4x4 D = Matrix4x4.zero;
        D[0, 0] = viewPort.width;
        D[1, 1] = viewPort.height;
        D[1, 3] = viewPort.height;
        D[2, 2] = 1;
        D[3, 3] = 1;
        return D;
    }
}
