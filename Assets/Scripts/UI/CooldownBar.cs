using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{
    public Transform cooldownBar;
    public Axis cooldownAxis;

    private float cooldownMax = -1;
    private float cooldown = -1;

    private bool inCooldown = false;

    void Start()
    {
        cooldownBar.gameObject.SetActive(false);
    }

    void Update()
    {
        if (cooldown > 0)
        {
            if (!cooldownBar.gameObject.activeSelf) { cooldownBar.gameObject.SetActive(true); }

            cooldown -= Time.deltaTime;

            var _scale = new Vector3(1, 1, 1);
            if (cooldownAxis == Axis.X || cooldownAxis == Axis.XY || cooldownAxis == Axis.XZ || cooldownAxis == Axis.XYZ)
            { _scale.x = cooldown / cooldownMax; }
            if (cooldownAxis == Axis.Y || cooldownAxis == Axis.XY || cooldownAxis == Axis.YZ || cooldownAxis == Axis.XYZ)
            { _scale.y = cooldown / cooldownMax; }
            if (cooldownAxis == Axis.Z || cooldownAxis == Axis.XZ || cooldownAxis == Axis.XZ || cooldownAxis == Axis.XYZ)
            { _scale.z = cooldown / cooldownMax; }

            cooldownBar.localScale = _scale;
        }
        else if (cooldown != -1)
        {
            cooldown = -1;
            cooldownBar.gameObject.SetActive(false);
            cooldownBar.localScale = new Vector3(1, 1, 1);
            inCooldown = false;
        }
    }

    public void SetCooldown(float _cooldown)
    {
        if (!inCooldown)
        {
            cooldownMax = _cooldown;
            cooldown = _cooldown;
            cooldownBar.gameObject.SetActive(true);

            inCooldown = true;
        }
    }
}
