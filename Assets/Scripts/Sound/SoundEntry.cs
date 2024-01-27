using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEntry : MonoBehaviour
{
    public static SoundEntry instance;
    public static AudioSource musicSource;
    
    public SoundEntry Instance
    {
        get
        {
            if (instance == null)
            {
                instance = this;
            }
            return instance;
        }
    }

    

    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
/*
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    public void PlayMusic()
    {
        musicSource.UnPause();
    }
*/
    
}
