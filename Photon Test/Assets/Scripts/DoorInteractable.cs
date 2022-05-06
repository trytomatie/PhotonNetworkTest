using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DoorInteractable : Interactable
{

    public Animator anim;
    private bool isOpen = false;
    private bool front = false;

    public List<Rigidbody> doorParts;
    public override void TriggerInteraction(GameObject interactor)
    {
        var heading = interactor.transform.position - transform.position;
        float dotProduct = Vector3.Dot(heading, transform.forward);
        if (dotProduct > 0)
        {
            front = false;
        }
        else
        {
            front = true;
        }

        if (isOpen)
        {
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }
        photonView.RPC("OpenDoor", RpcTarget.All,isOpen,front);
    }

    [PunRPC]
    private void OpenDoor(bool p_isOpen, bool p_front)
    {
        anim.SetBool("IsOpen", p_isOpen);
        isOpen = p_isOpen;
        anim.SetBool("Front", p_front);
        p_front = p_isOpen;
    }

    public void BreakOffParts()
    {
        if(doorParts.Count > 2)
        {
            doorParts[0].isKinematic = false;
            doorParts[0].transform.parent = null;
            doorParts.Remove(doorParts[0]);
            

            doorParts[1].isKinematic = false;
            doorParts.Remove(doorParts[1]);
        }

    }

    public void BreakDoor()
    {
        foreach(Rigidbody doorpart in doorParts)
        {
            doorpart.transform.parent = null;
            doorpart.isKinematic = false;
        }
        Destroy(this);
    }


}
