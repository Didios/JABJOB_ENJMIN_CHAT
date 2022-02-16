using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EphemeralObject : MonoBehaviour
{
    public float timeBeforeHide;
    public bool usePosition = false;
    public Vector3 position;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBeforeHide)
        {
            if (usePosition) { transform.position = position; }
            else { Destroy(gameObject); }
        }

    }
}
