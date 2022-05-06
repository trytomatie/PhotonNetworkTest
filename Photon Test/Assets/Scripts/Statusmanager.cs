using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class Statusmanager : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum Faction {Human,Zombie,Door};

    public Faction faction;
    public int hp = 10;
    public int maxHp = 10;

    public float invultime = 0;
    private bool isInvulnarable = false;
    public UnityEvent deathEvent = new UnityEvent();
    public UnityEvent damageEvent = new UnityEvent();
    public UnityEvent onHpUpdate = new UnityEvent();


    private void Start()
    {
        damageEvent.AddListener(Test);
    }

    void Test()
    {

    }
    public void TakeDamage(int damage,GameObject source)
    {
        if (isInvulnarable)
        {
            return;
        }
        int id = -1;
        if(source.tag == "Player")
        {
            id = source.GetComponent<PlayerController>().id;
        }
        photonView.RPC("TakeDamageNetwork", RpcTarget.All, damage, id);
        if (invultime > 0)
        {
            Invoke("ResetInvulnerableTimer", invultime);
        }
    }

    public void ResetInvulnerableTimer()
    {
        isInvulnarable = false;
    }

    [PunRPC]
    private void TakeDamageNetwork(int damage,int id)
    {
        damageEvent.Invoke();
        Hp -= damage;
        if(id != -1)
        {
            Stattrack.instance.AddDamageDealt(id, damage);
        }
        if(Hp <= 0)
        {
            if (id != -1)
            {
                Stattrack.instance.AddKill(id);
            }
            if (gameObject.tag == "Player")
            {
                id = gameObject.GetComponent<PlayerController>().id;
                Stattrack.instance.AddDeath(id);
            }
            deathEvent.Invoke();
        }
        if(invultime>0)
        {
            isInvulnarable = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(Hp);
        }
        else if(stream.IsReading)
        {
            Hp = (int)stream.ReceiveNext();
        }
    }

    public void BasicDeathEvent(float time)
    {
        GetComponent<Collider>().enabled = false;
        if(photonView.IsMine)
        {
            Invoke("DestroyObject", time);
        }
    }

    public void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public int Hp 
    { 
        get => hp;
        set
        {
            hp = value;
            onHpUpdate.Invoke();
        }

    }


}
