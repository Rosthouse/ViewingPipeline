using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewingPipelineManager : MonoBehaviour {

    private List<WorldObjectTransform> relativeWorldObjects;
    private LinkedListNode<ViewingPipelineAction> current;
    [SerializeField] private float animationTime = 0;
    [SerializeField] private Button forwardButton;
    [SerializeField] private Button backwardsButton;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private CameraProperties cameraProperties;
    [SerializeField] private RenderTexture image;

    // Use this for initialization
    void Start()
    {
        SetUpActions();
        SaveWorldObjects(GameObject.FindGameObjectsWithTag("WorldObject"));
        SetUpGui();
        //Camera simulationCamera = GetComponentInChildren<Camera>();
        //simulationCamera.nearClipPlane = cameraProperties.FrontPlane;
        //simulationCamera.farClipPlane = cameraProperties.BackPlane;
        //simulationCamera.fieldOfView = cameraProperties.FieldOfView;
        //simulationCamera.targetTexture = image;
    }

    private void OnPostRender()
    {
        RenderFrustum.DrawFrustum(GetComponent<Camera>());
        RenderFrustum.DrawLine(Vector3.zero, Vector3.one * 10, Color.magenta);
    }
        
    private void SetUpActions()
    {
        LinkedList<ViewingPipelineAction> delegateList = new LinkedList<ViewingPipelineAction>();
        LinkedListNode<ViewingPipelineAction> first = delegateList.AddFirst(GetComponent<WorldToViewCoordinate>());
        delegateList.AddAfter(first, GetComponent<ViewToNormalizedCoordinate>());


        current = first;
    }

    private void SetUpGui()
    {
        forwardButton.onClick.AddListener(Forward);
        backwardsButton.onClick.AddListener(Backward);
        timeSlider.onValueChanged.AddListener(ValueChanged);
        ValueChanged(timeSlider.value);
    }

    private void SaveWorldObjects(GameObject[] worldObjects)
    {        
        relativeWorldObjects = new List<WorldObjectTransform>();

        foreach (GameObject worldObject in worldObjects)
        {
            WorldObjectTransform trans = new WorldObjectTransform();
            trans.worldObject = worldObject;
            trans.originalPosition = worldObject.transform.position;
            trans.relativePosition = this.transform.InverseTransformPoint(trans.originalPosition);
            trans.originalRotation = worldObject.transform.rotation;
            trans.relativeRotation = trans.originalRotation * Quaternion.Inverse(this.transform.rotation);
            relativeWorldObjects.Add(trans);
        }
    }

    void Forward()
    {
        current.Value.Forward(relativeWorldObjects, this.animationTime);
        current = current.Next == null ? current: current.Next;
        
        //CheckButtonAvaiability();
    }

    void Backward()
    {
        current = current.Previous == null ? current : current.Previous;
        current.Value.Backward(relativeWorldObjects, this.animationTime);
        
        //CheckButtonAvaiability();
    }

    public void ValueChanged(float time)
    {
        this.animationTime = time;
    }

    private void CheckButtonAvaiability()
    {
        backwardsButton.interactable = current.Previous != null;
        forwardButton.interactable = current.Next != null;
    }

}
