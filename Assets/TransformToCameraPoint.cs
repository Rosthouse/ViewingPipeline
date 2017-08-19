using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformToCameraPoint : MonoBehaviour {

    [SerializeField] private Transform target;
    private Vector3 startDistance;
    private Quaternion startRotation;

    private void Start()
    {
        startDistance = this.transform.position - target.transform.position;
        startRotation = Quaternion.FromToRotation( target.transform.forward, this.transform.forward);
    }


    // Update is called once per frame
    void Update () {
        
        Matrix4x4 localM =  Matrix4x4.TRS(this.transform.position, this.transform.rotation, Vector3.one);
        Matrix4x4 targetM = Matrix4x4.TRS(this.target.position + startDistance, startRotation, Vector3.one);

        Matrix4x4 m = targetM;

        // Extract new local position
        Vector3 position = m.GetColumn(3);

        // Extract new local rotation
        Quaternion rotation = Quaternion.LookRotation(
            m.GetColumn(2),
            m.GetColumn(1)
        );

        // Extract new local scale
        Vector3 scale = new Vector3(
            m.GetColumn(0).magnitude,
            m.GetColumn(1).magnitude,
            m.GetColumn(2).magnitude
        );


        
        this.transform.SetPositionAndRotation(position, rotation);
        this.transform.localScale = scale;
	}
}
