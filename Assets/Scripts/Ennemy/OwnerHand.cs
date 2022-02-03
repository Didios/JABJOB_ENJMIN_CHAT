using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerHand : MonoBehaviour
{
    // player
    public Transform player;
    private Rigidbody playerRigidBody;
    private FPSController playerController;
    public bool playerGrabbed = false;

    // hand
    public Transform handPosition;
    public Transform tempTrans;

    // other
    private float pressCounter = 0;
    public float m_Thrust = 10f;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        playerRigidBody = player.GetComponent<Rigidbody>();
        playerController = player.GetComponent<FPSController>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerGrabbed = true;
            Debug.Log("[OwnerHand]\n owner grabbed Player");
            //colliderDetectionStatus = true;
            //If the GameObject's name matches the one you suggest, output this message in the console
        }
    }

    void ChangeParent()
    {
        player.parent = handPosition;
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        playerController.activate = false;
    }

    //Revert the parent of object 2.
    void RevertParent()
    {
        playerRigidBody.constraints = RigidbodyConstraints.None;
        player.parent = tempTrans;

        playerRigidBody.AddForce(-playerRigidBody.transform.forward * m_Thrust, ForceMode.Impulse);
        playerRigidBody.AddForce(playerRigidBody.transform.up * m_Thrust, ForceMode.Impulse);
        playerController.activate = true;
    }

    // Update is called once per frame
    public void Update()
    {
        if (playerGrabbed)
        {
            ChangeParent();

            if (Input.GetButton("Jump"))
            {
                pressCounter++;
                Debug.Log(pressCounter);
            }

            if (pressCounter > 0) { pressCounter -= Time.deltaTime * 5f; }

            if (pressCounter > 15)
            {
                playerGrabbed = false;
                RevertParent();
                Debug.Log("[OwnerHand]\n Player escaped from Owner");
                pressCounter = 0;
            }
        }
    }
}
