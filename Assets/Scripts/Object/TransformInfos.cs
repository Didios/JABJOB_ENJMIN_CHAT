using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformInfos : MonoBehaviour
{
    [Header("Grab")]
    public bool canBeGrab = false;
    public bool isGrab = false;
    public float noiseOnGrab;
    public float noiseOnGrabTimer;

    [Header("Noise")]
    public AnimationCurve noiseCurve;
    public float noiseCurveTimer = 0;
    private NoiseStatus noiseCurrent;
    private float noiseToSet = 0;

    private Rigidbody _rigidbody_;
    private Collider[] _collider_;

    // Timers
    public float timer = 0;
    public float noiseTimer = 0;

    private void Start()
    {
        noiseCurrent = GetComponent<NoiseStatus>();
        if (noiseCurrent == null)
            noiseCurrent = gameObject.AddComponent<NoiseStatus>();

        _rigidbody_ = GetComponent<Rigidbody>();
        _collider_ = GetComponents<Collider>();
    }

    private void Update()
    {
        noiseTimer += Time.deltaTime;
        if (noiseTimer > noiseCurveTimer)
            noiseTimer = 0;

        if (timer > 0)
            timer -= Time.deltaTime;
        else if (noiseCurve != null)
            noiseToSet = noiseCurve.Evaluate(noiseTimer / noiseCurveTimer);
        else
            noiseToSet = 0;

        noiseCurrent.NoiseLevel = noiseToSet;
    }

    public void SetNoise(float _noise, float _timer)
    {
        noiseToSet = _noise;
        timer = _timer;
    }

    public void GrabIt()
    {
        isGrab = true;
        SetNoise(noiseOnGrab, noiseOnGrabTimer);

        Lock();
    }

    public void Lock()
    {
        _rigidbody_.constraints = RigidbodyConstraints.FreezeAll;
        foreach (Collider c in _collider_) c.enabled = false;
    }
    public void Unlock()
    {
        _rigidbody_.constraints = RigidbodyConstraints.None;
        foreach (Collider c in _collider_) c.enabled = true;
    }
}
