using UnityEngine;
using System.Collections;
using System;

public class TransformToUnityCube : Transformation
{
    [SerializeField] private bool isOrtho = false;
  
    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
        Properties.ClipPlane farClip = cameraProperties.GetFarClipPlane(this.transform);
        Properties.ClipPlane nearClip = cameraProperties.GetNearClipPlane(this.transform);

        Matrix4x4 matrix = Matrix4x4.zero;
        matrix[0, 0] = isOrtho ? 1 : cameraProperties.D / (farClip.max.x - farClip.min.x);
        matrix[0, 2] = getAspect(farClip.max.x, farClip.min.x);
        matrix[1, 1] = isOrtho ? 1: cameraProperties.D / (farClip.max.y - farClip.min.y);
        matrix[1, 2] = getAspect(farClip.max.y, farClip.min.y);

        matrix[2, 2] = cameraProperties.dMax / (cameraProperties.dMax - cameraProperties.dMin);
        matrix[2, 3] = -((cameraProperties.dMax * cameraProperties.dMin) / (cameraProperties.dMax - cameraProperties.dMin));
        matrix[3, 2] = 1;

        return matrix;
    }

    private  float getAspect(float max, float min)
    {
        return .5f * (1 - ((max + min) / (max - min)));
    }

    private void Update()
    {
        if (this.Active)
        {
            RenderFrustum.DrawCube(Vector3.one * -1, Vector3.one, Color.magenta);
        }
    }
}
