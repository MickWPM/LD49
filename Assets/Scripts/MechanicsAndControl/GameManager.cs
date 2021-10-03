using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Task> tasks;
    int taskID = -1;
    Island island;

    GameOptionsPersistent gameOptions;

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
                Debug.Log("Normal mode");
                break;
            case GameOptionsPersistent.GameMode.HARDCORE:
                Debug.Log("Game over");
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
        Invoke("IssueNextTask", 2f);
    }

    private void Island_TaskCompleteEvent()
    {
        IssueNextTask();
    }

    public event System.Action<Task> TaskIssuedEvent;

    public void IssueNextTask()
    {
        taskID++;
        Task newTask;
        if (taskID < tasks.Count)
        {
            newTask = tasks[taskID];
        }
        else
        {
            newTask = Task.RandomTask();
        }
        TaskIssuedEvent?.Invoke(newTask);
    }

    public int TasksCompleted
    {
        get
        {
            return taskID;
        }
    }
}
