using System;
using System.Collections.Generic;
using UnityEngine;

public struct WorldObjectTransform
{
    public readonly GameObject worldObject;
    private readonly MeshFilter filter;
    public readonly Vector3 originalPosition;
    public readonly Vector3 relativePosition;
    private readonly Vector3[] originalVertices;
    public readonly Quaternion originalRotation;
    public readonly Quaternion relativeRotation;


    public Transform transform
    {
        get { return worldObject.transform;  }
    }

    public Vector3[] Vertices
    {
        get
        {
            Mesh mesh = worldObject.GetComponent<MeshFilter>().mesh;
            return mesh.vertices;
        }
    }

    public void SetVertices(Vector3[] vertices)
    {
        filter.mesh.vertices = vertices;
        filter.mesh.RecalculateBounds();
    }

    public WorldObjectTransform(GameObject worldObject, Vector3 relativePosition, Quaternion relativeRotation)
    {
        this.worldObject = worldObject;
        this.filter = worldObject.GetComponent<MeshFilter>();
        this.originalPosition = worldObject.transform.position;
        this.originalRotation = worldObject.transform.rotation;

        this.originalVertices = this.filter.mesh.vertices;

        this.relativePosition = relativePosition;
        this.relativeRotation = relativeRotation;
    }

    public void ToOrigin()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void Reset()
    {
        filter.mesh.vertices = filter.sharedMesh.vertices;
        transform.SetPositionAndRotation(originalPosition, originalRotation);
        SetVertices(originalVertices);
    }
}

public interface ViewingPipelineAction
{
    void Forward(List<WorldObjectTransform> worldObjects, float animationTime);
    void Backward(List<WorldObjectTransform> worldObjects, float animationTime);
}