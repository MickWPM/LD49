using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHighScores : MonoBehaviour
{
    
    public GameObject loadingHighScoresGO;
    public GameObject noHighScoresGO;
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

    bool disableOnlineScores = false;
    public void ShowHighScoresUI()
    {
        GameOptionsPersistent gameOptionsPersistent = GameObject.FindObjectOfType<GameOptionsPersistent>();
        if (gameOptionsPersistent != null) disableOnlineScores = gameOptionsPersistent.DisableOnlineScores;
        Debug.Log("Show high scores ui");
        highScoresUI.SetActive(true);
        clickedIslandButton = starterIslandButton;
        clickedIslandButton.DoClickedEffect();
        clickedButton = normalButton;
        clickedButton.DoClickedEffect();
        //Animation scaling messes up initial prefab spawn.
        StartCoroutine(ShowHighScoresAfterLoad());
    }

    //Animation scaling messes up initial prefab spawn.
    IEnumerator ShowHighScoresAfterLoad()
    {
        yield return new WaitForSeconds(0.2f);
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
        GetHighScoresForMapAndGame(currentMap, currentGameMode);
    }


    void GetHighScoresForMapAndGame(MapType mapType, GameOptionsPersistent.GameMode gameMode)
    {
        HighScoreSetup.IslandGameKey islandGameKey = new HighScoreSetup.IslandGameKey();
        islandGameKey.mapType = mapType;
        islandGameKey.gameMode = gameMode;

        string appid = HighScoreSetup.AppIds[islandGameKey];
        string appsecret = HighScoreSetup.AppSecrets[islandGameKey];
        string playerID = PlayerPrefs.GetString("Guid");

        if(disableOnlineScores)
        {
            FailedToLoadHighScores();
            return;
        }

        HighscoreService highscoreService = new HighscoreService(appid, appsecret, HighScoreSetup.API_URL);

        Debug.LogWarning("Move this to on initial load of scene? Of high scores menu? This is slow but will always give the latest scores");
        StartCoroutine(
            highscoreService.GetHighscores(playerID, OnHighScoreSuccess, OnHighScoreFailure)
            );

        return;
    }


    void OnHighScoreSuccess(HighscoreResults highscoreResults)
    {
        loadingHighScoresGO.SetActive(false);
        HighscoreResultItemDto playerScoreFromResults = highscoreResults.currentPlayerScore;
        List<HighscoreResultItemDto> topScores = highscoreResults.topScores;
        if(topScores.Count == 0)
        {
            noHighScoresGO.SetActive(true);
            SpawnPlayerHighScoreEntry();
            return;
        }
        long playerPlacementLong = highscoreResults.currentPlayerScore.placement;
        
        int numToSpawn = Mathf.Min(5, topScores.Count);
        bool playerScoreSpawned = false;
        for (int i = 0; i < numToSpawn; i++)
        {
            HighscoreResultItemDto score = topScores[i];
            string playername = score.placement + score.playerName;
            if(i == playerScoreFromResults.placement -1)
            {
                SpawnHighScoreEntry(playerScoreFromResults.placement + ". " + playerScoreFromResults.playerName,
                       highscoreResults.currentPlayerScore.score.ToString(), true);
                playerScoreSpawned = true;
            } else
            {
            SpawnHighScoreEntry(score.placement + ". " + score.playerName,
                score.score.ToString());
            }

        }
        if(playerScoreSpawned == false && playerScoreFromResults.score > 0)
        {
            SpawnHighScoreEntry(playerScoreFromResults.placement + ". " + playerScoreFromResults.playerName,
                highscoreResults.currentPlayerScore.score.ToString(), true);
        }


        Debug.Log("SUCCESS");
    }

    void OnHighScoreFailure(UnityEngine.Networking.UnityWebRequest.Result result)
    {
        FailedToLoadHighScores();
    }

    public void SpawnHighScoreEntry(string playerName, string score, bool player = false)
    {
        HighScoreVisualEntry h = Instantiate(player ? playerHighScoreEntry : highScoreEntry);
        h.SetHighScore(playerName, score);
        h.transform.SetParent(HighScoreBodyTransform);
        h.transform.localScale = Vector3.one;
    }

    public void SpawnHighScoreEntry(HighScore score, bool player = false)
    {
        HighScoreVisualEntry h = Instantiate(player ? playerHighScoreEntry : highScoreEntry);
        h.SetHighScore(score);
        h.transform.SetParent(HighScoreBodyTransform);
        h.transform.localScale = Vector3.one;
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
        noHighScoresGO.SetActive(false);
        loadingHighScoresGO.SetActive(true);
    }


    void SpawnPlayerHighScoreEntry()
    {
        HighScoreSetup.IslandGameKey islandGameKey = new HighScoreSetup.IslandGameKey();
        islandGameKey.gameMode = currentGameMode;
        islandGameKey.mapType = currentMap;

        string prefIndex = HighScoreSetup.GetGameModeIslandIDForHighScorePlayerPrefs(islandGameKey);
        int highScore = PlayerPrefs.GetInt(prefIndex, 0);

        HighScore tmpHighScore = new HighScore();
        if (highScore > 0)
        {
            tmpHighScore.playerName = PlayerPrefs.GetString(
                HighScoreSetup.GetGameModeIslandIDForHighScoreNAMEPlayerPrefs(islandGameKey),
                "Unknown local player"
                );
        }
        else
        {
            tmpHighScore.playerName = PlayerPrefs.GetString("PlayerName", "Local player");
        }
        tmpHighScore.playerScore = highScore;
        SpawnHighScoreEntry(tmpHighScore, true);
    }

    void FailedToLoadHighScores()
    {
        loadingHighScoresGO.SetActive(false);
        if (disableOnlineScores == false)
            failedToLoadHighScoresGO.SetActive(true);

        SpawnPlayerHighScoreEntry();
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

    public void ClearLocalHighScores()
    {
        ClearIslandGameKeys(MapType.STARTER);
        ClearIslandGameKeys(MapType.MAUG);
        ClearIslandGameKeys(MapType.SHATTERED);
    }

    void ClearIslandGameKeys(MapType island)
    {
        ClearKey(GameOptionsPersistent.GameMode.NORMAL, island);
        ClearKey(GameOptionsPersistent.GameMode.HARDCORE, island);
        ClearKey(GameOptionsPersistent.GameMode.ZEN, island);
        ClearKey(GameOptionsPersistent.GameMode.HARDCORE_ZEN, island);
    }

    void ClearKey(GameOptionsPersistent.GameMode gameMode, MapType mapType)
    {
        string gameID = HighScoreSetup.GetGameModeIslandIDForHighScorePlayerPrefs(mapType, gameMode);
        if(PlayerPrefs.HasKey(gameID))
            PlayerPrefs.DeleteKey(gameID);
        string gameIDName = HighScoreSetup.GetGameModeIslandIDForHighScoreNAMEPlayerPrefs(mapType, gameMode);
        if(PlayerPrefs.HasKey(gameIDName))
            PlayerPrefs.DeleteKey(gameIDName);
    }
}
