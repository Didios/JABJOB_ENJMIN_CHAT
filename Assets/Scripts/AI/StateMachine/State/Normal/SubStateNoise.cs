using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateNoise : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        infos.agent.SetMove(1);
        infos.agent.FindPathTo(infos.positionNoise);

        if (infos.agent.IsArrived(infos.positionNoise, infos.distanceMinToTarget))
        {
            infos.isNoise = false;
        }
    }
}
