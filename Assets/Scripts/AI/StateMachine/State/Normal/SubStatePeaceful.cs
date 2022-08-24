using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubStatePeaceful : MachineState<MachineStateInfo>
{
    private float timer = 0;
    private float waitingTime = -1;
    private bool init = true;

    public override void DoState(ref MachineStateInfo infos)
    {
        timer += infos.updatePeriod;

        if (init || timer > waitingTime || infos.isArrived)
        {
            if (infos.isArrived)
            {
                AddAndActivateSubState<SubStateWait>();

                infos.isArrived = false;
            }
            else
            {
                AddAndActivateSubState<SubStateWalk>();
            }

            timer = 0;
            waitingTime = Random.Range(infos.cooldownMinMaxWait.x, infos.cooldownMinMaxWait.y);
            init = false;
        }

        keepMeAlive = !infos.isNoise;// true;
    }
}
