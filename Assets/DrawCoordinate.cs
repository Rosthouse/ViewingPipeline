using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCoordinate : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        RenderFrustum.DrawCoordinateSystem(this.transform.position, this.transform.rotation, 5);
	}
}
