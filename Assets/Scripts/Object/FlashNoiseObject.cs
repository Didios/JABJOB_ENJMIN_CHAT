using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashNoiseObject : MonoBehaviour
{
    public float noise = 1.0f;
    public float noisyTime = 5.0f;

    public bool isActive = false;

    private float timer = 0.0f;
    private NoiseStatus status;

    private void Start()
    {
        status = GetComponent<NoiseStatus>();
    }

    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer > noisyTime)
            {
                status.NoiseLevel = 0;
                isActive = false;
            }
        }
    }

    public void ResetNoiseTimer(float noiseLevel, float _timer)
    {
        timer = 0;

        noise = noiseLevel;
        status.NoiseLevel = noise;
        noisyTime = _timer;

        isActive = true;
    }

    public void ResetNoiseTimer()
    {
        ResetNoiseTimer(noise, noisyTime);
    }
}
