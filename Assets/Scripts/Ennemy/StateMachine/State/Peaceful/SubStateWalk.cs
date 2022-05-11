using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateWalk : MachineState<MachineStateInfo>
{
    private Vector2 distanceMinMax = new Vector2(1, 10);

    public override void DoState(ref MachineStateInfo infos)
    {
        Vector3 target = infos.agent.transform.position
            + Random.insideUnitSphere * Random.Range(distanceMinMax.x, distanceMinMax.y);
        
        while (!infos.agent.CanReachPoint(target))
        {
            target = infos.agent.transform.position
                + Random.insideUnitSphere * Random.Range(distanceMinMax.x, distanceMinMax.y);
        }

        infos.agent.SetMove(1);
        infos.agent.FindPathTo(target);

        if (infos.agent.IsArrived(target, infos.distanceMinToTarget))
        {
            infos.isArrived = true;
        }
    }
}
