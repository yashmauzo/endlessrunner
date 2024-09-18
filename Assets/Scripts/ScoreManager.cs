using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour
{
    public Text Score;
    public float elapsedTime = 0f;

    public void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        elapsedTime += Time.deltaTime;
        Score.text = "Score: " + Math.Round(elapsedTime, 0);
    }
}
