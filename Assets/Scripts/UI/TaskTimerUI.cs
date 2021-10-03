using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTimerUI : MonoBehaviour
{
    public Image timerFillImage;
    public GameObject timerGO;
    GameManager gameManager;

    private void Awake()
    {
        timerGO.SetActive(false);
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.TaskIssuedEvent += GameManager_TaskIssuedEvent;
        gameManager.GameOverEvent += GameManager_GameOverEvent;
    }

    private void GameManager_GameOverEvent(GameManager.GameOverCause obj)
    {
        timerGO.SetActive(false);
    }

    private void GameManager_TaskIssuedEvent(Task obj)
    {
        timerFillImage.fillAmount = 0;
        timerGO.SetActive(true);
    }

    private void Update()
    {
        if (timerGO.activeSelf == false) return;

        float timeLeft = gameManager.GetTaskTimeRemaining();
        if (timeLeft < 0) return;

        timerFillImage.fillAmount = 1 - timeLeft;
    }
}
