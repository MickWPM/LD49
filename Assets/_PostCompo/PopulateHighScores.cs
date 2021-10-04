using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateHighScores : MonoBehaviour
{
    public void PopulateNormalHighScores()
    {
        PopulateHighScoresOfType(GameOptionsPersistent.GameMode.NORMAL);
    }

    public void PopulateHardcoreHighScores()
    {
        PopulateHighScoresOfType(GameOptionsPersistent.GameMode.HARDCORE);
    }

    public void PopulateZenHighScores()
    {
        PopulateHighScoresOfType(GameOptionsPersistent.GameMode.ZEN);
    }
    public void PopulateHardcoreZenHighScores()
    {
        PopulateHighScoresOfType(GameOptionsPersistent.GameMode.HARDCORE_ZEN);
    }


    public void PopulateHighScoresOfType(GameOptionsPersistent.GameMode gameMode)
    {
        ClearHighScores();
    }

    public Transform HighScoreBodyTransform;
    void ClearHighScores()
    {
        while(HighScoreBodyTransform.childCount > 0)
        {
            Transform t = HighScoreBodyTransform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
    }
}
