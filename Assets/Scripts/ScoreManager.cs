using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreManager : MonoBehaviour
{
    // UI Text element to display the score
    public Text Score;

    // Variable to track the elapsed time, which is used as the score
    public float elapsedTime = 0f;

    // Update is called once per frame
    public void Update()
    {
        // Ensure that the score only updates if the game has started
        if (!PlayerManager.isGameStarted)
            return;

        // Increment the elapsed time by the time that has passed since the last frame
        elapsedTime += Time.deltaTime;

        // Update the score display by rounding the elapsed time to the nearest whole number
        Score.text = "Score: " + Math.Round(elapsedTime, 0);
    }
}
