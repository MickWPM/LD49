using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionsPersistent : MonoBehaviour
{
    public GameMode GameModeSelected;

    public float NotificationTimeVisible = 10;

    public bool MuteAudio = false;

    public float AudioVolume = 1f;

    private void Awake()
    {
        GameOptionsPersistent[] existingInstance = GameObject.FindObjectsOfType<GameOptionsPersistent>();
        if(existingInstance.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public enum GameMode
    {
        NORMAL,
        HARDCORE
    }
}
