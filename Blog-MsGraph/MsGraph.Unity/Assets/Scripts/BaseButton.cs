using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour, IInputClickHandler  
{
    protected Action<string> updateTextContent;
    protected Text textContent;

	protected virtual void Start () 
	{
	    this.textContent = GameObject.Find("TextContent").GetComponent<Text>();
        
	    updateTextContent = (s) =>
	    {
	        if (textContent != null)
	        {
	            textContent.text += Environment.NewLine + s;
	        }
	        else
	        {
                Debug.Log(Environment.NewLine + s);
	        }
	    };
	}

    public abstract void OnInputClicked(InputClickedEventData eventData);
}
