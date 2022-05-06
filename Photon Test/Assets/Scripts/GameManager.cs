using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("Stats")]
    public bool gameEnded = false;
    public bool gameHasStarted = false;

    [Header("Players")]
    public string playerPrefabLocation;
    public GameObject playerNameplate;
    public GameObject eventText;
    public GameObject generalHud;
    public GameObject endScreen;
    public Transform[] spawnPoints;
    public PlayerController[] players;

    private int playersInGame = 0;

    public static GameManager instance;



    [Header("Zombies")]
    public string zombiePrefabLocation;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate Gamemanager Destroyed");
            Destroy(gameObject);
        }

    }

    public void EndGame()
    {
        gameEnded = true;
        generalHud.SetActive(false);
        endScreen.SetActive(true);
    }

    private void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("PlayerHasJoinedGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void PlayerHasJoinedGame()
    {
        playersInGame++;

        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            gameHasStarted = true;
            SpawnPlayer();
            Stattrack.instance.CreatePlayers();
        }
    }

    private void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public void SpawnNamePlate(PlayerController target)
    {
        GameObject namePlateObj = Instantiate(playerNameplate, GameObject.Find("NamePlates").transform);
        namePlateObj.GetComponent<TextMeshProUGUI>().text = target.photonPlayer.NickName;
        namePlateObj.GetComponent<FollowTargetUI>().target = target.transform;
    }

    public void SpawnEventText(string text)
    {
        GameObject eventTextObj = Instantiate(eventText, GameObject.FindObjectOfType<Canvas>().transform);
        eventTextObj.GetComponent<TextMeshProUGUI>().text = text;
        Destroy(eventTextObj, 7f);
    }
    public PlayerController GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId);
    }

    public PlayerController GetPlayer(GameObject playerObj)
    {
        return players.First(x => x.gameObject == playerObj);
    }
}
