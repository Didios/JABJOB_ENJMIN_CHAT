using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SatisfactionBar : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textUI;
    public Transform gauge;
    public Image gaugeImg;
    public Axis gaugeAxis;
    public Color colorBase;
    public Color colorEnd;

    [Header("Variables")]
    public int maxCount = 1;
    private int count = 0;
    public bool showDebug = false;

    public bool finish
    {
        get { return count >= maxCount; }
        private set { }
    }

    private void Start()
    {
        textUI.text = "100 %";
        gauge.localScale = new Vector3(1, 1, 1);
        gaugeImg.color = colorBase;
    }

    public void UpdateBar(int weight)
    {
        count += weight;

        var _percent = 100 - Mathf.RoundToInt(count * 100 / maxCount);
        if (_percent < 1 && !finish) { _percent = 1; }

        textUI.text = $"{_percent} %";

        var _scale = new Vector3(1, 1, 1);
        if (gaugeAxis == Axis.X || gaugeAxis == Axis.XY || gaugeAxis == Axis.XZ || gaugeAxis == Axis.XYZ)
        { _scale.x = _percent / 100.0f; }
        if (gaugeAxis == Axis.Y || gaugeAxis == Axis.XY || gaugeAxis == Axis.YZ || gaugeAxis == Axis.XYZ)
        { _scale.y = _percent / 100.0f; }
        if (gaugeAxis == Axis.Z || gaugeAxis == Axis.XZ || gaugeAxis == Axis.XZ || gaugeAxis == Axis.XYZ)
        { _scale.z = _percent / 100.0f; }

        gauge.localScale = _scale;
        gaugeImg.color = GetColorPercent(_percent);

        if (showDebug) Debug.Log("[SatisfactionBar]:\n Bar Update");

    }

    public void ResetBar(int newMax)
    {
        /* UI */
        textUI.text = "100 %";
        gauge.localScale = new Vector3(1, 1, 1);
        gaugeImg.color = colorBase;

        /* variables */
        maxCount = newMax;
        count = 0;

        if (showDebug) Debug.Log("[SatisfactionBar]:\n Bar Reset");
    }

    public Color GetColorPercent(int percentage)
    {
        /* get color with a curve between the two color, curve type y = ax + b */

        var _color = colorBase;
        _color.r = (Mathf.Abs(colorEnd.r - colorBase.r) / 100.0f) * percentage + colorBase.r;
        _color.g = (Mathf.Abs(colorEnd.g - colorBase.g) / 100.0f) * percentage + colorBase.g;
        _color.b = (Mathf.Abs(colorEnd.b - colorBase.b) / 100.0f) * percentage + colorBase.b;
        //_color.a = (colorEnd.a - colorBase.a) / 100;

        return _color;
    }
}

public enum Axis
{
    X,
    Y,
    Z,
    XY,
    XZ,
    YZ,
    XYZ
}
