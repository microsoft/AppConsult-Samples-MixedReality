using HoloToolkit.Unity.InputModule;
using MsGraph.Library;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public sealed class LoginButton : BaseButton
{
    public string ApplicationId;
    private IMsaService msaService = null;

    protected override void Start()
    {
        base.Start();
        msaService = new MsaService();
        msaService.Init(ApplicationId);
    }

    public override async void OnInputClicked(InputClickedEventData eventData)
    {
        await msaService.SignInAsync(updateTextContent);
    }
}
