using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // object
    private Rigidbody rigidBody;
    public Transform breakedObject; // the version breaked of the object
    public bool breakObject = false; // to break the object with the code

    // speed
    public bool usingLimit = true;
    public float limitSpeed = 5; // the necessary speed to break the object in m/s
    private float ancientSpeed = 0;

    public ScoreBoard score;
    public SatisfactionBar bar;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((ancientSpeed > limitSpeed && rigidBody.velocity.magnitude < limitSpeed && usingLimit) || // if the speed exceeds the limit if ther is a limit
            breakObject || // if the code call to break
            (ancientSpeed > rigidBody.velocity.magnitude && !usingLimit)) // if the object slow down, in case of no speed limit
        {
            Break();
        }

        ancientSpeed = rigidBody.velocity.magnitude;
    }

    private void Break()
    {
        score.Increment();
        bar.AddBreakObj();

        // creation of the break object at the exact same position
        var obj = GameObject.Instantiate<Transform>(breakedObject, transform.position, transform.rotation);

        // give the same parent
        obj.parent = transform.parent;
        //obj.localScale = transform.localScale; // scaling (be aware)             ne marche pas car les fragments sont en scale 1 alors que les obj blender sont en scale 100
        obj.GetComponent<Rigidbody>().velocity = rigidBody.velocity;

        // delete the ancient object
        Destroy(gameObject);
    }
}
