using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PathFinding : MonoBehaviour
{
    // agent
    public Transform[] points;
    private NavMeshAgent agent;
    private Rigidbody agentRigidbody;
    private int indexDestPoint = 0;

    // target
    public bool goToPriority = false;
    public Vector3 targetPriority;

    // player
    public bool goToPlayer = false;
    public Transform player;

    // collision
    private bool colliderDetectionStatus = false;
    public float stunnedTime = 0;
    public float waitingTime = 5;

    // message
    public TextMeshProUGUI message;

    public bool activate = false;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agentRigidbody = GetComponent<Rigidbody>();

        if (activate)
        {
            GotoNextPoint();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PukeBall"))// If the GameObject's tag matches the one you suggest, output this message in the console
        {
            colliderDetectionStatus = true;
            Destroy(collision.gameObject); // the collision object is remove

            // stop the agent
            agentRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            agent.isStopped = true;

            Debug.Log("[PathFinding]\n owner hit by PukeBall");

            // show to the player
            message.text = "L'homme est étourdi";
        }
    }

    private void GotoNextPoint()
    {
        if (activate)
        {
            if (goToPriority)
            {
                agent.destination = targetPriority;
                goToPriority = false;
            }
            else if (goToPlayer)
            {
                agent.destination = player.position;
                if (agent.remainingDistance < 0.1f) { goToPlayer = false; }
            }
            else
            {
                // Returns if no points have been set up
                if (points.Length == 0)
                    return;

                // Set the agent to go to the currently selected destination.
                agent.destination = points[indexDestPoint].position;

                // Choose the next point in the array as the destination,
                // cycling to the start if necessary.
                indexDestPoint = (indexDestPoint + 1) % points.Length;
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (activate)
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (colliderDetectionStatus)
            {
                if (stunnedTime > waitingTime)
                {
                    // make the agent moveable
                    agentRigidbody.constraints = RigidbodyConstraints.None;
                    agent.isStopped = false;

                    // reset composant Timer
                    colliderDetectionStatus = false;
                    stunnedTime = 0;

                    // delete message for player
                    message.text = "";
                }
                else { stunnedTime += Time.deltaTime; }
            }
            else
            {
                if (goToPriority || goToPlayer) { GotoNextPoint(); }
                else if (!agent.pathPending && agent.remainingDistance < 0.5f) { GotoNextPoint(); }
            }
        }
        else
        {
            // stop the agent
            agentRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            agent.isStopped = true;

            colliderDetectionStatus = true;
            stunnedTime = 100;
        }
    }

    private void OnDrawGizmos()
    {
        // display the path take by the owner
        Gizmos.color = Color.blue;
        for (int i = 0; i < points.Length ; i++)
        {
            if (i == points.Length - 1) { Gizmos.DrawLine(points[i].position, points[0].position); }
            else { Gizmos.DrawLine(points[i].position, points[i+1].position); }

            Gizmos.DrawSphere(points[i].position, 1);
        }
    }
}
