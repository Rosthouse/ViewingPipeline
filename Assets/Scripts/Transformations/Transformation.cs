using UnityEngine;

public abstract class Transformation : MonoBehaviour {

    [SerializeField] private bool isActive = true;
    
    public virtual bool Active { get { return isActive; } set { isActive = value; } }

    public abstract Matrix4x4 GetMatrix(Properties cameraProperties);
}