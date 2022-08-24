using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LightStimulus
{
    public Vector3 position;
    public SwitchLight light;
    public float lightLevel;
}

public class SenseLight : AISense<LightStimulus>
{
    public float maxDistance = 10;
    public float minLightLevel = 0.5f;

    protected override bool DoSense(Transform obj, ref LightStimulus sti)
    {
        LightStatus light = obj.GetComponent<LightStatus>();

        sti.position = light.position;

        if (light != null)
        {
            sti.lightLevel = light.LightLevel;
            if (light.LightLevel < minLightLevel && (obj.position - sensitivePart.position).sqrMagnitude < maxDistance * maxDistance)
            {
                Vector3 dirToPoint = obj.position - sensitivePart.position;
                float distance = dirToPoint.magnitude;

                RaycastHit hit;
                if (Physics.Raycast(sensitivePart.position, dirToPoint, out hit, distance))
                {
                    if (hit.transform.IsChildOf(obj) || hit.transform == obj)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (showDebug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(sensitivePart.position, maxDistance);
        }
    }
}
