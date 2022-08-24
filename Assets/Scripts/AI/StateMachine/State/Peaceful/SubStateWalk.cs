using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateWalk : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        Vector3 target = infos.agent.transform.position
            + Random.insideUnitSphere * Random.Range(infos.distanceMinMax.x, infos.distanceMinMax.y);
        
        while (!infos.agent.CanReachPoint(target))
        {
            target = infos.agent.transform.position
                + Random.insideUnitSphere * Random.Range(infos.distanceMinMax.x, infos.distanceMinMax.y);
        }

        infos.agent.SetMove(1);
        infos.agent.FindPathTo(target);

        if (infos.agent.IsArrived(target, infos.distanceMinToTarget))
        {
            infos.isArrived = true;
        }
    }
}
