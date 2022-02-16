using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : MonoBehaviour
{
    // light
    public Light light_;

    // infos
    private Collider collider_;

    private void Start()
    {
        collider_ = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (light_.enabled) { light_.enabled = false; }
        else { light_.enabled = true; }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }
}
