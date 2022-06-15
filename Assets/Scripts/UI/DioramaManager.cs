using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DioramaManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotation;

    [SerializeField] private float dist;

    [SerializeField] private Animator animator;
    [SerializeField] private List<Transform> diorama;
    [SerializeField] private Transform camera_;
    [SerializeField] private Transform cam_empty;

    private Quaternion rotBase;


    private int lockTarget = 0;
    public bool hasTarget { get; private set; }
    private bool canMove = false;
    public bool touchTarget = false;
    public bool goBase { get; private set; }

    private void Start()
    {
        rotBase = camera_.rotation;

        hasTarget = false;
        goBase = false;
    }

    void Update()
    {
        if (canMove)
        {
            if (hasTarget)
            {
                var _dir = (diorama[lockTarget].position - camera_.position).normalized;
                camera_.Translate(_dir * Time.deltaTime * speed, Space.World);
                camera_.rotation = Quaternion.Lerp(camera_.rotation, cam_empty.rotation, rotation);

                if (Vector3.Distance(camera_.position, diorama[lockTarget].position) < dist)
                {
                    camera_.gameObject.SetActive(false);
                    hasTarget = false;
                    canMove = false;

                    touchTarget = true;
                }
            }

            if (goBase)
            {
                var _dir = (diorama[lockTarget].position - cam_empty.position).normalized;
                camera_.Translate(_dir * Time.deltaTime * -speed, Space.World);

                if (Vector3.Distance(camera_.position, cam_empty.position) < dist)
                {
                    goBase = false;
                    canMove = false;

                    touchTarget = true;
                    animator.SetBool("loop", true);
                }
            }
        }

        canMove = animator.GetCurrentAnimatorStateInfo(0).IsName("empty");
    }

    public void ActiveCam()
    {
        camera_.gameObject.SetActive(true);
        Camera.SetupCurrent(camera_.GetComponent<Camera>());
    }

    public void SetTarget(int level)
    {
        cam_empty.LookAt(diorama[lockTarget]);

        animator.SetBool("loop", false);

        lockTarget = level;
        hasTarget = true;
        touchTarget = false;

        goBase = false;

        var _break = diorama[lockTarget].GetComponentInChildren<BreakableObject>();
        if (_break != null)
            _break.breakObject = true;
    }

    public void SetBase()
    {
        camera_.rotation = rotBase;

        animator.SetBool("loop", false);

        goBase = true;
        touchTarget = false;

        hasTarget = false;
    }
}
