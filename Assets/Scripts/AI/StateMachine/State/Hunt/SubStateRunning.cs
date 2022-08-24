using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateRunning : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        infos.agent.SetMove(2);
        infos.agent.FindPathTo(infos.posLastSee);

        if (infos.agent.IsArrived(infos.posLastSee, infos.distanceMinToTarget))
        {
            infos.isArrivedToLastSee = true;
        }
    }
}
