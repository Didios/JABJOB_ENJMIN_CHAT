using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStunned : MachineState<MachineStateInfo>
{
    private float timer = 0.0f;

    public override void DoState(ref MachineStateInfo infos)
    {
        infos.agent.SetMove(0);

        timer += infos.updatePeriod; // in state machine, update is make with infos.updatePeriod
        if (timer > infos.cooldownStunned)
        {
            infos.textUI.text = "";
            infos.isStunned = false;
        }
        else
        {
            infos.textUI.text = "Owner is stunned !";
            infos.cooldownBar.SetCooldown(infos.cooldownStunned);
            keepMeAlive = true; // don't lose the timer
        }
    }
}
