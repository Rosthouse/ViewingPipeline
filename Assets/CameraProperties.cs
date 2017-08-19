using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "cam", menuName = "Camera/Properties", order = 1)]
public class CameraProperties : ScriptableObject {

    [SerializeField] private Vector2 max;
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 uv;
    [SerializeField] private float d;
    [SerializeField] private float backPlane, frontPlane;
    [SerializeField, Range(0, 180)] private int fieldOfView;

    public Vector2 Max  { get  {  return max; } }
    public Vector2 Min { get { return min; } }
    public Vector2 Uv { get { return uv; } }
    public float D { get {  return d; } }
    public float BackPlane { get { return backPlane; } }
    public float FrontPlane { get { return frontPlane; } }
    public int FieldOfView { get { return fieldOfView; } }
}
