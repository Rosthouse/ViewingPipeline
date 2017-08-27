using UnityEngine;
using System.Collections;
using System;

public class TransformToUnityCube : Transformation
{
    [SerializeField] private bool isOrtho = false;
    [SerializeField] private bool swapColumn = false;
    [SerializeField, Range(1,3)] private int implementation = 1;
  
    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
        Matrix4x4 matrix; 
        switch(implementation){
            case (1):
                matrix = GetFreiImplementation(cameraProperties);
                break;
            case 2:
                matrix = GetCodingLabsImplementation(cameraProperties);
                break;
            case 3:
                matrix = GetUnityImplementation(cameraProperties);
                break;
            default:
                matrix = GetFreiImplementation(cameraProperties);
                break;
        }
        if(swapColumn){
            matrix = matrix.SwapColumn(2);
        }
        return matrix;
    }

    private Matrix4x4 GetFreiImplementation(Properties cameraProperties){

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

    private Matrix4x4 GetCodingLabsImplementation(Properties cameraProperties){

        Properties.ClipPlane farClip = cameraProperties.GetFarClipPlane(this.transform);
        Properties.ClipPlane nearClip = cameraProperties.GetNearClipPlane(this.transform);

        Matrix4x4 matrix = Matrix4x4.zero;
        matrix[0, 0] = Mathf.Atan(cameraProperties.FOV/2);
        matrix[1, 1] = Mathf.Atan(cameraProperties.FOV/2);
        matrix[2, 2] = -(cameraProperties.dMax + cameraProperties.dMin)/(cameraProperties.dMax - cameraProperties.dMin);
        matrix[2, 3] = -(2*cameraProperties.dMax*cameraProperties.dMin)/(cameraProperties.dMax - cameraProperties.dMin);
        matrix[3, 2] = -1;
        return matrix;
    }

    private Matrix4x4 GetUnityImplementation(Properties cameraProperties){

        Properties.ClipPlane farClip = cameraProperties.GetFarClipPlane(this.transform);
        Properties.ClipPlane nearClip = cameraProperties.GetNearClipPlane(this.transform);

        Matrix4x4 matrix = Matrix4x4.Perspective(cameraProperties.FOV, cameraProperties.Aspect, cameraProperties.dMin, cameraProperties.dMax);
       
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
            RenderFrustum.DrawCube(Vector3.zero, Vector3.one, Color.magenta);
        }
    }
}
