using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Tap : MonoBehaviour
{

    private GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += GestureRecognizer_Tapped;
        recognizer.StartCapturingGestures();
    }

    private void GestureRecognizer_Tapped(TappedEventArgs obj)
    {
        var o = GameObject.CreatePrimitive(PrimitiveType.Cube); //a
        o.transform.localScale = new Vector3(1f, 0.1f, 1f); //b
        o.transform.position = new Vector3(0f, 2f, 4f); //c
        o.AddComponent<Rigidbody>(); //d
    }

    // Update is called once per frame
    void Update()
    {

    }
}
