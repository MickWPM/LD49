using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonHandler : MonoBehaviour
{
    public GameObject[] NewGameButtons;
    public GameObject[] MapButtons;
    public GameObject OptionsGO;
    public GameObject HowToPlayGO;
    public Texture2D MainMenuCursor;
    private void Awake()
    {
        HideNewGameButtons();
        HideOptionsScreen();
        HideHowToPlay();
        Cursor.SetCursor(MainMenuCursor, Vector2.zero, CursorMode.Auto);
    }

    #region NewGame
    bool showingNewGameButtons = false;
    public void NewGameClicked()
    {
        HideOptionsScreen();
        HideHowToPlay();
        if (showingNewGameButtons)
        {
            HideNewGameButtons();
        } else
        {
            ShowNewGameButtons();
        }
        showingNewGameButtons = !showingNewGameButtons;
    }


    public void StartNormal()
    {
        GameObject.FindObjectOfType<GameOptionsPersistent>().GameModeSelected = GameOptionsPersistent.GameMode.NORMAL;
        HideMapOptions();
        ShowMapOptions();

    }
    public void StartHardcore()
    {
        GameObject.FindObjectOfType<GameOptionsPersistent>().GameModeSelected = GameOptionsPersistent.GameMode.HARDCORE;
        HideMapOptions();
        ShowMapOptions();
    }

    public void PlayTestMap()
    {
        Application.LoadLevel(1);
    }

    void HideNewGameButtons()
    {
        showingNewGameButtons = false;
        for (int i = 0; i < NewGameButtons.Length; i++)
        {
            NewGameButtons[i].SetActive(false);
        }
        HideMapOptions();
    }
    void ShowNewGameButtons()
    {
        for (int i = 0; i < NewGameButtons.Length; i++)
        {
            NewGameButtons[i].SetActive(true);
        }
    }

    void HideMapOptions()
    {
        for (int i = 0; i < MapButtons.Length; i++)
        {
            MapButtons[i].SetActive(false);
        }
    }
    void ShowMapOptions()
    {
        for (int i = 0; i < MapButtons.Length; i++)
        {
            MapButtons[i].SetActive(true);
        }

    }
    #endregion

    #region Options
    bool showingOptions = false;
    public void ToggleOptions()
    {
        HideNewGameButtons();
        HideHowToPlay();
        showingOptions = !showingOptions;
        OptionsGO.SetActive(showingOptions);
    }

    void HideOptionsScreen()
    {
        showingOptions = false;
        OptionsGO.SetActive(false);
    }

    #endregion

    #region HowToPlay
    bool showingHowtoPlay = false;
    public void ShowHowToPlay()
    {
        HideNewGameButtons();
        HideOptionsScreen();
        showingHowtoPlay = !showingHowtoPlay;
        HowToPlayGO.SetActive(showingHowtoPlay);
    }

    void HideHowToPlay()
    {
        showingHowtoPlay = false;
        HowToPlayGO.SetActive(false);
    }

    #endregion

}
