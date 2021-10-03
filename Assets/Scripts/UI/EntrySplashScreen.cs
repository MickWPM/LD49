using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySplashScreen : MonoBehaviour
{
    private void Start()
    {
        readyToLoad = false;
        loadText.SetActive(false);
        Invoke("ReadyToLoad", 0.5f);
    }

    public GameObject loadText;
    void ReadyToLoad()
    {
        loadText.SetActive(true);
        readyToLoad = true;
    }


    private void Update()
    {
        if (!readyToLoad) return;
        if (Input.anyKeyDown) LoadGame();
    }

    bool readyToLoad = false;
    void LoadGame()
    {

        Application.LoadLevel(1);
    }
}
