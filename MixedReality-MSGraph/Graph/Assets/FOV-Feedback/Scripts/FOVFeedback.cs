using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Interface gives one method to implement. This method modifies the text to display in Unity
/// Any code can then call this method outside the Unity MonoBehavior object
/// </summary>
public interface IUnityFOVFeedback
{
    void ModifyOutputText(string newText);
}
public class FOVFeedback : MonoBehaviour {

    public static FOVFeedback instance;

    void Awake()
    {
        if (instance != null)
            GameObject.Destroy(instance);
        else
        {
            instance = this;
            instance.OutputTextMesh = this.GetComponentInParent<TextMesh>();
        }

        DontDestroyOnLoad(this);
    }


    // TextMesh object provided by the OutputText game object
    private TextMesh OutputTextMesh;
    // string to be affected to the TextMesh object
    private string OutputTextString = string.Empty;
    // Indicate if we have to Update the text displayed
    private bool OutputTextChanged = false;

    // Use this for initialization
    void Start ()
    {
        // The parent is the Unity 3D Text object that contains 
        // the displayed TextMesh in the FOV
        OutputTextMesh = this.GetComponentInParent<TextMesh>();
        OutputTextMesh.text = string.Empty;
        ModifyText("Hello :-)");
    }

    /// <summary>
    /// Modify the text to be displayed in the FOV
    /// or/and in the debug traces
    /// + Indicate that we have to update the text to display
    /// </summary>
    /// <param name="newText">new string value to display</param>
    public void ModifyText(string newText)
    {
        OutputTextString = newText;
        OutputTextChanged = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (OutputTextChanged)
        {
            OutputTextMesh.text = OutputTextString;
            OutputTextChanged = false;
        }
    }
}
