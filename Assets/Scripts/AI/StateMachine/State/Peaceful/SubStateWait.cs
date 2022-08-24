using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateWait : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        infos.agent.SetMove(0);
    }
}
