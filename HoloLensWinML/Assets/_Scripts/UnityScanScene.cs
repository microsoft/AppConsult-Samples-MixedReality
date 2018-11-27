using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Interface gives one method to implement. This method modifies the text to display in Unity
/// Any code can then call this method outside the Unity MonoBehavior object
/// </summary>
public interface IUnityScanScene
{
	void ModifyOutputText(string newText);
}

public class UnityScanScene : MonoBehaviour, IUnityScanScene
{
	// Unity 3D Text object that contains 
	// the displayed TextMesh in the FOV
	public GameObject OutputText;
	// TextMesh object provided by the OutputText game object
	private TextMesh OutputTextMesh;
	// string to be affected to the TextMesh object
	private string OutputTextString = string.Empty;
	// Indicate if we have to Update the text displayed
	private bool OutputTextChanged = false;

#if UNITY_WSA && !UNITY_EDITOR
    private ScanEngine CameraScanEngine;
#endif

	// Use this for initialization
	async void Start ()
	{
		OutputTextMesh = OutputText.GetComponent<TextMesh>();
        OutputTextMesh.text = string.Empty;

#if UNITY_WSA && !UNITY_EDITOR // RUNNING ON WINDOWS
        CameraScanEngine = new ScanEngine();
        await CameraScanEngine.Inititalize(this);
		CameraScanEngine.StartPullCameraFrames();
#else                          // RUNNING IN UNITY
        ModifyOutputText("Sorry ;-( The app is not supported in the Unity player.");
#endif
	}

	/// <summary>
	/// Modify the text to be displayed in the FOV
	/// or/and in the debug traces
	/// + Indicate that we have to update the text to display
	/// </summary>
	/// <param name="newText">new string value to display</param>
	public void ModifyOutputText(string newText)
	{
		OutputTextString = newText;
		OutputTextChanged = true;
	}

	// Update is called once per frame
	void Update ()
    {
		if (OutputTextChanged)
		{
			OutputTextMesh.text = OutputTextString;
			OutputTextChanged = false;
		}
	}

}
