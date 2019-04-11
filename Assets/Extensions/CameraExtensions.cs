using UnityEngine;

public static class CameraExtensions
{
   public static void Normalize(this Camera camera)
   {
        camera.orthographic = true;
        camera.nearClipPlane = 0;
        camera.farClipPlane = 1;
        camera.orthographicSize = .5f;
   }

    public static void ApplyProperties(this Camera camera, CameraProperties properties)
    {
        camera.farClipPlane = properties.farClip;
        camera.nearClipPlane = properties.nearClip;
        camera.orthographic = properties.isOrtographic;
        camera.fieldOfView = properties.fieldOfView;
        camera.aspect = properties.aspect;
    }
}