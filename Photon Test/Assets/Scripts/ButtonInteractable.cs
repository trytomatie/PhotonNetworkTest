using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class ButtonInteractable : Interactable
{
    public UnityEvent buttonEvent;
    public string message;
    public override void TriggerInteraction(GameObject interactor)
    {
        photonView.RPC("InvokeButtonEvent", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void InvokeButtonEvent()
    {
        buttonEvent.Invoke();
    }

    public void TriggerMessage()
    {
        GameManager.instance.SpawnEventText(message);
    }
}
