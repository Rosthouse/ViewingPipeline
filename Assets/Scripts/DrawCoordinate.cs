using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCoordinate : MonoBehaviour {


    [SerializeField] float size = 5;

	// Update is called once per frame
	void Update () {
        RenderFrustum.DrawCoordinateSystem(this.transform.position, this.transform.rotation, size);
	}
}
