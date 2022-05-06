using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Destroyed duplicate Network Manager");
            Destroy(gameObject);
        }
        PhotonNetwork.IsMessageQueueRunning = false;

    }

    private void Start()
    {
        // Connect to master server
        PhotonNetwork.ConnectUsingSettings();
    }



    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

#region Callback Logic
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }
    #endregion
}











