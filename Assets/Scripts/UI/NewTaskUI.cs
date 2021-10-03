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
    }

    private IEnumerator ShowTaskPopup()
    {
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
        yield return new WaitForSeconds(2f);
        timer += 2;
        while (timer < 3f)
        {
            newTaskImage.color = new Color(1, 1, 1, 2* (3 - timer));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        newTaskImage.color = new Color(1, 1, 1, 0);
    }

    void ResetTaskUI()
    {
        newTaskImage.color = new Color(1, 1, 1, 0);
    }
}
