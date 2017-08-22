using UnityEngine;
using UnityEditor;

public static class MatrixExtensions
{
    public static Matrix4x4 MultiplyByScalar(this Matrix4x4 matrix, float scalar)
    {
        Matrix4x4 clone = matrix;
        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                clone[i, j] *= scalar;
            }
        }
        return clone;
    }

}