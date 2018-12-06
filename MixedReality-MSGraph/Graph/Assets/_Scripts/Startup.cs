using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
#if UNITY_WSA && !UNITY_EDITOR
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Identity.Client;
#endif

public class Startup : MonoBehaviour
{
    // Use this for initialization
    async void Start()
    {
        await GoGraph();
    }

    private async Task GoGraph()
    {
#if UNITY_WSA && !UNITY_EDITOR
        // Get the authentication token using Microsoft.Identity.Client
        bool authenticated = false;
        PublicClientApplication client = new PublicClientApplication("b6cb4849-fd27-4366-9dfb-c9f1bd99d35a");
        AuthenticationResult authResult = null;
        IEnumerable<string> Scopes = new List<string> { "User.Read", "User.ReadBasic.All" };
        try
        {
            authResult = await client.AcquireTokenAsync(Scopes);
            //FOVFeedback.instance.ModifyText($"UniqueId={authResult.UniqueId}, AccessToken={authResult.AccessToken}");
            authenticated = true;
        }
        catch (MsalException msaEx)
        {
            FOVFeedback.instance.ModifyText($"Failed to acquire token: {msaEx.Message}");
        }
        catch (Exception ex)
        {
            FOVFeedback.instance.ModifyText($"Failed to acquire token: {ex.Message}");
        }


        if (authenticated)
        {
            // Query MS Graph using a HTTP GET
            string GraphUriAndQuery = @"https://graph.microsoft.com/v1.0/users?$filter=startswith(Displayname,'Marty')&$select=id";
            var uri = new System.Uri(GraphUriAndQuery);
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    string result = await httpClient.GetStringAsync(uri);

                    Rootobject root = new Rootobject();
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
                    root = ser.ReadObject(ms) as Rootobject;
                    FOVFeedback.instance.ModifyText($"id={root.value[0].id}");
                }
                catch (Exception ex)
                {
                    FOVFeedback.instance.ModifyText($"Failed to query MS Graph: {ex.Message}");
                }
            }
        }
#else
        FOVFeedback.instance.ModifyText(":-( The app does not work in the Editor");
#endif
    }
}



public class Rootobject
{
    public string odatacontext { get; set; }
    public Value[] value { get; set; }
}

public class Value
{
    public string id { get; set; }
}