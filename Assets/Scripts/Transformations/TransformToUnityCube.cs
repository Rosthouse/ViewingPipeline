using UnityEngine;
using System.Collections;
using System;

public class TransformToUnityCube : Transformation
{
    [SerializeField] private bool isOrtho = false;
    [SerializeField] private bool swapColumn = false;
    [SerializeField] private bool transpose = false;
    [SerializeField] private bool inverse = false;
    [SerializeField, Range(1,4)] private int implementation = 1;
  
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
            case 4:
                matrix = GetScratchAPixleImplementation(cameraProperties);
                break;
            default:
                matrix = GetFreiImplementation(cameraProperties);
                break;
        }
        if(swapColumn){
            matrix = matrix.SwapColumn(2);
        }
        if(transpose){
            matrix = matrix.transpose;
        }
        if(inverse){
            matrix = matrix.inverse;
        }
        return matrix;
    }

    private Matrix4x4 GetFreiImplementation(Properties cameraProperties){

        Properties.ClipPlane farClip = cameraProperties.GetFarClipPlane(this.transform);
        Properties.ClipPlane nearClip = cameraProperties.GetNearClipPlane(this.transform);

        Matrix4x4 matrix = Matrix4x4.zero;
        matrix[0, 0] =  cameraProperties.D / (farClip.max.x - farClip.min.x);
        matrix[0, 2] = getAspect(cameraProperties.Window.max.x, cameraProperties.Window.min.x);
        matrix[1, 1] =  cameraProperties.D / (farClip.max.y - farClip.min.y);
        matrix[1, 2] = getAspect(cameraProperties.Window.max.y, cameraProperties.Window.min.y);

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
        Matrix4x4 matrix = Matrix4x4.Perspective(cameraProperties.FOV, cameraProperties.Aspect, cameraProperties.dMin, cameraProperties.dMax);
        return matrix;
    }

    private Matrix4x4 GetScratchAPixleImplementation(Properties cameraProperties){

        Properties.ClipPlane farClip = cameraProperties.GetFarClipPlane(this.transform);
        Properties.ClipPlane nearClip = cameraProperties.GetNearClipPlane(this.transform);
        Properties.ClipPlane viewPlane = cameraProperties.GetPlaneInFrustum(this.transform, cameraProperties.D);

        Matrix4x4 matrix = Matrix4x4.zero;
        float s = 1 / (Mathf.Tan((cameraProperties.FOV/2)+ Mathf.PI/180 ));

        matrix[0, 0] = 2 * cameraProperties.dMin/(viewPlane.max.x - viewPlane.min.x);
        matrix[1, 1] =  2 * cameraProperties.dMin/(viewPlane.max.y - viewPlane.min.y);
        matrix[2, 2] = (cameraProperties.dMax + cameraProperties.dMin)/(cameraProperties.dMax - cameraProperties.dMin);
        matrix[2, 3] = -2 * cameraProperties.dMax * cameraProperties.dMin / (cameraProperties.dMax - cameraProperties.dMin);
        matrix[3, 2] = -1;
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
