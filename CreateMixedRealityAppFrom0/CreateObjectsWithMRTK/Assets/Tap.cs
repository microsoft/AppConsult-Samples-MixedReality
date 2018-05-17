using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap : MonoBehaviour, IInputHandler
{
    void IInputHandler.OnInputDown(InputEventData eventData)
    {    }

    void IInputHandler.OnInputUp(InputEventData eventData)
    {
        var o = GameObject.CreatePrimitive(PrimitiveType.Cube); 
        o.transform.localScale = new Vector3(1f, 0.1f, 1f); 
        o.transform.position = new Vector3(0f, 2f, 4f); 
        o.AddComponent<Rigidbody>(); 
    }

    // Use this for initialization
    void Start()
    {    }

    // Update is called once per frame
    void Update()
    {    }
}
