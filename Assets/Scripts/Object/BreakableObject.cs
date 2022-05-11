using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Space]
    [Tooltip("Break when touch a zone with tag")] public bool usingZone = true;
    public string tagDestroy = "DeathZone";
    [Space]
    [Tooltip("Break on collision")] public bool usingTouch = false;

    [Header("UI")]
    public SatisfactionBar bar;

    [Header("Other infos")]
    [SerializeField] private bool hasBreak = false;
    [SerializeField] private bool hasOutline = true;
    [SerializeField] private bool hasNoise = false;
    private Outline visibility;

    [SerializeField][Tooltip("first time invincible")] private float TimerInvincibility = 5;

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
        if (TimerInvincibility >= 0)
        {
            TimerInvincibility -= Time.deltaTime;
        }

        if ((ancientSpeed > limitSpeed && rigidBody.velocity.magnitude < limitSpeed && usingLimit) || // if the speed exceeds the limit if ther is a limit
            breakObject || // if the code call to break
            (ancientSpeed > rigidBody.velocity.magnitude && !usingLimit)) // if the object slow down, in case of no speed limit
        {
            Break();
        }

        ancientSpeed = rigidBody.velocity.magnitude;

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

    private void Break()
    {
        if (!hasBreak && TimerInvincibility < 0)
        {
            if (weight > 0)
            {
                bar.UpdateBar(weight);
            }

            if (breakedObject != null)
            {
                // creation of the break object at the exact same position
                var obj = GameObject.Instantiate<Transform>(breakedObject, transform.position, transform.rotation);

                // give the same parent
                obj.parent = transform.parent;
                //obj.localScale = transform.localScale; // scaling (be aware) ne marche pas car les fragments sont en scale 1 alors que les obj blender sont en scale 100
                obj.GetComponent<Rigidbody>().velocity = rigidBody.velocity;

                if (hasNoise)
                {
                    obj.GetComponent<FlashNoiseObject>().isActive = true;
                }

                // delete the ancient object
                Destroy(gameObject);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == tagDestroy)
        {
            Destroy(gameObject);
        }
        else if (usingTouch && !hold)
        {
            Break();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == tagDestroy) { Destroy(gameObject); }
    }
}
