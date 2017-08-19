using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderFrustum : MonoBehaviour {
	private Camera camera;
	[SerializeField] private bool renderFrustum;

	public bool IsRendering {
		get { return this.renderFrustum; }
	}

	// Use this for initialization
	void Start () {
		this.camera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.renderFrustum && this.camera != null) {
			Gizmos.DrawFrustum (Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
		}
	}

    void OnDrawGizmos()
    {
        if (this.renderFrustum && this.camera != null)
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;

            float frustumHeight = camera.farClipPlane - camera.nearClipPlane;
            float viewPlane = frustumHeight / 2;

            Gizmos.color = Color.cyan;
            Gizmos.DrawFrustum(new Vector3(0, 0, camera.nearClipPlane), camera.fieldOfView, frustumHeight / 2 + camera.nearClipPlane, camera.nearClipPlane, camera.aspect);

            Gizmos.color = Color.white;
            Gizmos.DrawFrustum( new Vector3(0, 0, camera.nearClipPlane), camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
            
            //Gizmos.DrawWireCube(new Vector3(0, 0, viewPlane + camera.nearClipPlane) , new Vector3(camera.fieldOfView / viewPlane, camera.fieldOfView / viewPlane, 0));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero,  Vector3.up * 2);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.left * 2);
        }
    }
}
