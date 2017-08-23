using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NormalizedToCamera : MonoBehaviour, ViewingPipelineAction
{
    private Camera simulationCamera;
    private Boolean normalized = false;
    // Use this for initialization
    void Start()
    {
        this.simulationCamera = GetComponent<Camera>();
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
        normalized = true;
    }

}
