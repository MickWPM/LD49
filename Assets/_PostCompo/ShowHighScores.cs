using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHighScores : MonoBehaviour
{
    public HighScoreSetup.IslandGameKey IslandGameKey;
    public string appId, appSecret;
    private void Start()
    {
        appId = HighScoreSetup.AppIds[IslandGameKey];
        appSecret = HighScoreSetup.AppSecrets[IslandGameKey];
    }

    public GameObject loadingHighScoresGO;
    public GameObject failedToLoadHighScoresGO;
    public GameObject highScoresUI;

    public MyButton starterIslandButton, maugButton, shatteredButton;
    MyButton currentIslandButton;
    MyButton clickedIslandButton;
    public void SetMapToStarter()
    {
        clickedIslandButton = starterIslandButton;
        currentMap = MapType.STARTER;
        UpdateHighScores();
    }

    public void SetMapToMaug()
    {
        clickedIslandButton = maugButton;
        currentMap = MapType.MAUG;
        UpdateHighScores();
    }

    public void SetMapToShattered()
    {
        clickedIslandButton = shatteredButton;
        currentMap = MapType.SHATTERED;
        UpdateHighScores();
    }

    MapType currentMap = MapType.STARTER;
    public void CloseHighScoresUI()
    {
        highScoresUI.SetActive(false);
    }

    public void ShowHighScoresUI()
    {
        highScoresUI.SetActive(true);
        clickedIslandButton = starterIslandButton;
        PopulateNormalHighScores();
    }

    public MyButton normalButton, hardcoreButton, zenButton, hardcoreZenButton;
    MyButton currentButton;
    MyButton clickedButton;
    GameOptionsPersistent.GameMode currentGameMode;
    public void PopulateNormalHighScores()
    {
        clickedButton = normalButton;
        currentGameMode = GameOptionsPersistent.GameMode.NORMAL;
        UpdateHighScores();
    }

    public void PopulateHardcoreHighScores()
    {
        clickedButton = hardcoreButton;
        currentGameMode = GameOptionsPersistent.GameMode.HARDCORE;
        UpdateHighScores();
    }

    public void PopulateZenHighScores()
    {
        clickedButton = zenButton;
        currentGameMode = GameOptionsPersistent.GameMode.ZEN;
        UpdateHighScores();
    }
    public void PopulateHardcoreZenHighScores()
    {
        clickedButton = hardcoreZenButton;
        currentGameMode = GameOptionsPersistent.GameMode.HARDCORE_ZEN;

        UpdateHighScores();
    }

    public string[] testscores = { "22", "20", "20", "19", "12", };
    public string[] testnames = { "A", "B", "C", "D", "E"};
    public string testplayerscore = "19";
    public HighScoreVisualEntry highScoreEntry, playerHighScoreEntry;
    public void UpdateHighScores()
    {
        ClearHighScores();

        if (currentButton != null && currentButton != clickedButton)
        {
            currentButton.UnDoClickedEffect();
        }
        if (currentButton != clickedButton)
        {
            currentButton = clickedButton;
            currentButton.DoClickedEffect();
        }


        if (currentIslandButton != null && currentIslandButton != clickedIslandButton)
        {
            currentIslandButton.UnDoClickedEffect();
        }
        if (currentIslandButton != clickedIslandButton)
        {
            currentIslandButton = clickedIslandButton;
            currentIslandButton.DoClickedEffect();
        }



        //Get the high scores for map and game
        if(currentMap == MapType.STARTER)
        {
            GetHighScoresForMapAndGame(currentMap, currentGameMode);
        } else if (currentMap == MapType.SHATTERED)
        {
            FailedToLoadHighScores();
        }


    }


    void GetHighScoresForMapAndGame(MapType mapType, GameOptionsPersistent.GameMode gameMode)
    {
        //This is in a callback

        loadingHighScoresGO.SetActive(false);

        HighScore playerHighScore;
        bool validPlayerScoreToBeSpawned = (TryGetHighScoreFromStrings("Player name", testplayerscore, out playerHighScore));

        for (int i = 0; i < 5; i++)
        {
            HighScore h;
            if (TryGetHighScoreFromStrings(testnames[i], testscores[i], out h))
            {
                if (validPlayerScoreToBeSpawned && playerHighScore.playerScore > h.playerScore)
                {
                    validPlayerScoreToBeSpawned = false;
                    SpawnHighScoreEntry(playerHighScore, true);
                }
                SpawnHighScoreEntry(h);
            }
        }
    }

    public void SpawnHighScoreEntry(HighScore score, bool player = false)
    {
        HighScoreVisualEntry h = Instantiate(player ? playerHighScoreEntry : highScoreEntry);
        h.SetHighScore(score);
        h.transform.SetParent(HighScoreBodyTransform);
    }


    public Transform HighScoreBodyTransform;
    void ClearHighScores()
    {
        while (HighScoreBodyTransform.childCount > 0)
        {
            Transform t = HighScoreBodyTransform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
        failedToLoadHighScoresGO.SetActive(false);
        loadingHighScoresGO.SetActive(true);
    }

    bool TryGetHighScoreFromStrings(string playername, string playerscore, out HighScore highScore)
    {
        highScore = new HighScore();
        highScore.playerName = playername;
        bool valid = (int.TryParse(playerscore, out highScore.playerScore));

        return valid;
    }

    void FailedToLoadHighScores()
    {
        loadingHighScoresGO.SetActive(false);
        failedToLoadHighScoresGO.SetActive(true);

        Debug.LogWarning("Add in player preferences local high scores and show this here");
        HighScore tmpHighScore = new HighScore();
        tmpHighScore.playerName = "Local player";
        tmpHighScore.playerScore = 19;
        SpawnHighScoreEntry(tmpHighScore, true);
    }

    public struct HighScore
    {
        public string playerName;
        public int playerScore;
    }


    public enum MapType
    {
        STARTER,
        MAUG,
        SHATTERED
    }


    private void Awake()
    {
        highScoresUI.SetActive(false);
        loadingHighScoresGO.SetActive(false);
        failedToLoadHighScoresGO.SetActive(false);
    }
}
