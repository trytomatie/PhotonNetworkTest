using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetUI : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private Camera cam;

    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        cam = Camera.main;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(target == null)
        {
            rectTransform.position = new Vector3(10000, 1000, 10000);
        }
        else
        {
            rectTransform.position = cam.WorldToScreenPoint(target.transform.position + offset);
        }


    }
}
