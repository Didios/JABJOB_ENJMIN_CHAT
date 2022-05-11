using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SightStimulus
{
    public Vector3 position;
    public float accuracy;
}

public class SenseSight : AISense<SightStimulus>
{
    public float sightAngle = 60;
    public float maxDistance = 10f;

    public float coneAccuracy = 0.5f;
    public float boundsMargin = 0.2f; // not test total width of the mesh, keep a margin

    private List<Vector3> checkedPoints = new List<Vector3>();

    protected override void ResetSense()
    {
        base.ResetSense();

        if (showDebug)
        {
            checkedPoints.Clear();
        }
    }

    /* get the global form of the object */
    private Bounds GetObjectBounds(Transform t)
    {
        Renderer[] rends = t.GetComponentsInChildren<Renderer>();
        Bounds b = new Bounds();
        foreach(Renderer r in rends)
        {
            if (b.size.sqrMagnitude == 0)
            {
                b = new Bounds(r.bounds.center, r.bounds.size);
            }
            else
            {
                b.Encapsulate(r.bounds);
            }
        }
        return b;
    }

    private bool CanSeePoint(Vector3 point, Transform parent)
    {
        Vector3 dirToPoint = point - sensitivePart.position;

        if (Vector3.Angle(sensitivePart.forward, dirToPoint) < sightAngle / 2.0f)
        {
            float distance = dirToPoint.magnitude;
            if (distance < maxDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(sensitivePart.position, dirToPoint, out hit, distance))
                {
                    if (hit.transform.IsChildOf(parent) || hit.transform == parent)
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

    protected override bool DoSense(Transform obj, ref SightStimulus sti)
    {
        // check distance
        if ((obj.position - sensitivePart.position).sqrMagnitude > maxDistance * maxDistance)
        {
            return false;
        }

        // get size object
        Bounds b = GetObjectBounds(obj);
        float width = Mathf.Max(b.extents.x, b.extents.z) * (1 - boundsMargin);
        float height = b.extents.y * (1 - boundsMargin);

        // get position of center and coins point of the object
        Vector3[] pointsToCheck =
        {
            b.center + obj.right * width,
            b.center - obj.right * width,
            b.center + obj.right * width + obj.up * height,
            b.center - obj.right * width + obj.up * height,
            b.center + obj.right * width - obj.up * height,
            b.center - obj.right * width - obj.up * height,
            b.center
        };

        sti.position = obj.position;

        checkedPoints.Clear();
        foreach(Vector3 point in pointsToCheck)
        {
            if (showDebug)
                checkedPoints.Add(point);

            if (CanSeePoint(point, obj))
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
            Vector3 pointMax = new Vector3(0, 0, maxDistance);

            Gizmos.color = Color.red;
            Vector3 posWorld = sensitivePart.TransformPoint(pointMax);
            Gizmos.DrawLine(sensitivePart.position, posWorld);

            pointMax = Quaternion.AngleAxis(sightAngle / 2, Vector3.right) * pointMax;

            float nbRays = 100 * coneAccuracy;
            Quaternion rCone = Quaternion.AngleAxis(360 / nbRays, Vector3.forward);

            for (float i = 0; i < nbRays; i++)
            {
                pointMax = rCone * pointMax;
                posWorld = sensitivePart.TransformPoint(pointMax);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(sensitivePart.position, posWorld);
            }

            // draw checked points
            Gizmos.color = Color.yellow;
            foreach (Vector3 p in checkedPoints)
            {
                Gizmos.DrawLine(sensitivePart.position, p);
            }
        }
    }
}
