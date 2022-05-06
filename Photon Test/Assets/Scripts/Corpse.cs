using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Corpse : MonoBehaviour
{

    public static Dictionary<Corpse, float> corpses = new Dictionary<Corpse, float>();
    public static int maxCorpses = 20;
    public float corpseExplosionStrength = 25;
    public Rigidbody[] myRbs;
    private void Awake()
    {
        foreach (Rigidbody rb in myRbs)
        {
            rb.AddExplosionForce(corpseExplosionStrength, transform.position + transform.forward, 2.3f, 0f,ForceMode.VelocityChange);
        }
        if(corpses.Count < maxCorpses)
        {
            corpses.Add(this, Time.time);
        }
        else
        {

            corpses.Add(this, Time.time);
            for (int i = corpses.Count - maxCorpses; i >= 0;i--)
            {
                Corpse oldestCorpse = corpses.OrderBy(n => n.Value).First().Key;
                corpses.Remove(oldestCorpse);
                Destroy(oldestCorpse.gameObject);
            }

        }
    }
    private void OnDestroy()
    {
    }
}
