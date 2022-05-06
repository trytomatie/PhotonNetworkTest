using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour
{
    private Material myMaterial;
    public LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = lr.material;
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.SetFloat("_Alpha", Mathf.Clamp01(myMaterial.GetFloat("_Alpha") - 0.2f * Time.deltaTime));
    }
}
