using System;
using UnityEngine;

public class PositionTransformation : Transformation {

	[SerializeField] private Vector3 position;


    public override bool Active { get { return true; } set { /*Do nothing here;*/ } }

    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetRow(0, new Vector4(1f, 0f, 0f, position.x));
        matrix.SetRow(1, new Vector4(0f, 1f, 0f, position.y));
        matrix.SetRow(2, new Vector4(0f, 0f, 1f, position.z));
        matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        return matrix;
    }
}