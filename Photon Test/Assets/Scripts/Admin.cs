using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Admin : MonoBehaviourPunCallbacks
{


    private void Start()
    {
        if(!photonView.IsMine)
        {
            this.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.M))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit);
            if(raycastHit.point != null)
            {
                SpawnZombie(raycastHit.point);
            }
   
        }
    }

    public void SpawnZombie(Vector3 position)
    {
        PhotonNetwork.Instantiate(GameManager.instance.zombiePrefabLocation, position, Quaternion.identity);
    }
}
