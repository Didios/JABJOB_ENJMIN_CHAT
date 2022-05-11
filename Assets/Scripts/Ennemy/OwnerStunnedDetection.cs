using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerStunnedDetection : MonoBehaviour
{
    private MachineStateInfo ownerInfos;

    public string tagToStunned = "PukeBall";

    public void Start()
    {
        ownerInfos = GetComponent<StateInfoController>().infos;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == tagToStunned)
        {
            Destroy(collision.gameObject);
            ownerInfos.isStunned = true;
        }
    }
}
