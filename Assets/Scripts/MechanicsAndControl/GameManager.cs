using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Task> tasks;
    int taskID = -1;
    Island island;

    GameOptionsPersistent gameOptions;

    private float timeLeftToComplete;
    public float TimeLeftToComplete { get => timeLeftToComplete; private set => timeLeftToComplete = value; }
    bool firstTaskIssued = false;
    bool playing = false;
    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
        island.TaskCompleteEvent += Island_TaskCompleteEvent;
        island.BuildingUnderwaterEvent += Island_BuildingUnderwaterEvent;
        Time.timeScale = 1;
    }

    private void Island_BuildingUnderwaterEvent(Building obj)
    {
        if(gameOptions == null)
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();

        switch (gameOptions.GameModeSelected)
        {
            case GameOptionsPersistent.GameMode.NORMAL:
            case GameOptionsPersistent.GameMode.ZEN:
                break;
            case GameOptionsPersistent.GameMode.HARDCORE_ZEN:
            case GameOptionsPersistent.GameMode.HARDCORE:
                EndGame(GameOverCause.HARDCORE_WASHED_AWAY);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
        if (gameOptions != null) StartGame();
    }

    public event System.Action<GameOptionsPersistent.GameMode> GameStartedEvent;
    void StartGame()
    {
        Invoke("IssueNextTask", 2f);
        playing = true;
        Debug.Log("starting game: " + CurrentGameMode);
        GameStartedEvent?.Invoke(CurrentGameMode);
    }

    public GameOptionsPersistent GameOptionsPersistent
    {
        get
        {
            return gameOptions;
        }
    }

    public void QuitToMenu()
    {
        Application.LoadLevel(1);
    }

    public void RetireWithDignity()
    {
        EndGame(GameOverCause.RETIRED);
    }

    private void Update()
    {
        if (gameOptions == null)
        {
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
            if (gameOptions == null)
            {
                return;
            } else
            {
                StartGame();
            }
        }
        if (playing == false) return;

        if (firstTaskIssued == false) return;
        if (gameOptions.GameModeSelected == GameOptionsPersistent.GameMode.ZEN || gameOptions.GameModeSelected == GameOptionsPersistent.GameMode.HARDCORE_ZEN) return;


        timeLeftToComplete -= Time.deltaTime;
        if(timeLeftToComplete < 0)
        {
            EndGame(GameOverCause.TIME_OUT);
        }
    }

    public float GetTaskTimeRemaining()
    {
        if (currentTask == null) return -1;
        if (gameOptions.GameModeSelected == GameOptionsPersistent.GameMode.HARDCORE_ZEN || gameOptions.GameModeSelected == GameOptionsPersistent.GameMode.ZEN) return -1;
        return Mathf.Clamp01(timeLeftToComplete / currentTask.TimeToComplete);
    }

    public GameOptionsPersistent.GameMode CurrentGameMode
    {
        get
        {
            return gameOptions.GameModeSelected;
        }
    }

    public event System.Action<GameOverCause> GameOverEvent;
    public TMPro.TextMeshProUGUI scoreSubmittingStatusText;
    public void EndGame(GameOverCause cause)
    {
        scoreSubmittingStatusText.text = "";
        Debug.Log("END GAME : " + cause);
        playing = false;

        //--- ADDED AFTER COMPO FOR HIGH SCORES ----
        HighScoreSetup.IslandGameKey islandGameKey = new HighScoreSetup.IslandGameKey();
        islandGameKey.gameMode = gameOptions.GameModeSelected;
        islandGameKey.mapType = gameOptions.IslandSelected;
        string gameID = HighScoreSetup.GetGameModeIslandIDForHighScorePlayerPrefs(islandGameKey);
        int prevBest = PlayerPrefs.GetInt(gameID, -1);
        if(TasksCompleted > prevBest)
        {
            string playerNamePref = HighScoreSetup.GetGameModeIslandIDForHighScoreNAMEPlayerPrefs(islandGameKey);
            PlayerPrefs.SetInt(gameID, TasksCompleted);
            PlayerPrefs.SetString(playerNamePref, PlayerPrefs.GetString("PlayerName"));
            if (GameOptionsPersistent.DisableOnlineScores == false)
            {
                scoreSubmittingStatusText.text = "Sending high score to server, please wait.";
                //SEND HIGH SCORES TO SERVER
                string appid = HighScoreSetup.AppIds[islandGameKey];
                string appsecret = HighScoreSetup.AppSecrets[islandGameKey];
                HighscoreService highscoreService = new HighscoreService(appid, appsecret, HighScoreSetup.API_URL);

                Debug.Log("Trying to submit");
                StartCoroutine(
                    highscoreService.SubmitScore(PlayerPrefs.GetString("Guid"), PlayerPrefs.GetString("PlayerName"), TasksCompleted,
                    OnHighScoreSuccess, OnHighScoreFailure)
                    );
            }
        }
        //---- END ADDED AFTER COMPO -------
        GameOverEvent?.Invoke(cause);
    }

    //--- ADDED AFTER COMPO FOR HIGH SCORES ----
    void OnHighScoreSuccess()
    {
        scoreSubmittingStatusText.text = "Submitted high score to server";
    }

    void OnHighScoreFailure(UnityEngine.Networking.UnityWebRequest.Result result)
    {
        scoreSubmittingStatusText.text = "Couldn't submit high score to server";
    }
    //---- END ADDED AFTER COMPO -------


    private void Island_TaskCompleteEvent()
    {
        IssueNextTask();
    }

    public event System.Action<Task> TaskIssuedEvent;
    Task currentTask;
    public void IssueNextTask()
    {
        firstTaskIssued = true;
        taskID++;
        if (taskID < tasks.Count)
        {
            currentTask = tasks[taskID];
        }
        else
        {
            currentTask = Task.RandomTask(island.RemainingLumberRoom());
        }
        timeLeftToComplete = currentTask.TimeToComplete;
        TaskIssuedEvent?.Invoke(currentTask);
    }

    public int TasksCompleted
    {
        get
        {
            return taskID;
        }
    }

    public enum GameOverCause
    { 
        TIME_OUT,
        HARDCORE_WASHED_AWAY,
        RETIRED
    }
}
