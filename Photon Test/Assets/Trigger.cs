using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Trigger : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool hasBeenTriggered = false;
    public bool playerTrigger = true;
    public string triggerText;
    public PlayerController triggerPlayer;
    public bool isHordeTrigger = true;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(HasBeenTriggered);
        }
        else if (stream.IsReading)
        {
            HasBeenTriggered = (bool)stream.ReceiveNext();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasBeenTriggered)
        {
            return;
        }
        if (playerTrigger)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            triggerPlayer = other.GetComponent<PlayerController>();
        }
        photonView.RPC("TriggerInteraction", RpcTarget.All,triggerPlayer.id);
    }

    private void TriggerMessage(string text)
    {
        GameManager.instance.SpawnEventText(string.Format(triggerText, text));
    }

    [PunRPC]
    private void TriggerInteraction(int id)
    {
        TriggerMessage(GameManager.instance.GetPlayer(id).photonPlayer.NickName);
        HasBeenTriggered = true;
    }

    public bool HasBeenTriggered 
    { get => hasBeenTriggered;
        set
        {
            if(value != hasBeenTriggered)
            {
                if(PhotonNetwork.IsMasterClient && isHordeTrigger)
                {
                    SpawnManager.instnace.SpawnHorde();
                }
            }
            hasBeenTriggered = value;
        }
    }
}
