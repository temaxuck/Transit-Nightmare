using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float fadeInTime = 1.0f;
    [SerializeField] private float fadeOutTime = 1.0f;
    [SerializeField] private float maxVolume = 1.0f;

    private Coroutine fadeCoroutine;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.clip = audioClip;
        audioSource.volume = 0;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeInAudio());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeOutAudio());
        }
    }

    private IEnumerator FadeInAudio()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, maxVolume, elapsedTime / fadeInTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;
        float elapsedTime = 0;

        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeOutTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
