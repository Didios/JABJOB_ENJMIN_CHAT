using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    private TextMeshProUGUI scoreboard;
    private int score = 0;

    private void Start()
    {
        scoreboard = GetComponent<TextMeshProUGUI>();
        scoreboard.text = "Object break : " + score.ToString();
    }

    public void Increment()
    {
        score += 1;
        scoreboard.text = "Object break : " + score.ToString();
    }
}
