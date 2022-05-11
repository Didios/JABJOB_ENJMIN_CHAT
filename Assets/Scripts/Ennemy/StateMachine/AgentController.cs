using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    /* Control NavMeshAgent */

    [SerializeField] private NavMeshAgent agent;

    public bool hasAnim = false;
    [SerializeField] private Animator animator;

    public bool showDebug = true;

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (hasAnim && animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void FindPathTo(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    public bool CanReachPoint(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);

        return path.status == NavMeshPathStatus.PathComplete;
    }

    public bool IsArrived(Vector3 target, float distance)
    {
        var _pos = agent.transform.position;
        var _tar = target;
        _pos.y = 0;
        _tar.y = 0;

        return ((_tar - _pos).sqrMagnitude < distance * distance);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canMouv">Number between -1/2,-1 make move if wait, 0 for wait, 1 for walf and 2 for run</param>
    public void SetMove(int canMouv)
    {
        if (canMouv == -1)
        {
            if (agent.isStopped)
                canMouv = 1;
            else
                return;
        }
        
        if (canMouv <= 0)
        {
            agent.isStopped = true;
            if (hasAnim)
            {
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
            }
        }
        else if (canMouv == 1)
        {
            agent.isStopped = false;
            if (hasAnim)
            {
                animator.SetBool("walk", true);
                animator.SetBool("run", false);
            }
        }
        else if (canMouv >= 2)
        {
            agent.isStopped = false;
            if (hasAnim)
            {
                animator.SetBool("walk", true);
                animator.SetBool("run", true);
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (showDebug)
        {
            float height = agent.height;
            
            if (agent.hasPath)
            {
                Vector3[] corners = agent.path.corners;
                if (corners.Length >= 2)
                {
                    Gizmos.color = Color.red;
                    for (int i = 1; i < corners.Length; i++)
                    {
                        Gizmos.DrawLine(corners[i - 1] + Vector3.up * height / 2, corners[i] + Vector3.up * height / 2);
                    }
                }
            }
        }
    }
}
