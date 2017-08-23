using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NormalizedToDevice : MonoBehaviour, ViewingPipelineAction
{
    private Camera simulationCamera;
    private Boolean normalized = false;
    private RenderTexture renderTexture;
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
            RenderFrustum.DrawRectangle(Vector3.zero, new Vector3(256, 256, 0), Color.cyan);
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
        D[0, 0] = this.renderTexture.width;
        D[1, 1] = -this.renderTexture.height;
        D[1, 3] = this.renderTexture.height;
        D[2, 2] = 1;
        D[3, 3] = 1;
        return D;
    }
}
