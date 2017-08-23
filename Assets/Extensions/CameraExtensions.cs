using UnityEngine;
using UnityEditor;

public static class CameraExtensions
{
   public static void Normalize(this Camera camera)
   {
        camera.orthographic = true;
        camera.nearClipPlane = 0;
        camera.farClipPlane = 1;
        camera.orthographicSize = .5f;
   }
}