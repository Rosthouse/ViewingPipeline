using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WorldToViewCoordinate : MonoBehaviour, ViewingPipelineAction {

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Camera simulationCamera;
    private List<WorldObjectTransform> relativeWorldObjects;
    private bool active;

    private void Start()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        simulationCamera = GetComponentInChildren<Camera>();
        active = false;
    } 

    public void Forward(List<WorldObjectTransform> relativeWorldObjects, float animationTime)
    {
        Matrix4x4 V = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale).inverse;
        foreach (WorldObjectTransform worldObject in relativeWorldObjects)
        {
            Matrix4x4 M = worldObject.transform.localToWorldMatrix;
            Matrix4x4 MV = V * M;
            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = MV.MultiplyPoint3x4(vertices[i]);
            }
            mesh.vertices = vertices;
            worldObject.worldObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);        
    }

    

    public void Backward(List<WorldObjectTransform> relativeWorldObjects, float animationTime)
    {

        this.transform.SetPositionAndRotation(originalPosition, originalRotation);
        Matrix4x4 V = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale);
        foreach (WorldObjectTransform worldObject in relativeWorldObjects)
        {
            worldObject.worldObject.transform.SetPositionAndRotation(worldObject.originalPosition, worldObject.originalRotation);
            Matrix4x4 M = worldObject.worldObject.transform.worldToLocalMatrix;
            Matrix4x4 MV = M * V;
            Mesh mesh = worldObject.worldObject.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = MV.MultiplyPoint3x4(vertices[i]);
            }
            mesh.vertices = vertices;
            
        }
    }
}
