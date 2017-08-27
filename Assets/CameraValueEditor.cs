using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraValueEditor : MonoBehaviour {

    [SerializeField] private Properties cameraProperties;
    [SerializeField] private InputField focalLenght;
    [SerializeField] private InputField nearClip;
    [SerializeField] private InputField farClip;
    [SerializeField] private InputField wX;
    [SerializeField] private InputField wY;
    [SerializeField] private InputField wW;
    [SerializeField] private InputField wH;

    [SerializeField] private Toggle isOrtho;
    public void Start()
    {
        focalLenght.text = cameraProperties.D.ToString();
        nearClip.text = cameraProperties.dMin.ToString();
        farClip.text = cameraProperties.dMax.ToString();
        wX.text = cameraProperties.Window.position.x.ToString();
        wY.text = cameraProperties.Window.position.y.ToString();
        wW.text = cameraProperties.Window.size.x.ToString();
        wH.text = cameraProperties.Window.size.y.ToString();

        focalLenght.onEndEdit.AddListener(SetFocalLenght);
        nearClip.onEndEdit.AddListener(SetNearClip);
        farClip.onEndEdit.AddListener(SetFarClip);
        isOrtho.onValueChanged.AddListener(IsOrthoGraphic);

        wX.onEndEdit.AddListener(SetWindowX);
        wY.onEndEdit.AddListener(SetWindowY);
        wW.onEndEdit.AddListener(SetWindowW);
        wH.onEndEdit.AddListener(SetWindowH);
    }


    public void SetFocalLenght(string f)
    {
        cameraProperties.D = float.Parse(f);
    }

    public void SetNearClip(string f)
    {
        cameraProperties.dMin = float.Parse(f);
    }

    public void SetFarClip(string f)
    {
        cameraProperties.dMax= float.Parse(f);
    }

    public void IsOrthoGraphic(bool isOrhto)
    {
        cameraProperties.IsOrtho = isOrhto;
    }

    public void SetWindowX(string f)
    {
        Rect window = cameraProperties.Window;
        cameraProperties.Window = new Rect(new Vector2(float.Parse(f), window.position.y), window.size);
    }

    public void SetWindowY(string f)
    {
        Rect window = cameraProperties.Window;
        cameraProperties.Window = new Rect(new Vector2(window.position.x, float.Parse(f)), window.size);
    }

    public void SetWindowW(string f)
    {
        Rect window = cameraProperties.Window;
        cameraProperties.Window = new Rect(window.position, new Vector2(float.Parse(f), window.size.y));
    }

    public void SetWindowH(string f)
    {
        Rect window = cameraProperties.Window;
        cameraProperties.Window = new Rect(window.position, new Vector2(window.size.x, float.Parse(f)));
    }
}
