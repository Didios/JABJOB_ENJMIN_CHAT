using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShooterBar : MonoBehaviour
{
    [Header("UI")]
    // text
    public TextMeshProUGUI textUI;
    public string textDisplay = "balls";

    // ball
    public Transform ballPrefab;
    private List<Transform> listBallsBar = new List<Transform>();
    public Vector3 localStart = new Vector3();
    public int nbrLine = 10;
    public float space = 10;

    // cooldown
    public CooldownBar cooldownBar;
    /*
    public Transform cooldownBar;
    public Axis cooldownAxis;
    public float cooldown = 0;
    private float maxCooldown = 2;
    */
    [Header("Variables")]
    public int maxCount = 1;
    public int actualCount = 0;
    public bool showDebug = false;

    void Start()
    {
        //cooldownBar.gameObject.SetActive(false);
        textUI.text = $"{actualCount} {textDisplay}";
        ResetBalls();
    }

    /*
    private void Update()
    {
        if (cooldown > 0)
        {
            if (!cooldownBar.gameObject.activeSelf) { cooldownBar.gameObject.SetActive(true); }

            cooldown -= Time.deltaTime;

            var _scale = new Vector3(1, 1, 1);
            if (cooldownAxis == Axis.X || cooldownAxis == Axis.XY || cooldownAxis == Axis.XZ || cooldownAxis == Axis.XYZ)
            { _scale.x = cooldown / maxCooldown; }
            if (cooldownAxis == Axis.Y || cooldownAxis == Axis.XY || cooldownAxis == Axis.YZ || cooldownAxis == Axis.XYZ)
            { _scale.y = cooldown / maxCooldown; }
            if (cooldownAxis == Axis.Z || cooldownAxis == Axis.XZ || cooldownAxis == Axis.XZ || cooldownAxis == Axis.XYZ)
            { _scale.z = cooldown / maxCooldown; }

            cooldownBar.localScale = _scale;
        }
        else if (cooldown != -1)
        {
            cooldown = -1;
            cooldownBar.gameObject.SetActive(false);
            cooldownBar.localScale = new Vector3(1, 1, 1);
        }
    }
    */

    public void UpdateBar(int count, float cd = 0)
    {
        /*
        maxCooldown = cd;
        cooldown = cd;
        */
        cooldownBar.SetCooldown(cd);

        actualCount += count;
        actualCount = Mathf.Min(actualCount, maxCount);
        actualCount = Mathf.Max(0, actualCount);

        for (int i = 0; i < listBallsBar.Count; i++)
        {
            listBallsBar[i].gameObject.SetActive(i < actualCount);
        }

        textUI.text = $"{actualCount} {textDisplay}";

        if (showDebug) Debug.Log("[ShooterBar]\n Update");
    }

    public void ResetBar(int max, int actual)
    {
        maxCount = max;
        actualCount = actual;

        textUI.text = $"{actualCount} {textDisplay}";
        ResetBalls();

        if (showDebug) Debug.Log("[ShooterBar]\n Reset Bar");
    }

    private void ResetBalls()
    {
        var _length = listBallsBar.Count;

        for (int i = 0; i < _length; i++)
        {
            listBallsBar[i].gameObject.SetActive(i < actualCount);
        }
        
        if (_length < maxCount)
        {
            for (int i = _length; i < maxCount; i++)
            {
                var _x = localStart.x + (i % nbrLine) * space;
                var _y = localStart.y - (i / nbrLine) * space;

                var _ball = Instantiate<Transform>(ballPrefab, transform);
                _ball.localPosition = new Vector3(_x, _y, localStart.z);
                _ball.gameObject.SetActive(i < actualCount);
                listBallsBar.Add(_ball);
            }
        }

        if (showDebug) Debug.Log("[ShooterBar]\n Reset Balls Sprite");
    }
}
