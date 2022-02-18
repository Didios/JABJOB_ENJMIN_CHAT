using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public float distMax;
    public float strengthMax;

    [SerializeField]
    private Animator _animator;

    void Start()
    {
        if (_animator == null) { _animator = GetComponent<Animator>(); }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            // punch animation launch
            _animator.SetTrigger("punch");

            // punch In-Game
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distMax))
            {
                var hitTurn = hit.transform.GetComponent<OrientableObject>();

                if (hitTurn != null) { hitTurn.AddTurn(transform.forward * (-(strengthMax / distMax) * hit.distance + strengthMax)); }
                else if (hit.rigidbody != null) { hit.rigidbody.AddForce(transform.forward * (-(strengthMax / distMax) * hit.distance + strengthMax)); }

                Debug.Log("[Punch]:\n Strength : " + (-(strengthMax / distMax) * hit.distance + strengthMax));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distMax);
    }
}
