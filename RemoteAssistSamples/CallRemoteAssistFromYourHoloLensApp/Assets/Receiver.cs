using HoloToolkit.Unity.Receivers;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Receiver : InteractionReceiver
{
    protected override void FocusEnter(GameObject obj, PointerSpecificEventData eventData)
    {
        Debug.Log("FocusEnter:" + obj.name);
    }

    protected override async void InputUp(GameObject obj, InputEventData eventData)
    {
        Debug.Log("InputUp for: " + obj.name);
#if ENABLE_WINMD_SUPPORT
        if (obj.name.CompareTo("RemoteAssistvideoCall") == 0)
        {
            string uriToLaunch = @"ms-voip-video:?contactids=bf538576-8ada-46d8-83ff-1d548b9e7010";
            Debug.Log("LaunchUriAsync: " + uriToLaunch);

            var uri = new Uri(uriToLaunch);
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // Work done on the UI thread
                LaunchURI(uri).ConfigureAwait(false).GetAwaiter().GetResult();
            });
        }
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
