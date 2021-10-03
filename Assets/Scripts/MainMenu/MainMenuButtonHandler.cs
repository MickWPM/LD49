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
    public GameObject popupGO;
    public TMPro.TextMeshProUGUI popupText;
    private void Awake()
    {
        HideNewGameButtons();
        HideOptionsScreen();
        HideHowToPlay();
        popupGO.SetActive(false);
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

    public void StartZen()
    {
        GameObject.FindObjectOfType<GameOptionsPersistent>().GameModeSelected = GameOptionsPersistent.GameMode.ZEN;
        HideMapOptions();
        ShowMapOptions();
    }

    public void StartHardcoreZen()
    {
        GameObject.FindObjectOfType<GameOptionsPersistent>().GameModeSelected = GameOptionsPersistent.GameMode.HARDCORE_ZEN;
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

    #region Popups
    string NormalModeText = "Normal Mode:\nTasks are given with a timer to complete. If buildings get washed away you need to rebuild before the task completes.";
    string HardcoreModeText = "Hardcore Mode:\nTasks are given with a timer to complete. If buildings get washed away you lose immediately.";
    string ZenModeText = "Zen Mode:\nTasks are still given but there are no timers and you are under no obligation to complete them. Enjoy!";
    string HardcoreZenModeText = "Hardcore Zen:\nZen mode... but if you lose any buildings you lose immediately. For the patient perfectionist.";

    public void HidePopup()
    {
        popupGO.SetActive(false);
    }

    public void ShowNormalPopup()
    {
        ShowPopup(GameOptionsPersistent.GameMode.NORMAL);
    }

    public void ShowHardcorePopup()
    {
        ShowPopup(GameOptionsPersistent.GameMode.HARDCORE);
    }

    public void ShowZenPopup()
    {
        ShowPopup(GameOptionsPersistent.GameMode.ZEN);
    }

    public void ShowHardcoreZenPopup()
    {
        ShowPopup(GameOptionsPersistent.GameMode.HARDCORE_ZEN);
    }

    public void ShowPopup(GameOptionsPersistent.GameMode gameMode)
    {
        string s = "";
        switch (gameMode)
        {
            case GameOptionsPersistent.GameMode.NORMAL:
                s = NormalModeText;
                break;
            case GameOptionsPersistent.GameMode.HARDCORE:
                s = HardcoreModeText;
                break;
            case GameOptionsPersistent.GameMode.ZEN:
                s = ZenModeText;
                break;
            case GameOptionsPersistent.GameMode.HARDCORE_ZEN:
                s = HardcoreZenModeText;
                break;
            default:
                Debug.LogError($"Game mode {gameMode} is not accounted for");
                return;
        }
        popupText.text = s;
        popupGO.SetActive(true);
    }

    #endregion
}
