using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStunned : MachineState<MachineStateInfo>
{
    private float timer = 0.0f;
    private bool cooldownSet = false;

    public override void DoState(ref MachineStateInfo infos)
    {
        infos.agent.SetMove(0);

        if (timer > infos.cooldownStunned)
        {
            infos.textUI.text = "";
            infos.isStunned = false;
        }
        else
        {
            infos.textUI.text = "Owner is stunned !";
            if (!cooldownSet)
            {
                infos.cooldownBar.SetCooldown(infos.cooldownStunned);
                cooldownSet = true;
            }
            keepMeAlive = true; // don't lose the timer
        }

        timer += infos.updatePeriod; // in state machine, update is make with infos.updatePeriod
    }
}
