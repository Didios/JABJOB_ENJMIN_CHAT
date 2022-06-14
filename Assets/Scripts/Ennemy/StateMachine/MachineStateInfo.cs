using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class MachineStateInfo
{
    /* Contains all informations about IA for the state machine */

    [Header("State Machine")]
    public float updatePeriod;

    [Header("Controller")]
    public AgentController agent;
    public float distanceMinToTarget;
    public Vector2 distanceMinMax;
    public bool isStunned;
    public bool isArrived;

    [Header("Player")]
    public Transform player;
    public bool isCatch;
    public bool canSeeTarget;
    public Vector3 posLastSee;
    public bool isArrivedToLastSee;

    [Header("Hear")]
    public bool isNoise;
    public Vector3 positionNoise;

    [Header("Light")]
    public bool isLightOff;
    public SwitchLight switchLight;

    [Header("Cooldown")]
    public CooldownBar cooldownBar;
    public float cooldownStunned;
    public Vector2 cooldownMinMaxWait;

    [Header("UI")]
    public TextMeshProUGUI textUI;

    [Header("Other")]
    public Vector3 outside;
    public bool gameOver;
}
