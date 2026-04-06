using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
    public bool loop;
    //public bool PlayOneShot;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(this);
        }
        else
        {
            _instance = this;

        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            //s.source.PlayOneShot = s.PlayOneShot;
        }
    }
    public Sound[] sounds;
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play(); 
    }
    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
