using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun;

public class Interactor : MonoBehaviourPunCallbacks
{
    public List<Interactable> interactables = new List<Interactable>();
    public FollowTargetUI namePlate;
    public TextMeshProUGUI text;
    public Interactable closestInteractable = null;

    private void Start()
    {
        if(!photonView.IsMine)
        {
            Destroy(this);
        }
        namePlate = GameObject.Find("InteractionNamePlate").GetComponent<FollowTargetUI>();
        text = GameObject.Find("InteractionNamePlate").GetComponent<TextMeshProUGUI>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            interactables.Add(other.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            interactables.Remove(other.GetComponent<Interactable>());
    }

    private void Update()
    {
        List<Interactable> deads = new List<Interactable>();
        deads.AddRange(interactables.Where(e => e == null || e.enabled == false).ToList());
        foreach(Interactable dead in deads)
        {
            interactables.Remove(dead);
        }
        if(interactables.Count> 0)
        {
            closestInteractable = interactables.OrderBy(n => Vector2.Distance(n.transform.position, transform.position)).First();
            namePlate.target = closestInteractable.transform;
            namePlate.offset = closestInteractable.interactionNamePlateOffset;
            text.text = "[E] " + closestInteractable.interactionName;
        }
        else
        {
            closestInteractable = null;
            namePlate.target = null;
        }
   
    }

    public void Interact()
    {
        if(closestInteractable != null)
        {
            closestInteractable.TriggerInteraction(gameObject);
        }
    }
}
