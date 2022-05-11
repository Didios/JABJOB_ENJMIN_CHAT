using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLight : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        infos.agent.SetMove(-1);

        var _pos = infos.switchLight.transform.position;
        if (infos.agent.IsArrived(_pos, infos.distanceMinToTarget))
        {
            infos.switchLight.Switch();
        }
        else
        {
            infos.agent.FindPathTo(_pos);
        }
    }
}
