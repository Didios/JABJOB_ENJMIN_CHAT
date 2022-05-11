using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* can work alone */
public class OwnerHand : MonoBehaviour
{
    // player
    public Transform player;
    private Rigidbody playerRigidBody;
    private FPSController playerController;
    private SphereCollider playerCollider;
    public bool playerGrabbed = false;

    // hand
    public Animator animator;
    public Transform handPosition;
    public Vector3 handVector;
    public Transform tempTrans;

    // other
    private float pressCounter = -1;
    public int escapeCount = 30;
    public float m_Thrust = 10f;

    // info
    public bool lose = true;
    public bool finish = false;
    public bool activate = true;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        playerRigidBody = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<FPSController>();
        playerCollider = player.GetComponent<SphereCollider>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerGrabbed = true;
            Debug.Log("[OwnerHand]\n owner grabbed Player");
            //colliderDetectionStatus = true;
            //If the GameObject's name matches the one you suggest, output this message in the console
        }
        else { Debug.Log("[OwnerHand]\n owner touch something"); }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerGrabbed = true;
            Debug.Log("[OwnerHand]\n owner grabbed Player");
            //colliderDetectionStatus = true;
            //If the GameObject's name matches the one you suggest, output this message in the console
        }
        else { Debug.Log("[OwnerHand]\n owner touch something"); }
    }

    void ChangeParent()
    {
        animator.SetBool("inHand", true);

        player.parent = handPosition;

        var position = handPosition.transform.position;
        position += handPosition.transform.right * handVector.x;
        position += handPosition.transform.forward * handVector.z;
        position += handPosition.transform.up * handVector.y;
        player.position = position;

        playerController.activate = false;
        playerCollider.isTrigger = true;
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    //Revert the parent of object 2.
    void RevertParent()
    {
        animator.SetBool("inHand", false);

        playerRigidBody.constraints = RigidbodyConstraints.None;
        playerCollider.isTrigger = false;
        player.parent = tempTrans;

        playerRigidBody.AddForce(-playerRigidBody.transform.forward * m_Thrust, ForceMode.Impulse);
        playerRigidBody.AddForce(playerRigidBody.transform.up * m_Thrust, ForceMode.Impulse);
        playerController.activate = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if (activate)
        {
            if (playerGrabbed)
            {
                ChangeParent();

                if (pressCounter == -1) { pressCounter = escapeCount; }

                if (Input.GetButtonDown("Jump"))
                {
                    pressCounter++;
                    Debug.Log($"[OwnerHand]\n pressCounter : {pressCounter}");
                }

                pressCounter -= Time.deltaTime * 5f;

                if (pressCounter < 0)
                {
                    playerGrabbed = false;
                    RevertParent();
                    pressCounter = -1;
                    finish = true;

                    Debug.Log("[OwnerHand]\n Player grab by the Owner");
                }

                if (pressCounter > escapeCount)
                {
                    playerGrabbed = false;
                    RevertParent();
                    pressCounter = -1;

                    Debug.Log("[OwnerHand]\n Player escaped from Owner");
                }
            }
        }
        else if (!playerController.activate)
        {
            playerGrabbed = false;
            finish = false;
        }
    }

    private void OnDrawGizmos()
    {
        var position = handPosition.transform.position;
        position += handPosition.transform.right * handVector.x;
        position += handPosition.transform.forward * handVector.z;
        position += handPosition.transform.up * handVector.y;

        Gizmos.DrawWireSphere(position, 0.1f);
    }
}
