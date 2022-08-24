using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HearingStimulus
{
    public Vector3 position;
    public float alertLevel;
}

public class SenseHearing : AISense<HearingStimulus>
{
    public float maxDistance = 10;
    public float minNoiseHear = 0.5f;

    protected override bool DoSense(Transform obj, ref HearingStimulus sti)
    {
        NoiseStatus noise = obj.GetComponent<NoiseStatus>();
        sti.position = obj.position;

        if (noise != null)
        {
            sti.alertLevel = noise.NoiseLevel;
            if (noise.NoiseLevel > minNoiseHear &&  // noise level
                (obj.position - sensitivePart.position).sqrMagnitude < maxDistance * maxDistance) // distance
            {
                return true;
            }
        }
        return false;
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (showDebug)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sensitivePart.position, maxDistance);
        }
    }
}
