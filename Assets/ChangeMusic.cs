using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public GameObject spawner;
    public AudioClip fasterMusic;

    // Update is called once per frame
    void Update()
    {
        if(spawner.GetComponent<WaveSpawner>().currentWave > 4)
        {
            GetComponent<AudioSource>().clip = fasterMusic;
            if(GetComponent<AudioSource>().isPlaying == false)
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
