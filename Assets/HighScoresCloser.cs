using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoresCloser : MonoBehaviour
{
    public ShowHighScores highScores;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            highScores.CloseHighScoresUI();
    }
}
