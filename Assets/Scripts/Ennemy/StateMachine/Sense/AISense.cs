using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AISense<Stimulus> : MonoBehaviour
{
    /* parent class for all Sense of an AI */

    public enum Status
    {
        Enter,
        Stay,
        Leave
    };

    public Transform sensitivePart;

    public float updateInterval = 0.3f;
    private float updateTimer = 0.0f;

    protected List<Transform> trackedObjects = new List<Transform>(); // Object that can change sense status
    protected List<Transform> sensedObjects = new List<Transform>(); // Object detected by sense

    public delegate void SenseEventHandler(Stimulus sti, Status sta);
    private event SenseEventHandler CallSenseEvent;

    public bool showDebug = true;

    /* call of the sense
     * do event if object sensed
     */
    private void Update()
    {
        Stimulus stimulus;
        Status sta = Status.Stay;

        updateTimer += Time.deltaTime;
        if (updateTimer > updateInterval)
        {
            ResetSense();

            foreach(Transform t in trackedObjects)
            {
                stimulus = default(Stimulus);

                if (DoSense(t, ref stimulus))
                {
                    sta = Status.Stay;

                    if (!sensedObjects.Contains(t))
                    {
                        sensedObjects.Add(t);
                        sta = Status.Enter;
                    }

                    CallSenseEvent(stimulus, sta);
                }
                else
                {
                    if (sensedObjects.Contains(t))
                    {
                        sta = Status.Leave;
                        CallSenseEvent(stimulus, sta);
                        sensedObjects.Remove(t);
                    }
                }
            }

            updateTimer = 0;
        }
    }

    /* Determine if obj trigger the sense 
     * if true, give the sense parameters in sti
     */
    protected abstract bool DoSense(Transform obj, ref Stimulus sti);

    /* Call before check object
     * can be redefine
     */
    protected virtual void ResetSense()
    {

    }

    /* call from a script to follow this event (and check if event trigger) */
    public void AddSenseHandler(SenseEventHandler handler)
    {
        CallSenseEvent += handler;
    }

    /* Add an object to track */
    public void AddObjectToTrack(Transform t)
    {
        trackedObjects.Add(t);
    }

    public void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.red;
            foreach(Transform t in sensedObjects)
            {
                Gizmos.DrawLine(sensitivePart.position, t.position);
            }
        }
    }
}
