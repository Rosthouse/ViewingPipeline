using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WorldToViewCoordinate : MonoBehaviour, ViewingPipelineAction {

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Camera simulationCamera;

    private void Start()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        simulationCamera = GetComponentInChildren<Camera>();
    }

    public void Forward(List<WorldObjectTransform> relativeWorldObjects, float animationTime)
    {
        iTween.MoveTo(this.gameObject, Vector3.zero,animationTime);
        iTween.RotateTo(this.gameObject, Vector3.zero,animationTime);


        foreach (WorldObjectTransform worldObject in relativeWorldObjects)
        {
            iTween.MoveTo(worldObject.worldObject, worldObject.relativePosition,  animationTime);
            iTween.RotateTo(worldObject.worldObject, worldObject.relativeRotation.eulerAngles, animationTime);
        }
    }

    public void Backward(List<WorldObjectTransform> relativeWorldObjects, float animationTime)
    {
        iTween.MoveTo(this.gameObject, originalPosition, animationTime);
        iTween.RotateTo(this.gameObject, originalRotation.eulerAngles, animationTime);


        foreach (WorldObjectTransform worlObject in relativeWorldObjects)
        {
            iTween.MoveTo(worlObject.worldObject, worlObject.originalPosition, animationTime);
            iTween.RotateTo(worlObject.worldObject, worlObject.originalRotation.eulerAngles, animationTime);
        }
        
    }
}
