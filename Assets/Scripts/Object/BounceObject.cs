using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    public float strenght = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<FPSController>() != null)
        {
            collision.transform.GetComponent<FPSController>().AddForce(transform.up * strenght);
            Debug.Log("[BounceObject]:\n Character");
        }
        else
        {
            // don't work on character controller
            collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * strenght, ForceMode.Impulse);
            Debug.Log("[BounceObject]:\n Object");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<FPSController>() != null)
        {
            other.transform.GetComponent<FPSController>().AddForce(transform.up * strenght);
            Debug.Log("[BounceObject]:\n Character");
        }
        else
        {
            // don't work on character controller
            other.transform.GetComponent<Rigidbody>().AddForce(transform.up * strenght, ForceMode.Impulse);
            Debug.Log("[BounceObject]:\n Object");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
