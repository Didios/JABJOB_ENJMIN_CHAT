using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    // speed
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    // saut
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

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

    // projectile
    public float speedProj = 5;
    public Transform crachat;
    [HideInInspector]
    public bool canMove = true;

    // tir
    private float shootTimer = 0;
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
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // course ?
            bool isRunning = Input.GetButton("Fire3");

            // mouvement du joueur
            float curSpeedX = 0;
            float curSpeedY = 0;
            if (canMove)
            {
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
            }

            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // on gère le saut
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded) { moveDirection.y = jumpSpeed; }
            else { moveDirection.y = movementDirectionY; }

            // on gère le tir
            if (Input.GetButton("Fire1") && shootTimer <= 0)
            {
                shootTimer = shootCooldown;
                Throw();
            }
            if (shootTimer > 0) { shootTimer -= Time.deltaTime; }

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
    }

    private void Throw()
    {
        //Création du projectile au bon endroit
        Transform proj = GameObject.Instantiate<Transform>(crachat,
            transform.position + transform.forward * 1.5f, transform.rotation);
        //Ajout d une impulsion de départ
        proj.GetComponent<Rigidbody>().AddForce(transform.forward * speedProj, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        // debug direction du personnage
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        Gizmos.DrawSphere(transform.position + transform.forward * 1.5f, 0.2f);
    }
}