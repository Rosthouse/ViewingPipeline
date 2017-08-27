using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformToCamera : Transformation
{
    [SerializeField] private new Transform camera;

    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
       return Matrix4x4.TRS(camera.position, camera.rotation, camera.localScale);
    }
}
