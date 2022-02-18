using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientableObject : MonoBehaviour
{
    public float angleMaxX = 0;
    public float angleMaxY = 0;
    public float angleMaxZ = 0;
    [SerializeField]
    private Vector3 turnTo = Vector3.zero;

    public float forceMax = 1;


    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(turnTo), 0.05f);
    }

    public void AddTurn(Vector3 force)
    {
        turnTo.x += force.x * angleMaxX / forceMax;
        turnTo.y += force.y * angleMaxY / forceMax;
        turnTo.z += force.z * angleMaxZ / forceMax;

        if (turnTo.x > angleMaxX) { turnTo.x = angleMaxX; }
        if (turnTo.y > angleMaxY) { turnTo.y = angleMaxY; }
        if (turnTo.z > angleMaxZ) { turnTo.z = angleMaxZ; }

        if (turnTo.x < -angleMaxX) { turnTo.x = -angleMaxX; }
        if (turnTo.y < -angleMaxY) { turnTo.y = -angleMaxY; }
        if (turnTo.z < -angleMaxZ) { turnTo.z = -angleMaxZ; }
    }
}
