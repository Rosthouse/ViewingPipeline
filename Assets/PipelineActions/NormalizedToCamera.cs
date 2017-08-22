using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NormalizedToCamera : MonoBehaviour, ViewingPipelineAction
{
    private Camera simulationCamera;
    // Use this for initialization
    void Start()
    {
        this.simulationCamera = GetComponent<Camera>();
    }

    public void Backward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        throw new NotImplementedException();
    }

    public void Forward(List<WorldObjectTransform> worldObjects, float animationTime)
    {
        throw new NotImplementedException();
    }

}
