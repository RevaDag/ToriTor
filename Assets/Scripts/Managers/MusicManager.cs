using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    public AudioClip mainMusic;


    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);


        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayThemeSong ()
    {
        PlayAudioClip(mainMusic);
    }


    public void PlayAudioClip ( AudioClip _clip )
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}

