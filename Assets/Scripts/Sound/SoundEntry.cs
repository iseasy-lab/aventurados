using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEntry : MonoBehaviour
{
    private SoundEntry instance;
    
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

    
}
