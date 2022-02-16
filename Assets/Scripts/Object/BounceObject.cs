using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    public float strenght = 10;

    private void OnCollisionEnter(Collision collision)
    {
        // don't work on character controller
        collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * strenght, ForceMode.Impulse);
    }
}
