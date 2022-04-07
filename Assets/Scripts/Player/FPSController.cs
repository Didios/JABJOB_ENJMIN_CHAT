using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    [Header("Move")]
    // speed

    public AnimationCurve speedCurve;
    [SerializeField]
    private float time = 0;
    /*
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    //*/
    // saut
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    [Header("Camera")]
    // camera
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    //public float lookYLimit = 45.0f;

    // deplacement
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float rotationY = 0;
    private Vector3 forceToAdd = Vector3.zero;

    // fear something
    [Header("Fear")]
    public string tagFear = "Water";
    public float forceUp = 5;
    public float forceBack = 10;

    [Header("Projectile")]
    // projectile
    public float speedProj = 5;
    public Transform crachat;
    public int maxProj = 5;
    public int countNbrProj = 0;
    [HideInInspector]
    public bool canMove = true;

    // hold something
    [HideInInspector]
    public bool holdSomething = false;
    [Header("Holding")]
    private Transform holdObj;
    public float distGrab;
    public Vector3 posHoldObj;

    // tir
    private float shootTimer = 0;
    [Header("Fire")]
    public float shootCooldown = 2;

    public bool activate = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (activate)
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // course ?
            bool isRunning = Input.GetButton("Fire3");

            // mouvement du joueur
            float curSpeedX = 0;
            float curSpeedY = 0;
            if (canMove)
            {
                if (isRunning && time < 1)
                {
                    time += 0.03f;
                }
                else if (!isRunning && time > 0)
                {
                    time -= 0.2f;
                }

                if (time > 1) time = 1;
                else if (time < 0) time = 0;

                
                curSpeedX = speedCurve.Evaluate(time) * Input.GetAxis("Vertical");
                curSpeedY = speedCurve.Evaluate(time) * Input.GetAxis("Horizontal");
                /*
                if (isRunning)
                {
                    curSpeedX = runningSpeed * Input.GetAxis("Vertical");
                    curSpeedY = runningSpeed * Input.GetAxis("Horizontal");
                }
                else
                {
                    curSpeedX = walkingSpeed * Input.GetAxis("Vertical");
                    curSpeedY = walkingSpeed * Input.GetAxis("Horizontal");
                }
                //*/
            }

            float movementDirectionY = moveDirection.y;
            moveDirection = (transform.forward * curSpeedX) + (transform.right * curSpeedY);

            // on gère le saut
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded) { moveDirection.y = jumpSpeed; }
            else { moveDirection.y = movementDirectionY; }

            // on gère le hold
            if (Input.GetButtonDown("Fire2"))
            {
                if (!holdSomething)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, distGrab))
                    {
                        BreakableObject obj = hit.transform.GetComponent<BreakableObject>();
                        if (obj != null)
                        {
                            if (obj.holdeable)
                            {
                                holdObj = hit.transform;
                                obj.hold = true;

                                holdObj.position = transform.position + transform.forward * posHoldObj.x + transform.right * posHoldObj.y + transform.up * posHoldObj.z;
                                hit.transform.parent = transform;

                                holdSomething = true;
                                Debug.Log("[FPSController]:\n Grab something");
                            }
                        }
                    }
                }
            }
            // on gère le tir
            if (Input.GetButtonDown("Fire1") && shootTimer <= 0)
            {
                if (holdSomething)
                {
                    holdObj.parent = transform.parent;
                    holdObj.GetComponent<BreakableObject>().hold = false;

                    Throw(holdObj, false);
                    holdSomething = false;
                }
                else if (countNbrProj < maxProj)
                {
                    Throw(crachat, true);
                    countNbrProj += 1;
                }

                shootTimer = shootCooldown;
            }
            if (shootTimer > 0) { shootTimer -= Time.deltaTime; }

            moveDirection += forceToAdd;
            forceToAdd = Vector3.zero;

            // gravité
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationY += Input.GetAxis("Mouse X") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                //rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
                //transform.rotation *= Quaternion.Euler(Input.GetAxis("Mouse Y") * lookSpeed, Input.GetAxis("Mouse X") * lookSpeed, 0);
                transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
            }
        }
        else if (!Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        forceToAdd += force;
    }

    private void Throw(Transform obj, bool new_)
    {
        if (new_)
        {
            //Création du projectile au bon endroit
            Transform proj = GameObject.Instantiate<Transform>(obj,
                transform.position + transform.forward * 1.5f, transform.rotation);

            //Ajout d une impulsion de départ
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * speedProj, ForceMode.Impulse);
        }
        else
        {
            // repossitionnement
            obj.position = transform.position + transform.forward * 1.5f;

            //Ajout d une impulsion de départ
            obj.GetComponent<Rigidbody>().AddForce(transform.forward * speedProj, ForceMode.Impulse);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == tagFear)
        {
            Debug.Log("OUCH !");
            Vector3 forceFear = -transform.forward * forceBack;
            forceFear.y = forceUp;

            AddForce(forceFear);
        }
    }

    private void OnDrawGizmos()
    {
        // debug position de lancement
        Gizmos.DrawSphere(transform.position + transform.forward * 1.5f, 0.2f);
        // debug position tenir objet
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + 
            transform.forward * posHoldObj.x +
            transform.right * posHoldObj.y +
            transform.up * posHoldObj.z, 0.2f);

        // debug direction du personnage avec distance hold
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distGrab);
    }
}