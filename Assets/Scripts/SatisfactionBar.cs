using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SatisfactionBar : MonoBehaviour
{
    private float sizeMax;

    public int nbrObjToBreak;
    private int nbrObjBreak = 0;

    public TextMeshProUGUI percent;
    private int percentGood = 0;

    public bool finish
    {
        get { return percentGood == 100 || nbrObjBreak == nbrObjToBreak; }
        private set { }
    }

    private void Start()
    {
        sizeMax = transform.localScale.x;
        percent.text = "100 %";
    }

    public void AddBreakObj()
    {
        nbrObjBreak += 1;

        float probaNotGood = 1 - (nbrObjBreak / (nbrObjToBreak + 0.0f));
        percentGood = Mathf.RoundToInt(probaNotGood * 100);
        percent.text = $"{percentGood} %";

        transform.localScale = new Vector3(sizeMax * probaNotGood, 1, 1);

    }

    public void ResetBar(int nbr)
    {
        percentGood = 0;
        nbrObjToBreak = nbr;
        nbrObjBreak = 0;
        transform.localScale = new Vector3(sizeMax, 1, 1);
    }
}
