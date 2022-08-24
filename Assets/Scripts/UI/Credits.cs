using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public Transform creditsToMove;

    public Vector3 posStartLocal;
    public Vector3 posEndLocal;
    private Vector3 dir;

    public float speed;
    public float distMin;

    public bool inMove = false;

    private void Start()
    {
        dir = (posEndLocal - posStartLocal).normalized;
    }

    private void Update()
    {
        if (inMove)
        {
            creditsToMove.Translate(dir * speed * Time.deltaTime);

            if (Vector3.Distance(creditsToMove.position, transform.position + posEndLocal) < distMin)
            {
                inMove = false;
            }
        }
    }

    public void StartCredits()
    {
        creditsToMove.localPosition = posStartLocal;
        inMove = true;
    }

    public void EndCredits()
    {
        creditsToMove.localPosition = posEndLocal;
        inMove = false;
    }

    private void OnDrawGizmosSelected()
    {
        var _start = transform.position + posStartLocal;
        var _end = transform.position + posEndLocal;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_start, 10);

        Gizmos.DrawLine(_start, _end);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_end + (_start - _end).normalized * distMin, 10);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_end, 10);
    }
}
