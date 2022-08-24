using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MachineState<StateInfo>
    where StateInfo : MachineStateInfo
{
    public string name = "Undefined";
    public bool keepMeAlive = false; // keep or not the state at the end of update

    public List<MachineState<StateInfo>> subStates = new List<MachineState<StateInfo>>();
    public MachineState<StateInfo> activeSubState = null;
    private static int recursionLevel = 0; // diplay hierarchical level on debug

    public bool showDebug = true;

    public MachineState()
    {
        name = GetType().ToString();
    }

    public void Update(ref StateInfo infos)
    {
        recursionLevel++;

        Log("Update");

        keepMeAlive = false; // explicit ask to keep alive in DoState
        DoState(ref infos);

        // unstack if no active substate
        if (activeSubState == null)
        {
            if (subStates.Count > 0)
            {
                activeSubState = subStates[subStates.Count - 1];
            }
        }

        // execute active substate
        if (activeSubState != null)
        {
            activeSubState.showDebug = showDebug;
            activeSubState.Update(ref infos);

            if (!activeSubState.keepMeAlive)
            {
                RemoveSubState(activeSubState);
                Log(activeSubState.name + " has ended");
            }
        }

        // keep alive if a substate want it
        foreach(MachineState<StateInfo> state in subStates)
        {
            if (state.keepMeAlive)
            {
                keepMeAlive = true;
                break;
            }
        }

        recursionLevel--;
    }

    /* action of the state */
    public abstract void DoState(ref StateInfo infos);

    public bool isActiveSubState<State>()
        where State : MachineState<StateInfo>
    {
        if (activeSubState != null && activeSubState.GetType() == typeof(State))
        {
            return true;
        }
        return false;
    }

    private MachineState<StateInfo> FindSubStateWithType<State>()
        where State : MachineState<StateInfo>
    {
        foreach (MachineState<StateInfo> state in subStates)
        {
            if (state.GetType() == typeof(State))
            {
                return state;
            }
        }
        return null;
    }

    protected void AddAndActivateSubState<State>()
        where State : MachineState<StateInfo>, new()
    {
        MachineState<StateInfo> state = FindSubStateWithType<State>();
        if (state == null)
        {
            state = new State();
            Log("Create " + state.name);
        }
        else
        {
            subStates.Remove(state);
        }

        // stack
        subStates.Add(state);
        activeSubState = state;
    }

    protected void ClearSubStates()
    {
        Log("Cleared substates");

        subStates.Clear();
        activeSubState = null;
    }

    protected void RemoveSubState<State>()
        where State : MachineState<StateInfo>, new()
    {
        MachineState<StateInfo> state = FindSubStateWithType<State>();
        if (state != null)
        {
            Log("Remove " + state.name);

            subStates.Remove(state);
        }

        if (activeSubState == state)
        {
            activeSubState = null;
        }
    }

    protected void RemoveSubState(MachineState<StateInfo> state)
    {
        if (subStates.Remove(state))
        {
            Log("Remove " + state.name);
        }
    }

    protected void Log(string message)
    {
        if (showDebug)
        {
            string msg = "";
            for (int i = 0; i < recursionLevel; i++)
            {
                msg += "-";
            }

            Debug.Log($"{msg} [{name}] {message}");
        }
    }
}
