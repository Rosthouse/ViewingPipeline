using UnityEngine;
using System.Collections;
using System;

public class TransformToDevice : Transformation
{
    private Rect window;

    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
        this.window = cameraProperties.Window;
        Matrix4x4 matrix = Matrix4x4.zero;
        matrix[0, 0] = cameraProperties.Window.width;
        matrix[1, 1] = -cameraProperties.Window.height;
        matrix[2, 2] = 1;
        matrix[1, 3] = cameraProperties.Window.height;
        matrix[3, 3] = 1;
        return matrix;
    }

    public void Update()
    {
        if (this.Active && window != null)
        {
            RenderFrustum.DrawRectangle(window.min, window.max, Color.green);
        }
    }
}
