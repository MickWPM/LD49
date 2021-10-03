using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsMainMenu : MonoBehaviour
{
    public TMPro.TMP_InputField taskVisibleTimeText;
    public Toggle muteToggle;
    public Slider audioSlider;
    GameOptionsPersistent gameOptions;

    private void Update()
    {
        if(gameOptions == null)
        {
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        if (gameOptions == null)
        {
            gameOptions = GameObject.FindObjectOfType<GameOptionsPersistent>();
        }
        taskVisibleTimeText.text = gameOptions.NotificationTimeVisible.ToString();
        audioSlider.value = gameOptions.AudioVolume;
        muteToggle.isOn = gameOptions.MuteAudio;
    }

    public void SetAudioMute()
    {
        AudioListener.volume = muteToggle.isOn ? 0 : audioSlider.value;
    }

    public void SetVolumeLevel()
    {
        AudioListener.volume = muteToggle.isOn ? 0 : audioSlider.value;
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
    }
}
