using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateInfoController : AgentController
{
    /* Update Manually state machine
     * Update state infos with sense
     */
    [Header("General")]
    public MachineStateInfo infos = new MachineStateInfo();
    private UpdateMachine<StateBase, MachineStateInfo> machine = new UpdateMachine<StateBase, MachineStateInfo>();

    [Header("Personnal")]
    public List<Transform> lights;
    public bool isActive = false;

    private void Start()
    {
        machine.updatePeriod = infos.updatePeriod; //# TO DELETE

        // get sense
        var sight = GetComponent<SenseSight>();
        var hear = GetComponent<SenseHearing>();
        var light = GetComponent<SenseLight>();

        // put to a method
        sight.AddSenseHandler(new AISense<SightStimulus>.SenseEventHandler(HandleSight));
        hear.AddSenseHandler(new AISense<HearingStimulus>.SenseEventHandler(HandleHear));
        light.AddSenseHandler(new AISense<LightStimulus>.SenseEventHandler(HandleLight));

        // add object to track
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        sight.AddObjectToTrack(player);
        hear.AddObjectToTrack(player);
        foreach(Transform t in lights)
        {
            light.AddObjectToTrack(t);
        }
    }

    private void Update()
    {
        if (isActive)
        {
            machine.updatePeriod = infos.updatePeriod;
            machine.showDebug = showDebug;
            machine.Update(infos);
        }
    }

    private void HandleSight(SightStimulus sti, AISense<SightStimulus>.Status evt)
    {
        /* infos to change when event (here sight) call, means that the status of sti has change */
        if (evt == AISense<SightStimulus>.Status.Enter)
        {
            infos.canSeeTarget = true;
            infos.isArrivedToLastSee = false;
            infos.posLastSee = sti.position;
        }
        else if (evt == AISense<SightStimulus>.Status.Stay)
        {
            infos.canSeeTarget = true;
            infos.isArrivedToLastSee = false;
            infos.posLastSee = sti.position;
        }
        else if (evt == AISense<SightStimulus>.Status.Leave)
        {
            infos.canSeeTarget = false;
        }
    }

    private void HandleHear(HearingStimulus sti, AISense<HearingStimulus>.Status evt)
    {
        if (evt == AISense<HearingStimulus>.Status.Enter)
        {
            infos.positionNoise = sti.position;
            infos.isNoise = true;
        }
        else if (evt == AISense<HearingStimulus>.Status.Stay)
        {
            infos.isNoise = true;
        }
        else if (evt == AISense<HearingStimulus>.Status.Leave)
        {

        }
    }

    private void HandleLight(LightStimulus sti, AISense<LightStimulus>.Status evt)
    {
        if (evt == AISense<LightStimulus>.Status.Enter)
        {
            infos.switchLight = sti.light;
            infos.isLightOff = true;
        }
        else if (evt == AISense<LightStimulus>.Status.Stay)
        {
            infos.isLightOff = true;
        }
        else if (evt == AISense<LightStimulus>.Status.Leave)
        {
            infos.isLightOff = false;
        }
    }
}
