using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;
using TMPro;

public class ZombieAI : MonoBehaviourPunCallbacks
{
    public Transform target;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;

    public float moveSpeed = 12;
    private Statusmanager myStatus;

    private float distanceToTarget;

    private TextMeshProUGUI nameplate;
    private void Start()
    {
        myStatus = GetComponent<Statusmanager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public void SelectTarget()
    {
        Transform newTarget = target;
        float distance = 10000;
        foreach(PlayerController player in GameManager.instance.players)
        {

            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (playerDistance < distance)
            {
                newTarget = player.transform;
                distance = playerDistance;
            }
        }
        distanceToTarget = distance;
        target = newTarget;
    }
    private void Update()
    {
        if (!photonView.IsMine)
            return;

        SelectTarget();
        navMeshAgent.destination = target.transform.position;
        rb.velocity = navMeshAgent.velocity.normalized * moveSpeed;
    }

    private void OnCollisionStay(Collision collision)
    {
        Statusmanager otherStatus = collision.gameObject.GetComponent<Statusmanager>();
        if (otherStatus != null && myStatus.faction != otherStatus.faction)
        {
            otherStatus.TakeDamage(3,gameObject);
        }
    }

    public void DeeatachCorpse(Transform corpse)
    {
        corpse.parent = null;
    }


}
