using System;
using System.Collections;
using UnityEngine;

public class IntermittentAudio : MonoBehaviour
{
    public float maxWaitSeconds;
    public float minWaitSeconds;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (audioSource != null) StartCoroutine(AudioLoop());
    }

    IEnumerator AudioLoop()
    {
        while (true)
        {
            if (minWaitSeconds > maxWaitSeconds)
            {
                float f = minWaitSeconds;
                minWaitSeconds = maxWaitSeconds;
                maxWaitSeconds = f;
            }
            audioSource.Play();
            float nextInterval = audioSource.clip.length + UnityEngine.Random.Range(Math.Max(0, minWaitSeconds), Math.Max(0, maxWaitSeconds));
            yield return new WaitForSecondsRealtime(nextInterval);
        }
    }
}
