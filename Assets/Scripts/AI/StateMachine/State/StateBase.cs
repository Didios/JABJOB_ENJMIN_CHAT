using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MachineState<MachineStateInfo>
{
    /* declare usefull variable */

    public override void DoState(ref MachineStateInfo infos)
    {
        /* put here what the state as to do 
         * or if it as substate, just make :
         * if ([condition with infos]) { AddAndActivateSubState<[substate to activate]>(); }
         */
        
        if (infos.isStunned)
        {
            AddAndActivateSubState<StateStunned>();
        }
        else if (infos.isLightOff)
        {
            AddAndActivateSubState<StateLight>();
        }
        else if (infos.canSeeTarget || !infos.isArrivedToLastSee || infos.isCatch)
        {
            AddAndActivateSubState<StateHunt>();
        }
        else
        {
            AddAndActivateSubState<StateNormal>();
        }

        keepMeAlive = true; // if it as to exist multiple time (always keep base alive)
    }
}
