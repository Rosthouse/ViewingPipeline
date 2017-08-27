using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProperties  {
    
    public readonly float farClip, nearClip;
    public readonly bool isOrtographic = false;
    public readonly float fieldOfView;
    public readonly float aspect;

    public CameraProperties() { }

    public CameraProperties(Camera simulationCamera)
    {
        this.farClip = simulationCamera.farClipPlane;
        this.nearClip = simulationCamera.nearClipPlane;
        this.isOrtographic = simulationCamera.orthographic;
        this.fieldOfView = simulationCamera.fieldOfView;
        this.aspect = simulationCamera.aspect;
    }

    public CameraProperties(float fieldOfView, float aspect, float nearClip, float farClip, bool isOrtgraphic = false )
    {
        this.farClip = farClip;
        this.nearClip = nearClip;
        this.isOrtographic = isOrtgraphic;
        this.fieldOfView = fieldOfView;
        this.aspect = aspect;
    }
}
