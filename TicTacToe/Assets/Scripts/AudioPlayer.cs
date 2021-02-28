using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    AudioSource audioSource;

    void Start()
    {
        if (audioMixer == null)
        {
            Debug.LogError(gameObject.name + ": Audio Mixer reference not set in the Inspector.");
            return;
        }
        audioSource = GameObject.Find("Audio Player").GetComponent<AudioSource>();
        StartCoroutine(PlayLoop());
    }

    IEnumerator PlayLoop()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            audioMixer.SetFloat("PitchShift", UnityEngine.Random.Range(0.6667f, 1.3333f));
            audioSource.volume = UnityEngine.Random.Range(0.05f, 0.5f);
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
