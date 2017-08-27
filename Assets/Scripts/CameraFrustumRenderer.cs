using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFrustumRenderer : MonoBehaviour {

    
    private Camera camera;
    [SerializeField] private Properties cameraProperties;

	// Use this for initialization
	void Start () {
        if(cameraProperties == null){
            this.camera = GetComponent<Camera>();	
        }
	}
	

    private void Update()
    {
        if(cameraProperties == null){
            RenderFrustum.DrawFrustum(camera);
        }
        else{
            RenderFrustum.DrawFrustum(cameraProperties.GetNearClipPlane(this.transform), cameraProperties.GetFarClipPlane(this.transform));
        }
    }


}
