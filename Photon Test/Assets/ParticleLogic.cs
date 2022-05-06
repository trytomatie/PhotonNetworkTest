using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLogic : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject, 10);
            this.enabled = false;
            return;
        }
        transform.position = target.position;
    }
}
