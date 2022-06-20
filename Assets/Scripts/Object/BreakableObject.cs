using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BreakableObject : MonoBehaviour
{
    [Header("Object")]
    private Rigidbody rigidBody;
    private Collider collider_;
    [Tooltip("Breaked version of the object")] public Transform breakedObject;
    [Tooltip("Break by script")] public bool breakObject = false;
    [Tooltip("weight on bar")] public int weight = 1;
    public bool holdeable = false;
    [HideInInspector] public bool hold = false;

    [Header("Break type")]
    [Tooltip("Break when velocity decrease")] public bool usingLimit = true;
    public float limitSpeed = 2;
    private float ancientSpeed = 0;
    private bool canBreak = false;
    [Space]
    [Tooltip("Break when touch a zone with tag")] public bool usingZone = true;
    public string tagDestroy = "DeathZone";
    [Space]
    [Tooltip("Break on collision")] public bool usingTouch = false;

    [Header("UI")]
    public bool useBar = true;
    public SatisfactionBar bar;

    [Header("Other infos")]
    [SerializeField] private bool useParentScale = false;
    [SerializeField] private bool hasBreak = false;
    [SerializeField] private bool hasOutline = true;
    [SerializeField] private bool hasNoise = false;
    private Outline visibility;

    public bool autoDestroy = false;
    [SerializeField] [Tooltip("first time invincible")] private float TimerInvincibility = 5;

    [Space]
    public bool showDebug = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        collider_ = GetComponent<Collider>();

        if (hasOutline) // check outline
        {
            visibility = GetComponent<Outline>();
            if (visibility == null)
            {
                visibility = gameObject.AddComponent<Outline>();

                visibility.OutlineColor = Color.red;
                visibility.OutlineMode = Outline.Mode.OutlineAll;
                visibility.OutlineWidth = 10;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Invincibility Timer
        if (TimerInvincibility >= 0)
        {
            TimerInvincibility -= Time.deltaTime;
        }
        else if (autoDestroy)
        {
            Destroy(gameObject);
        }

        // Break with velocity
        if (usingLimit)
        {
            if ((ancientSpeed > limitSpeed && rigidBody.velocity.magnitude < limitSpeed && usingLimit) || // if the speed exceeds the limit if ther is a limit
                (ancientSpeed > rigidBody.velocity.magnitude)) // if the object slow down, in case of no speed limit
            {
                canBreak = true;// Break();
            }

            ancientSpeed = rigidBody.velocity.magnitude;
        }

        // Break with Code
        if (breakObject)
        {
            Break();
        }

        // Holdeable object
        if (holdeable)
        {
            if (hold)
            {
                transform.rotation = new Quaternion();
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                collider_.enabled = false;
            }
            else
            {
                rigidBody.constraints = RigidbodyConstraints.None;
                collider_.enabled = true;
            }
        }
    }

    private void Break()
    {
        if (!hasBreak && TimerInvincibility < 0)
        {
            if (weight > 0)
            {
                if (useBar)
                {
                    bar.UpdateBar(weight);
                }
            }

            if (breakedObject != null)
            {
                // in case object cannot be destroyed
                try
                {
                    Transform obj = null;
                    if (useParentScale)
                    {
                        obj = GameObject.Instantiate<Transform>(breakedObject, transform.position, transform.rotation, transform.parent);
                    }
                    else
                    {
                        // creation of the break object at the exact same position
                        obj = GameObject.Instantiate<Transform>(breakedObject, transform.position, transform.rotation);
                        // give the same parent
                        obj.parent = transform.parent;
                    }


                    //obj.localScale = transform.localScale; // scaling (be aware) ne marche pas car les fragments sont en scale 1 alors que les obj blender sont en scale 100

                    // set velocity to make illusion
                    if (rigidBody != null)
                    {
                        var rigid = obj.GetComponent<Rigidbody>();
                        if (rigid != null)
                        {
                            rigid.velocity = rigidBody.velocity;
                        }
                        else
                        {
                            foreach (Rigidbody childRigid in obj.GetComponentsInChildren<Rigidbody>())
                            {
                                if (childRigid != null)
                                {
                                    childRigid.velocity = rigidBody.velocity;
                                }
                            }
                        }
                    }

                    if (hasNoise)
                        obj.GetComponent<FlashNoiseObject>().isActive = true;

                    // delete the ancient object
                    Destroy(gameObject);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                if (hasOutline)
                {
                    visibility.enabled = false;
                }
                if (hasNoise)
                {
                    GetComponent<FlashNoiseObject>().isActive = true;
                }
            }

            if (showDebug) Debug.Log($"[BreakableObject]\n {transform.name} Break");
            hasBreak = true;
        }
    }

    public void Freeze()
    {
        transform.rotation = new Quaternion();
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        collider_.enabled = false;
    }

    public void UnFreeze()
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        collider_.enabled = true;
    }

    public void Touch()
    {
        if ((usingTouch && holdeable && !hold) ||
            (usingTouch && !holdeable))
        {
            if (TimerInvincibility < 0)
            {
                Break();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(tagDestroy))
        {
            Break();
            Destroy(gameObject);
        }
        else
        {
            Touch();
        }

        if (TimerInvincibility > 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            if (canBreak)
            {
                Break();
            }
        }
    }

    /*
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag(tagDestroy)) { Destroy(gameObject); }
    }
    */
}