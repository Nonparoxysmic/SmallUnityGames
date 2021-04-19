using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public float secondsToFade;
    [SerializeField] AudioClip deathSound;
    AudioSource audioSource;
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
                if (secondsToFade > 0)
                {
                    audioSource.volume -= Time.deltaTime / secondsToFade;
                    audioSource.volume = Math.Max(0, audioSource.volume);
                }
                else audioSource.volume = 0;
            }
            else
            {
                fadingOut = false;
                audioSource.Stop();
            }
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = 1;
            audioSource.Play();
        }
    }

    public void FadeOutMusic()
    {
        fadingOut = true;
    }

    public void PlayDeathSound()
    {
        audioSource.volume = 1;
        audioSource.PlayOneShot(deathSound);
    }
}
