using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Tap : MonoBehaviour
{

    private GestureRecognizer recognizer;

    // Use this for initialization
    async void Start()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += GestureRecognizer_Tapped;
        recognizer.StartCapturingGestures();
    }

    private async void GestureRecognizer_Tapped(TappedEventArgs obj)
    {
#if ENABLE_WINMD_SUPPORT        
        string uriToLaunch = @"ms-voip-video:?contactids=bf538576-xxxx-yyyy-zzzz-1d548b9e7010";
        Debug.Log("LaunchUriAsync: " + uriToLaunch);

        Uri uri = new Uri(uriToLaunch);
        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        {
            // Work done on the UI thread
            LaunchURI(uri).ConfigureAwait(false).GetAwaiter().GetResult();
        });
#endif
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
