using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Elevator_Event : MonoBehaviour
{

    public Vector3 startPosition;
    public Vector3 endPosition;

    public float time = 45;

    public UnityEvent endEvent;

    private float distance;
    private bool eventStarted = false;
    private bool eventEnded = false;



    // Start is called before the first frame update
    public void StartEvent()
    {
        distance = Vector3.Distance(startPosition, endPosition);
        transform.localPosition = startPosition;
        eventStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(eventStarted && !eventEnded)
        {
            transform.localPosition = (transform.localPosition - new Vector3(0, (distance/ time) * Time.deltaTime, 0));
            if(transform.localPosition.y <= endPosition.y)
            {
                eventEnded = true;
                GameManager.instance.SpawnEventText("Get in the Elevator!");
                endEvent.Invoke();
            }
        }
    }

    
}
