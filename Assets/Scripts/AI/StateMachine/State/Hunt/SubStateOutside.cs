using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStateOutside : MachineState<MachineStateInfo>
{
    public override void DoState(ref MachineStateInfo infos)
    {
        infos.textUI.text = "Jump to escape !";

        infos.agent.SetMove(1);
        infos.agent.FindPathTo(infos.outside);

        if (infos.agent.IsArrived(infos.outside, infos.distanceMinToTarget))
        {
            infos.gameOver = true;
        }
    }
}
