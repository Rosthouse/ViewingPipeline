using UnityEngine;

public static class TransformExtensions 
{
    private static readonly Vector3 ONE = new Vector3(1, 1, 1);

    public static void Reset(this Transform t)
    {
        t.position = Vector3.zero;
        t.rotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static void SetPositionAndRotation(this Transform t, Vector3 position, Quaternion rotation)
    {
        SetPositionAndRotation(t, position, rotation, Vector3.one);
    }

    public static void SetPositionAndRotation(this Transform t, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        t.position = position;
        t.rotation = rotation;
        t.localScale = scale;
    }
}