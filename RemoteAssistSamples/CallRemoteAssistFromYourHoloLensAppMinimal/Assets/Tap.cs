using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private async void GestureRecognizer_Tapped(TappedEventArgs obj)
    {
        string uriToLaunch = @"ms-voip-video:?contactids=bf538576-8ada-46d8-83ff-1d548b9e7010";
        Debug.Log("LaunchUriAsync: " + uriToLaunch);

        var uri = new Uri(uriToLaunch);
        await LaunchURI(uri);
    }



    private async Task LaunchURI(System.Uri uri)
    {
    #if ENABLE_WINMD_SUPPORT
    // Launch the URI
    try
    {
        var success = await Windows.System.Launcher.LaunchUriAsync(uri);

        if (success)
        {
            Debug.Log("URI launched");
        }
        else
        {
            Debug.Log("URI launch failed");
        }
    }
    catch (Exception ex)
    {
        Debug.Log(ex.Message);
    }
    #endif
    }
}