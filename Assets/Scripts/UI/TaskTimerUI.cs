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
        gameManager.GameStartedEvent += GameManager_GameStartedEvent;
    }

    bool showTimer;
    private void GameManager_GameStartedEvent(GameOptionsPersistent.GameMode mode)
    {
        switch (mode)
        {
            case GameOptionsPersistent.GameMode.NORMAL:
            case GameOptionsPersistent.GameMode.HARDCORE:
                showTimer = false;
                break;
            case GameOptionsPersistent.GameMode.ZEN:
            case GameOptionsPersistent.GameMode.HARDCORE_ZEN:
                showTimer = true;
                break;
            default:
                Debug.LogError("Unhandled game mode : " + mode);
                break;
        }
    }

    private void GameManager_GameOverEvent(GameManager.GameOverCause obj)
    {
        timerGO.SetActive(false);
    }

    private void GameManager_TaskIssuedEvent(Task obj)
    {
        if (showTimer == false) return;
        if (gameManager.CurrentGameMode == GameOptionsPersistent.GameMode.HARDCORE_ZEN || gameManager.CurrentGameMode == GameOptionsPersistent.GameMode.ZEN) return;
        timerFillImage.fillAmount = 0;
        timerGO.SetActive(true);
    }

    private void Update()
    {
        if (timerGO.activeSelf == false) return;
        if(showTimer == false)
        {
            timerGO.SetActive(false);
            return;
        }

        float timeLeft = gameManager.GetTaskTimeRemaining();
        if (timeLeft < 0) return;

        timerFillImage.fillAmount = 1 - timeLeft;
    }
}
