using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public Vector3 posStartLocal;
    public Vector3 posEndLocal;
    private Vector3 posStart { get { return transform.position + posStartLocal; } set { } }
    private Vector3 posEnd { get { return transform.position + posEndLocal; } set { } }
    private Vector3 dir;

    public float speed;
    public float distMin;

    public bool inMove = false;

    private void Start()
    {
        dir = (posEnd - posStart).normalized;
    }

    private void Update()
    {
        if (inMove)
        {
            transform.Translate(dir * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, posEnd) < distMin)
            {
                inMove = false;
            }
        }
    }

    public void StartCredits()
    {
        transform.position = posStart;
        inMove = true;
    }

    public void EndCredits()
    {
        transform.position = posEnd;
        inMove = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(posStart, 10f);

        Gizmos.DrawLine(posStart, posEnd);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(posEnd, 10f);
    }
}
