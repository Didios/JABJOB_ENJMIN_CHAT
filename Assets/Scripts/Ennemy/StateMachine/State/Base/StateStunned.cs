using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StateStunned : MachineState<MachineStateInfo>
{
    private float timer = 0.0f;
    private bool cooldownSet = false;
    private int _iterateurR = 0;
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
                HurtSounds(infos._hurtSounds, infos.agent.transform);
                cooldownSet = true;
            }
            keepMeAlive = true; // don't lose the timer
        }

        timer += infos.updatePeriod; // in state machine, update is make with infos.updatePeriod
    }
    public void HurtSounds(List<AudioClip> _audioClips, Transform fixGetComponentBug)
    {
        if (_audioClips != null)
        {
            if (_iterateurR <= _audioClips.Count)
            {
                _iterateurR = Random.Range(0, _audioClips.Count);
                fixGetComponentBug.GetComponent<AudioSource>().PlayOneShot(_audioClips[_iterateurR]);
                _iterateurR++;

                if (_iterateurR >= _audioClips.Count)
                {
                    _iterateurR = 0;
                }
            }
        }
        else
        {
            Debug.Log("Il faut mettre des sons au characterhurt!");
        }
    }
}
