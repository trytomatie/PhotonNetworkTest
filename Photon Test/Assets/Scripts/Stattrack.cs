using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class Stattrack : MonoBehaviourPunCallbacks
{
    public Dictionary<int,StattrackItem> players = new Dictionary<int, StattrackItem>();

    public static Stattrack instance;

    private void Start()
    {
        instance = this;
        
    }

    public void CreatePlayers()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            players.Add(player.ActorNumber, new StattrackItem(player.NickName));
        }
    }
    public void AddKill(int id)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            players[id].kills++;
            photonView.RPC("SynchKill", RpcTarget.Others, id, players[id].kills);
        }

    }
    [PunRPC]
    public void SynchKill(int id,int value)
    {
        players[id].kills = value;
    }

    public void AddDeath(int id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            players[id].deaths++;
            photonView.RPC("SynchDeath", RpcTarget.Others, id, players[id].deaths);
        }

    }

    [PunRPC]
    public void SynchDeath(int id, int value)
    {
        players[id].deaths = value;
    }

    public void AddDamageDealt(int id,int amount)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            players[id].dammageDealt++;
            photonView.RPC("SynchDamageDealt", RpcTarget.Others, id, players[id].dammageDealt);
        }
    }
    [PunRPC]
    public void SynchDamageDealt(int id, int value)
    {
        players[id].dammageDealt = value;
    }


}
