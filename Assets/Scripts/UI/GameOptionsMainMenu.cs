using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsMainMenu : MonoBehaviour
{
    public TMPro.TMP_InputField taskVisibleTimeText;
    public Toggle muteToggle;
    public Slider mouseScrollSlider;
    public Slider audioSlider;
    GameOptionsPersistent gameOptions;
    public Toggle disableOnlineScoreToggle;

    private void Update()
    {
        if(gameOptions == null)
        {
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
        }
    }

    private void OnEnable()
    {
        if (gameOptions == null)
        {
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
        }
        taskVisibleTimeText.text = gameOptions.NotificationTimeVisible.ToString();
        audioSlider.value = gameOptions.AudioVolume;
        muteToggle.isOn = gameOptions.MuteAudio;
        mouseScrollSlider.value = gameOptions.MouseScrollZoomSpeed;
        disableOnlineScoreToggle.isOn = gameOptions.DisableOnlineScores;
    }

    public void SetAudioMute()
    {
        AudioListener.volume = muteToggle.isOn ? 0 : audioSlider.value;
    }

    public void SetVolumeLevel()
    {
        AudioListener.volume = muteToggle.isOn ? 0 : audioSlider.value;
    }

    public void SetMouseScrollSpeed()
    {
        gameOptions.MouseScrollZoomSpeed = mouseScrollSlider.value;
    }

    public void SetDisableOnlineScores()
    {
        gameOptions.DisableOnlineScores = disableOnlineScoreToggle.isOn;
    }

    private void OnDisable()
    {
        float newTime;
        if(float.TryParse(taskVisibleTimeText.text, out newTime))
        {
            if(newTime >= 0)
            {
                gameOptions.NotificationTimeVisible = newTime;
            }
        }

        gameOptions.AudioVolume = audioSlider.value;
        gameOptions.MuteAudio = muteToggle.isOn;
        gameOptions.MouseScrollZoomSpeed = mouseScrollSlider.value;
        gameOptions.DisableOnlineScores = disableOnlineScoreToggle.isOn;
    }
}
