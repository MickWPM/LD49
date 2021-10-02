using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Task> tasks;
    int taskID = -1;
    Island island;

    private void Awake()
    {
        island = GameObject.FindObjectOfType<Island>();
        island.TaskCompleteEvent += Island_TaskCompleteEvent;
    }

    private void Start()
    {
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
