using System;
using UnityEngine;

public class ScaleTransformation : Transformation {

	[SerializeField] private Vector3 scale;

    public override bool Active { get { return true; } set { /*Do nothing here;*/ } }

    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, new Vector4(scale.x, 0f, 0f, 0f));
        matrix.SetRow(1, new Vector4(0f, scale.y, 0f, 0f));
        matrix.SetRow(2, new Vector4(0f, 0f, scale.z, 0f));
        matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        return matrix;
    }
}