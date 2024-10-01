using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource[] audioSources;

    int currentTrack = 0;

    void Start()
    {
        ShuffleTrackOrder();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            audioSources[currentTrack].Stop();

        if (!audioSources[currentTrack].isPlaying)
        {
            currentTrack++;

            if (currentTrack == audioSources.Length)
            {
                ShuffleTrackOrder();
                currentTrack = 0;
            }

            audioSources[currentTrack].Play();
        }
    }

    void ShuffleTrackOrder()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            int randomIndex = Random.Range(0, audioSources.Length);

            AudioSource temp = audioSources[i];
            audioSources[i] = audioSources[randomIndex];
            audioSources[randomIndex] = temp;
        }
    }
}
