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
    [SerializeField] private RenderTexture image;

    // Use this for initialization
    void Start()
    {
        SetUpActions();
        SaveWorldObjects(GameObject.FindGameObjectsWithTag("WorldObject"));
        SetUpGui();
    }
    
    private void SetUpActions()
    {
        LinkedList<ViewingPipelineAction> delegateList = new LinkedList<ViewingPipelineAction>();
        LinkedListNode<ViewingPipelineAction> first = delegateList.AddFirst(GetComponent<WorldToViewCoordinate>());
        LinkedListNode<ViewingPipelineAction> second = delegateList.AddAfter(first, GetComponent<ViewToNormalizedCoordinate>());
        delegateList.AddAfter(second, GetComponent<NormalizedToDevice>());
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
            WorldObjectTransform trans = new WorldObjectTransform(
                worldObject,
                this.transform.InverseTransformPoint(worldObject.transform.position),
                worldObject.transform.rotation * Quaternion.Inverse(this.transform.rotation)
            );
            relativeWorldObjects.Add(trans);
        }
    }

    void Forward()
    {
        current.Value.Forward(relativeWorldObjects, this.animationTime);
        current = current.Next == null ? current: current.Next;
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
