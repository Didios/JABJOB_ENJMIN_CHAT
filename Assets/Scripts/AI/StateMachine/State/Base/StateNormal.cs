using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateNormal : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        if (infos.isNoise)
        {
            AddAndActivateSubState<SubStateNoise>();
        }
        else
        {
            AddAndActivateSubState<SubStatePeaceful>();
        }
    }
}

