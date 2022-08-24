using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenObject : MonoBehaviour
{
    public int nbrRegen = 1;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<FPSController>().bulletMax += nbrRegen; //countNbrProj -= nbrRegen;
            Destroy(gameObject);
        }
    }
}
