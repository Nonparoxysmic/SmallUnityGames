using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public float secondsToFadeIn;
    public float secondsToFadeOut;

    AudioSource audioSource;
    bool fadingIn;
    bool fadingOut;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (fadingOut)
        {
            if (audioSource.volume > 0)
            {
                if (secondsToFadeOut > 0)
                {
                    audioSource.volume -= Time.deltaTime / secondsToFadeOut;
                    audioSource.volume = Math.Max(0, audioSource.volume);
                }
                else audioSource.volume = 0;
            }
            else
            {
                fadingOut = false;
            }
        }
        if (fadingIn)
        {
            if (audioSource.volume < 1)
            {
                if (secondsToFadeIn > 0)
                {
                    audioSource.volume += Time.deltaTime / secondsToFadeIn;
                    audioSource.volume = Math.Min(1, audioSource.volume);
                }
                else audioSource.volume = 1;
            }
            else
            {
                fadingIn = false;
            }
        }
    }

    public void FadeOutMusic()
    {
        fadingIn = false;
        fadingOut = true;
    }

    public void FadeInMusic()
    {
        fadingOut = false;
        fadingIn = true;
    }
}
