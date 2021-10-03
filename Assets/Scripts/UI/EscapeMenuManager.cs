using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuManager : MonoBehaviour
{
    public GameObject escapeMenuContainerGO;
    public GameObject escapeMenuGO;
    public GameObject optionsMenuGO;

    private void Awake()
    {
        CloseMenu();
        GameObject.FindObjectOfType<GameManager>().GameOverEvent += EscapeMenuManager_GameOverEvent;
    }

    private void EscapeMenuManager_GameOverEvent(GameManager.GameOverCause obj)
    {
        this.enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(escapeMenuContainerGO.activeSelf)
            {
                if(optionsMenuGO.activeSelf)
                {
                    CloseSettingsMenu();
                } else
                {
                    CloseMenu();
                }
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenSettingsMenu()
    {
        escapeMenuGO.SetActive(false);
        optionsMenuGO.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        //This will reset to default escape menu! hooray
        OpenMenu();
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        optionsMenuGO.SetActive(false);
        escapeMenuContainerGO.SetActive(true);
        escapeMenuGO.SetActive(true);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        optionsMenuGO.SetActive(false);
        escapeMenuContainerGO.SetActive(false);
        escapeMenuGO.SetActive(true);
    }
}
