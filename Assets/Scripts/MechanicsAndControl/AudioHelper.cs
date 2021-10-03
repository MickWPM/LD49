using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    [SerializeField] AudioClip buildingToBePlacedSelectedClip;
    [SerializeField] AudioClip newNotificationClip;
    AudioSource audioSource;
    BuildingPlacer buildingPlacer;
    GameManager gameManager;
    Island island;
    private void Awake()
    {
        gameManager = gameObject.GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        buildingPlacer = gameObject.GetComponent<BuildingPlacer>();
        buildingPlacer.BuildingSetToBePlacedEvent += BuildingPlacer_BuildingSetToBePlacedEvent;
        island = GameObject.FindObjectOfType<Island>();
        gameManager.TaskIssuedEvent += GameManager_TaskIssuedEvent;
    }
    bool okForNotification = true;
    private void GameManager_TaskIssuedEvent(Task obj)
    {
        if (okForNotification == false) return;
        okForNotification = false;
        Invoke("PlayNotification", 0.5f);
        Invoke("SetOkForNotification", 2f);
    }

    void PlayNotification()
    {
        PlayClip(newNotificationClip);
    }
    void SetOkForNotification()
    {
        okForNotification = true;
    }

    private void BuildingPlacer_BuildingSetToBePlacedEvent(Building obj)
    {
        if(buildingToBePlacedSelectedClip != null)
        {
            PlayClip(buildingToBePlacedSelectedClip);
        }
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
