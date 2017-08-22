using System.Collections.Generic;
using UnityEngine;

public struct WorldObjectTransform
{
    public GameObject worldObject;
    public Vector3 originalPosition;
    public Vector3 relativePosition;
    public Quaternion originalRotation;
    public Quaternion relativeRotation;

    public Transform transform
    {
        get { return worldObject.transform;  }
    }
}

public interface ViewingPipelineAction
{
    void Forward(List<WorldObjectTransform> worldObjects, float animationTime);
    void Backward(List<WorldObjectTransform> worldObjects, float animationTime);
}