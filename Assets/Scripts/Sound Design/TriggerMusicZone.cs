using UnityEngine;
using System.Collections;

public class MusicTriggerZone : MonoBehaviour
{
    [Header("Musique quand le joueur ENTRE")]
    public AudioClip musicOnEnter;

    [Header("Musique quand le joueur SORT")]
    public AudioClip musicOnExit;

    [Header("Durée du fade (secondes)")]
    public float fadeDuration = 1.5f;

    private AudioSource audioSource;
    private Coroutine currentFade;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0f; // commence silencieux
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayWithFade(musicOnEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayWithFade(musicOnExit);
        }
    }

    private void PlayWithFade(AudioClip newClip)
    {
        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeToClip(newClip));
    }

    private IEnumerator FadeToClip(AudioClip newClip)
    {
        // Fade out
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }
}


