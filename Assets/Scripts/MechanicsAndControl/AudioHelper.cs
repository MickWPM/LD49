using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    [SerializeField] AudioClip buildingToBePlacedSelectedClip;
    AudioSource audioSource;
    BuildingPlacer buildingPlacer;
    Island island;
    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        buildingPlacer = gameObject.GetComponent<BuildingPlacer>();
        buildingPlacer.BuildingSetToBePlacedEvent += BuildingPlacer_BuildingSetToBePlacedEvent;
        island = GameObject.FindObjectOfType<Island>();
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
