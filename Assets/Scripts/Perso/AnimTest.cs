using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
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
                if (hit.rigidbody != null) { hit.rigidbody.AddForce(transform.forward * (-(strengthMax / distMax) * hit.distance + strengthMax)); }
                Debug.Log("Hit something with a strength of :\n" + (-(strengthMax / distMax) * hit.distance + strengthMax));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distMax);
    }
}
