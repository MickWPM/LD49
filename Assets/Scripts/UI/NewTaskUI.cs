using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTaskUI : MonoBehaviour
{
    public GameObject taskGO;
    GameManager gameManager;
    public Image newTaskImage;
    public TMPro.TextMeshProUGUI taskText;
    bool hideTaskAfterCountdown = false;
    float taskHideCountdownRemaining;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.TaskIssuedEvent += GameManager_TaskIssuedEvent;
        ResetTaskUI();
        taskGO.SetActive(false);
    }

    Coroutine taskCoroutine;
    Task currentTask;
    private void GameManager_TaskIssuedEvent(Task newTask)
    {
        if(taskCoroutine != null)
        {
            StopCoroutine(taskCoroutine);
            ResetTaskUI();
        }
        currentTask = newTask;
        StartCoroutine(ShowTaskPopup());

        if(gameManager.GameOptionsPersistent != null)
        {
            taskHideCountdownRemaining = gameManager.GameOptionsPersistent.NotificationTimeVisible;
            hideTaskAfterCountdown = taskHideCountdownRemaining <= 0 ? false : true;
        } 
    }

    private void Update()
    {
        if (hideTaskAfterCountdown == false) return;

        if (taskGO.activeSelf == false) return;

        taskHideCountdownRemaining -= Time.deltaTime;
        if(taskHideCountdownRemaining < 0)
        {
            if(coroutining)
            {
                StopCoroutine(taskCoroutine);
            }
            taskGO.SetActive(false);
        }
    }

    bool coroutining = false;
    private IEnumerator ShowTaskPopup()
    {
        coroutining = true;
        taskGO.SetActive(true);
        taskText.text = $"---- NEW TASK ---- \n{currentTask.ToString()}";
        float timer = 0;
        while(timer < 0.5f)
        {
            newTaskImage.color = new Color(1, 1, 1, timer * 2);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        newTaskImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(.5f);
        timer += .5f;
        while (timer < 1.5f)
        {
            newTaskImage.color = new Color(1, 1, 1, 2* (1.5f - timer));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        newTaskImage.color = new Color(1, 1, 1, 0);
        coroutining = false;
    }

    void ResetTaskUI()
    {
        newTaskImage.color = new Color(1, 1, 1, 0);
    }
}
