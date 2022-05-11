using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMachine<State, StateInfo>
    where State : MachineState<StateInfo>, new()
    where StateInfo : MachineStateInfo
{
    private State baseState;

    public float updatePeriod = 0.3f;
    private float updateTimer = 0;

    public bool showDebug = true;

    public void Update(StateInfo infos)
    {
        if (baseState == null)
        {
            baseState = new State();
            return;
        }

        updateTimer += Time.deltaTime;
        if (updateTimer > updatePeriod)
        {
            infos.updatePeriod = updatePeriod;
            baseState.showDebug = showDebug;
            baseState.Update(ref infos);

            updateTimer = 0;
        }
    }
}
