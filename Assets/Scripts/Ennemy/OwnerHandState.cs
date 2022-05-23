using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* work with state machine */
public class OwnerHandState : MonoBehaviour
{
    [Header("Hand")]
    public Vector3 grabPosition;
    public Transform tempTrans;

    [Header("Owner")]
    public StateInfoController ownerController;
    private MachineStateInfo ownerInfos;

    [Space][Tooltip("Put if isAnim")] public Animator animator;

    [Header("Player")]
    public Transform player;
    private Rigidbody playerRigidBody;
    private FPSController playerController;
    private SphereCollider playerCollider;

    [Header("Escape")]
    private int pressCounter = -1;
    public int pressToMake = 5;
    public float timeToPress = 5;
    [SerializeField][Tooltip("Don't touch")] private float timer = 0;
    public float strengthRelease = 10f;

    [Header("Test infos")]
    public bool showDebug = false;
    public bool isAnim = false;
    public bool hasGrabbed = false;
    public bool isActive = true;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        playerRigidBody = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<FPSController>();
        playerCollider = player.GetComponent<SphereCollider>();

        ownerInfos = ownerController.infos;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isActive && hasGrabbed)
        {
            if (Input.GetButtonDown("Jump"))
            {
                pressCounter++;
            }

            if (showDebug) Debug.Log($"[OwnerHand]\n {timer}");

            if (pressCounter == -1)
            {
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;

                if (pressCounter >= pressToMake)
                {
                    pressCounter = -1;
                    timer = 0;

                    Release();
                }
                else if (timer > timeToPress)
                {
                    pressCounter = -1;
                }
            }
        }
        else
        {
            if (hasGrabbed)
            {
                Release();
            }

            pressCounter = -1;
            timer = 0;
        }
    }

    void Catch()
    {
        hasGrabbed = true;
        ownerInfos.isCatch = true;

        player.parent = transform;

        player.position = transform.position
            + transform.forward * grabPosition.x
            + transform.right * grabPosition.z
            + transform.up * grabPosition.y;

        playerController.activate = false;
        playerCollider.isTrigger = true;
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        player.rotation = transform.rotation;

        if (isAnim)
        {
            animator.SetBool("catch", true);
        }
    }

    void Release()
    {
        hasGrabbed = false;
        ownerInfos.isCatch = false;

        player.parent = tempTrans;

        playerController.activate = true;
        playerCollider.isTrigger = false;
        playerRigidBody.constraints = RigidbodyConstraints.None;

        playerRigidBody.AddForce(transform.forward * strengthRelease, ForceMode.Impulse);
        playerRigidBody.AddForce(transform.up * strengthRelease, ForceMode.Impulse);
        playerController.activate = true;

        if (isAnim)
        {
            animator.SetBool("catch", false);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject && !ownerInfos.isStunned)
        {
            Catch();
            if (showDebug) Debug.Log("[OwnerHand]\n Owner grabbed Player");
        }
        else
        {
            if (showDebug) Debug.Log("[OwnerHand]\n owner touch something"); 
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject && !ownerInfos.isStunned)
        {
            Catch();
            if (showDebug) Debug.Log("[OwnerHand]\n owner grabbed Player");
        }
        else
        {
            if (showDebug) Debug.Log("[OwnerHand]\n owner touch something");
        }
    }

    private void OnDrawGizmos()
    {
        var position = transform.position
            + transform.forward * grabPosition.x
            + transform.right * grabPosition.z
            + transform.up * grabPosition.y;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position, 0.1f);
    }
}
