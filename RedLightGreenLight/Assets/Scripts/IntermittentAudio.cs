using System;
using System.Collections;
using UnityEngine;

public class IntermittentAudio : MonoBehaviour
{
    public float maxWaitSeconds;
    public float minWaitSeconds;
    public float mostRecentInterval;

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
        if (minWaitSeconds > maxWaitSeconds)
        {
            float f = minWaitSeconds;
            minWaitSeconds = maxWaitSeconds;
            maxWaitSeconds = f;
        }
        mostRecentInterval = audioSource.clip.length + UnityEngine.Random.Range(Math.Max(0, minWaitSeconds), Math.Max(0, maxWaitSeconds));
        yield return new WaitForSecondsRealtime(mostRecentInterval);

        while (true)
        {
            if (minWaitSeconds > maxWaitSeconds)
            {
                float f = minWaitSeconds;
                minWaitSeconds = maxWaitSeconds;
                maxWaitSeconds = f;
            }
            audioSource.Play();
            mostRecentInterval = audioSource.clip.length + UnityEngine.Random.Range(Math.Max(0, minWaitSeconds), Math.Max(0, maxWaitSeconds));
            yield return new WaitForSecondsRealtime(mostRecentInterval);
        }
    }
}
