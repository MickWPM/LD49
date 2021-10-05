using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionsPersistent : MonoBehaviour
{
    public GameMode GameModeSelected;


    public float NotificationTimeVisible = 10;

    public bool MuteAudio = false;

    public float AudioVolume = 1f;

    public float MouseScrollZoomSpeed = 10f;

    public bool DisableOnlineScores = false;

    private void Awake()
    {
        GameOptionsPersistent[] existingInstance = GameObject.FindObjectsOfType<GameOptionsPersistent>();
        if(existingInstance.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    [System.Serializable]
    public enum GameMode
    {
        NORMAL,
        HARDCORE,
        ZEN,
        HARDCORE_ZEN
    }

    //--- ADDED AFTER COMPO FOR HIGH SCORES ----
    public ShowHighScores.MapType IslandSelected;
    //---- END ADDED AFTER COMPO -------
}
