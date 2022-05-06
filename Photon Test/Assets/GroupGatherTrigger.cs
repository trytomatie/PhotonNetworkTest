using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class GroupGatherTrigger : MonoBehaviourPunCallbacks
{
    int playerCount = 0;

    public UnityEvent triggerEvent;
    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerCount++;
            if(playerCount >= GameManager.instance.players.Length)
            {
                triggerEvent.Invoke();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCount--;
        }
    }
}
