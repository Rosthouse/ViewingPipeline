using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "prop", menuName = "Transformation/Camera Properties", order = 1)]
public class Properties : ScriptableObject
{
    [SerializeField] private float d = 1;
    [SerializeField] private float dmin = 0;
    [SerializeField] private float dmax = 5;
    [SerializeField] private Rect window = Rect.zero;
    [SerializeField] private bool isOrtho = false;

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
}