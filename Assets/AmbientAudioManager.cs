using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioManager : MonoBehaviour
{
    public AudioClip[] seagullSounds;
    public Vector2 seagullDelay = new Vector2(6, 20);
    AudioSource audioSource;

    float nextClipTimer;
    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        nextClipTimer = Random.Range(seagullDelay.x, seagullDelay.y);
    }

    void Update()
    {
        nextClipTimer -= Time.deltaTime;
        if(nextClipTimer < 0)
        {
            nextClipTimer = Random.Range(seagullDelay.x, seagullDelay.y);
            audioSource.PlayOneShot(seagullSounds[Random.Range(0, seagullSounds.Length)]);
        }
    }
}
