using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class CameraLocker : MonoBehaviour {

    private Toggle t;
    private FreeLookCam freeLookCam;

	// Use this for initialization
	void Start () {
        t = GetComponent<Toggle>();
        t.onValueChanged.AddListener(this.ValueChanged);
        this.freeLookCam = FindObjectOfType<FreeLookCam>();
        this.ValueChanged(t.isOn);
	}

    private void ValueChanged(bool arg0)
    {
        if (arg0)
        {
            t.GetComponentInChildren<Text>().text = "CTRL to unlock";
            this.freeLookCam.Lock();
        }
        else
        {
            t.GetComponentInChildren<Text>().text = "Lock Camera";
        }
    }
}
