using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class SpawnManager : MonoBehaviourPun
{
    public bool isActive = false;
    [Header("Enemies")]
    public string[] enemyPrefabLocations;

    [Header("Spawns")]
    public List<SpawnPointLogic> spawnPoints;

    [Header("Trigger")]
    public Trigger startTrigger;
    public Trigger[] hordeTrigger;

    private float passiveSpawnTimer = 4;
    private System.Random random = new System.Random();

    public static SpawnManager instnace;

    public static List<Statusmanager> zombies = new List<Statusmanager>();

    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
            return;
        }
        
        instnace = this;
        InvokeRepeating("PassiveSpawn", passiveSpawnTimer, passiveSpawnTimer);

    }

    public void PassiveSpawn()
    {
        if(!isActive)
            return;

        PlayerController player = GameManager.instance.players[Random.Range(0, GameManager.instance.players.Length)];
        var eligibleSpawnPoints = spawnPoints
            .Where(p => Vector3.Distance(p.transform.position, player.transform.position) < 33 && p.isAvailable)
            .OrderBy(p => random.Next());
        SpawnPointLogic spawnpoint = eligibleSpawnPoints.First();
        switch (Random.Range(0, 100))
        {
            case > 90:
                SpawnZombie(2, spawnpoint.transform.position);
                break;
            case > 80:
                SpawnZombie(1, spawnpoint.transform.position);
                break;
            default:
                SpawnZombie(0, spawnpoint.transform.position);
                break;
        }

    }

    public void GetSpawnpoints(Transform center)
    {

    }
    public void SpawnZombie(int enemyId,Vector3 spawnPos)
    {
        zombies.RemoveAll(e => e.Hp <= 0);
        if (zombies.Count > 50)
        {
            return;
        }
        GameObject obj;
        obj = PhotonNetwork.Instantiate(enemyPrefabLocations[enemyId], spawnPos, Quaternion.identity);
        zombies.Add(obj.GetComponent<Statusmanager>());
    }

    

    public void SpawnHorde()
    {
        InvokeRepeating("SpawnHordeEnemy", 0.5f, 0.3f);
        Invoke("StopHorde", 10f);

    }

    public void StopHorde()
    {
        CancelInvoke("SpawnHordeEnemy");
    }

    public void SpawnHordeEnemy()
    {
        PlayerController player = GameManager.instance.players[Random.Range(0, GameManager.instance.players.Length)];
        var eligibleSpawnPoints = spawnPoints
            .Where(p => Vector3.Distance(p.transform.position, player.transform.position) < 43 && p.isAvailable)
            .OrderBy(p => random.Next());
        SpawnPointLogic spawnpoint = eligibleSpawnPoints.First();
        SpawnZombie(0, spawnpoint.transform.position);
        switch (Random.Range(0, 100))
        {
            case > 90:
                SpawnZombie(2, spawnpoint.transform.position);
                break;
            case > 80:
                SpawnZombie(1, spawnpoint.transform.position);
                break;
            default:
                SpawnZombie(0, spawnpoint.transform.position);
                break;
        }
    }

}
