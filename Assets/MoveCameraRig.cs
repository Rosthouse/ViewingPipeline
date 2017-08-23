using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraRig : MonoBehaviour {

    private Rigidbody body;
    [SerializeField] private float speed = 1;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (axis != Vector2.zero)
        {
            this.transform.Translate(new Vector3( axis.x, 0, axis.y) * speed);
        }
	}
}
