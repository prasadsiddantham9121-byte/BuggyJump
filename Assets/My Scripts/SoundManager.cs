using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public AudioClip passClip;
    public AudioClip failClip;
    public AudioClip carSoundClip;
    public AudioClip skidClip;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void CarSound()
    {
        audioSource.clip = carSoundClip;
        audioSource.Play();
        audioSource.loop = true;

    }

    public void PassSound()
    {
        audioSource.clip = passClip;
        audioSource.Play();
        audioSource.loop = false;
    } 
    
    public void FailSound()
    {
        audioSource.clip = failClip;
        audioSource.Play();
        audioSource.loop = false;
    }
    
    public void SkidSoundStart()
    {
        audioSource.clip = skidClip;
        audioSource.Play();
        audioSource.loop = true;
    }
    
    public void SkidSoundStop()
    {
        audioSource.clip = skidClip;
        audioSource.Stop();
        
    }


}
