using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    private TextMeshProUGUI scoreboard;
    private int score = 0;
    public string text = "Broken object : ";

    private void Start()
    {
        scoreboard = GetComponent<TextMeshProUGUI>();
        scoreboard.text = text + score.ToString();
    }

    public void Increment()
    {
        score += 1;
        scoreboard.text = text + score.ToString();
    }
}
