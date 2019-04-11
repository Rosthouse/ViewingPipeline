using UnityEngine;


[CreateAssetMenu(fileName = "prop", menuName = "Transformation/Camera Properties", order = 1)]
public class Properties : ScriptableObject
{
    [SerializeField] private float d = 1;
    [SerializeField] private float dmin = 0;
    [SerializeField] private float dmax = 5;
    [SerializeField] private Rect window = Rect.zero;
    [SerializeField] private bool isOrtho = false;
    [SerializeField] private float fov = 60;
    [SerializeField] private float aspect = 1;

    public struct ClipPlane {
        public readonly Vector3 center;
        public readonly Vector2 size;

        public Vector3 min
        {
            get { return new Vector3(center.x - size.x / 2, center.y - size.y / 2, center.z); }

        }

        public Vector3 max
        {
            get { return new Vector3(center.x + size.x / 2, center.y + size.y / 2, center.z); }
        }

        public ClipPlane(Vector3 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
        }
    }

    public float D
    {
        get { return d; }
        set { d = value; }
    }

    public float dMin
    {
        get { return dmin; }
        set { dmin = value; }
    }

    public float dMax
    {
        get { return dmax ; }
        set { dmax = value; }
    }

    public Rect Window
    {
        get { return window; }
        set { window = value;  }
    }

    public bool IsOrtho {
        get { return isOrtho; }
        set{ isOrtho = value; }
    }

    public float FOV {
        get { return fov; }
        set { fov = value; }
    }

    public float Aspect {
        get { return aspect; }
        set { aspect = value; }
    }

    public ClipPlane GetFarClipPlane(Transform t)
    {
        return GetPlaneInFrustum(t, dMax);
       float frustumHeight = 2.0f * dMax * Mathf.Tan(FOV * 0.5f * Mathf.Deg2Rad);
       float frustumWidth = frustumHeight * Aspect;
       return new ClipPlane(t.position + new Vector3(0, 0, dmax), new Vector2(frustumWidth, frustumHeight));
    }

    public ClipPlane GetNearClipPlane(Transform t)
    {
        return GetPlaneInFrustum(t, dMin);
        float frustumHeight = 2.0f * dMin * Mathf.Tan(FOV * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * Aspect;
        return new ClipPlane(t.position + new Vector3(0, 0, dmin), new Vector2(frustumWidth, frustumHeight));
    }

    public ClipPlane GetPlaneInFrustum(Transform t, float d)
    {
        float frustumHeight = 2.0f * d * Mathf.Tan(FOV * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * Aspect;
        return new ClipPlane(t.position + new Vector3(0, 0, d), new Vector2(frustumWidth, frustumHeight));
    }
}
