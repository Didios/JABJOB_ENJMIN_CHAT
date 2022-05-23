using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public float distMax;
    public float strengthMax;

    [SerializeField]
    private Animator _animator;

    public bool showDebug = false;

    void Start()
    {
        if (_animator == null) { _animator = GetComponent<Animator>(); }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            // punch animation launch
            _animator.SetTrigger("punch");

            // punch In-Game
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distMax))
            {
                var force = -(strengthMax / distMax) * hit.distance + strengthMax;

                var hitTurn = hit.transform.GetComponent<OrientableObject>();
                var hitBreak = hit.transform.GetComponent<BreakableObject>();

                if (hitTurn != null)
                {
                    hitTurn.AddTurn(transform.forward * force);
                }

                if (hitBreak != null)
                {
                    hitBreak.Touch();
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
                }

                if (showDebug) Debug.Log("[Punch]:\n Strength : " + force);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distMax);
    }
}
