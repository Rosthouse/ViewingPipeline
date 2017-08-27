using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFrustumRenderer : MonoBehaviour {

    private Camera camera;

	// Use this for initialization
	void Start () {
        this.camera = GetComponent<Camera>();	
	}
	

    private void OnPostRender()
    {
        RenderFrustum.DrawFrustum(camera);
    }
}
