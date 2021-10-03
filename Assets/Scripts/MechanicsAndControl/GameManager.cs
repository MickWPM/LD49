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
    }

    private void Island_BuildingUnderwaterEvent(Building obj)
    {
        if(gameOptions == null)
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();

        Debug.Log($"Game mode selected = {gameOptions.GameModeSelected}.", gameOptions.gameObject);
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
        Invoke("IssueNextTask", 2f);
        playing = true;
    }

    private void Update()
    {
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

    public event System.Action<GameOverCause> GameOverEvent;
    public void EndGame(GameOverCause cause)
    {
        Debug.Log("END GAME : " + cause);
    }

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
            currentTask = Task.RandomTask();
        }
        timeLeftToComplete = currentTask.TimeToComplete;
        TaskIssuedEvent?.Invoke(currentTask);
    }
    //public void IssueNextTask()
    //{
    //    firstTaskIssued = true;
    //    taskID++;
    //    Task newTask;
    //    if (taskID < tasks.Count)
    //    {
    //        newTask = tasks[taskID];
    //    }
    //    else
    //    {
    //        newTask = Task.RandomTask();
    //    }
    //    timeLeftToComplete = newTask.TimeToComplete;
    //    currentTask = newTask;
    //    TaskIssuedEvent?.Invoke(newTask);
    //}

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
