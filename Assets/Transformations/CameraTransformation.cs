using System;
using UnityEngine;

public class CameraTransformation : Transformation {

	public float focalLength = 1f;
    [SerializeField] private bool isOrtho = false;

	//public override Matrix4x4 Matrix {
	//	get {
            
	//		Matrix4x4 matrix = new Matrix4x4();
 //           if (isOrtho)
 //           {
 //               matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
 //               matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
 //               matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
 //               matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
 //           }
 //           else
 //           {
 //               matrix.SetRow(0, new Vector4(focalLength, 0f, 0f, 0f));
 //               matrix.SetRow(1, new Vector4(0f, focalLength, 0f, 0f));
 //               matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
 //               matrix.SetRow(3, new Vector4(0f, 0f, 1f, 0f));
 //           }
 //           return matrix;           
	//	}
	//}

    public override Matrix4x4 GetMatrix(Properties cameraProperties)
    {
        Matrix4x4 matrix = new Matrix4x4();
        if (isOrtho)
        {
            matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        }
        else
        {
            matrix.SetRow(0, new Vector4(cameraProperties.D, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, cameraProperties.D, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 1f, 0f));
        }
        return matrix;
    }
}