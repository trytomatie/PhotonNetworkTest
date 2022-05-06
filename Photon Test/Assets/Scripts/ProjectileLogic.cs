using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    public Statusmanager origin;
    public bool isArmed = false;
    private void OnTriggerEnter(Collider other)
    {
       
        if(isArmed)
        {
            Statusmanager otherStatus = other.GetComponent<Statusmanager>();
            if(otherStatus != null && origin.faction != otherStatus.faction)
            {
                otherStatus.TakeDamage(1,origin.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
