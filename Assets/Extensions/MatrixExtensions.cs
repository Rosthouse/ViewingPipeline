using UnityEngine;

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

    public static Matrix4x4 SwapColumn(this Matrix4x4 matrix, int column){
        
        Matrix4x4 COORD_TRANSF = Matrix4x4.identity;
        for(int i = 0; i<4; i++){
            COORD_TRANSF[i, column] = -1;
        }


        return COORD_TRANSF* matrix  ;


    }

}