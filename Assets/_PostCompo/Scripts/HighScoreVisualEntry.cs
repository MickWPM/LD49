using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreVisualEntry : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI scoreText;

    public void SetHighScore(ShowHighScores.HighScore highScore)
    {
        SetHighScore(highScore.playerName, highScore.playerScore.ToString());
    }

    public void SetHighScore(string playerName, string score)
    {
        nameText.text = playerName;
        scoreText.text = score;
    }
}
