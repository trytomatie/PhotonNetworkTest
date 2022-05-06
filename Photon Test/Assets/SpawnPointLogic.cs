using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class SpawnPointLogic : MonoBehaviour
{
    public bool isAvailable;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.players[0] != null)
         isAvailable = GameManager.instance.players.Any(p => Vector3.Distance(transform.position, p.transform.position) > 10);

    }
}
