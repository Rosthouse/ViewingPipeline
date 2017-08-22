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

    private void Update()
    {
        if (this.active) { 
            Matrix4x4 V  = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale).inverse;
            foreach (WorldObjectTransform worldObject in relativeWorldObjects)
            {
                Matrix4x4 M = worldObject.worldObject.transform.localToWorldMatrix;
                Matrix4x4 MV = V * M;
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

    public void Forward(List<WorldObjectTransform> relativeWorldObjects, float animationTime)
    {
        Matrix4x4 V = Matrix4x4.TRS(this.transform.position, this.transform.rotation, this.transform.localScale).inverse;
        foreach (WorldObjectTransform worldObject in relativeWorldObjects)
        {
            Matrix4x4 M = worldObject.worldObject.transform.localToWorldMatrix;
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

    private Matrix4x4 ViewMatrix()
    {
        Vector3 u = this.transform.TransformPoint(this.transform.localPosition + new Vector3(1, 0, 0));
        Vector3 v = this.transform.TransformPoint(this.transform.localPosition + new Vector3(0, 1, 0));
        Vector3 n = this.transform.TransformPoint(this.transform.localPosition + new Vector3(0, 0, 1));

        Matrix4x4 matrix = new Matrix4x4();
        matrix[0, 0] = u.x;
        matrix[1, 0] = u.y;
        matrix[2, 0] = u.z;
        matrix[3, 0] = 0;

        matrix[0, 1] = v.x;
        matrix[1, 1] = v.y;
        matrix[2, 1] = v.z;
        matrix[3, 1] = 0;

        matrix[0, 2] = n.x;
        matrix[1, 2] = n.y;
        matrix[2, 2] = n.z;
        matrix[3, 2] = 0;

        matrix[0, 3] = this.transform.position.x;
        matrix[1, 3] = this.transform.position.y;
        matrix[2, 3] = this.transform.position.z;
        matrix[3, 3] = 1;

        Matrix4x4 matrixInv = matrix.inverse;

        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        matrix = simulationCamera.worldToCameraMatrix;
        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
        return matrix;
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
