using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHunt : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        infos.textUI.text = "";

        if (!infos.isCatch)
        {
            AddAndActivateSubState<SubStateRunning>();
        }
        else
        {
            AddAndActivateSubState<SubStateOutside>();
        }
    }
}
