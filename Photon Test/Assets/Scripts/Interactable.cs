using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Interactable : MonoBehaviourPunCallbacks
{
    public string interactionName;
    public Vector3 interactionNamePlateOffset = new Vector3(0,-0.25f,0);

    public PhotonView myView;

    private void Start()
    {

    }
    public virtual void TriggerInteraction(GameObject interactor)
    {

    }
}
