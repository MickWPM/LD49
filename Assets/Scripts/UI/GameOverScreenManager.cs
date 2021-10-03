using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenManager : MonoBehaviour
{
    public GameObject gameOverScreenGO;
    public GameObject[] toDisable;
    public GameObject[] toDisableOnGameOverHideUI;
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.GameOverEvent += ShowGameOverScreen;
        gameOverScreenGO.SetActive(false);
    }

    bool showingGameOverScreen = false;
    private void Update()
    {
        if(showingGameOverScreen)
        {
            if(Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Escape))
            {
                bool setActive = true;
                if(gameOverScreenGO.activeSelf)
                {
                    setActive = false;
                }

                for (int i = 0; i < toDisableOnGameOverHideUI.Length; i++)
                {
                    toDisableOnGameOverHideUI[i].SetActive(setActive);
                }
            }
        }
    }

    public void ShowGameOverScreen(GameManager.GameOverCause cause)
    {
        Time.timeScale = 0;
        if (toDisable != null && toDisable.Length > 0)
        {
            for (int i = 0; i < toDisable.Length; i++)
            {
                toDisable[i].SetActive(false);
            }
        }
        SetGameOverText(cause);
        gameOverScreenGO.SetActive(true);
        showingGameOverScreen = true;
    }

    public TMPro.TextMeshProUGUI gameOverTextField;
    public void SetGameOverText(GameManager.GameOverCause cause)
    {
        string headerMessaage = "";
        switch (cause)
        {
            case GameManager.GameOverCause.TIME_OUT:
                headerMessaage = "Alright chump, times up. Look, I know its nice to take your time and slow down, live on island time and all that. If you are getting stuck, why not try zen mode?";
                break;
            case GameManager.GameOverCause.HARDCORE_WASHED_AWAY:
                headerMessaage = "Devestating - first loss of the village is enough for you to see this screen. Do you need a copy of the tide times? Pretty rough to play Hardcore, good on you for trying though.";
                break;
            case GameManager.GameOverCause.RETIRED:
                headerMessaage = "Congratulations on your retirement! (And being smart enough to palm off a sinking island to someone else to deal with - their problem now!)";
                break;
            default:
                break;
        }

        int tasksCompleted = gameManager.TasksCompleted < 0 ? 0 : gameManager.TasksCompleted;
        string tasksComplete = tasksCompleted.ToString();
        if(tasksCompleted == 0)
        {
            tasksComplete = "zero tasks. No tasks. None. I'll pretend I forgot to tell you that you were meant to do the";
        } else if (tasksCompleted == 1)
        {
            tasksComplete = "one task. Place centrally early game and you might complete more";
        }

        gameOverTextField.text = $"{headerMessaage} \n\nYou managed to complete {tasksComplete} tasks." +
            $"\n\nTake some time to enjoy your village - Press H to hide the UI and have a look around. Feel free to take a screenshot.\n\n" +
            $"Thanks for playing, I hope you enjoyed playing it as much as I did making it.";
    }

}
