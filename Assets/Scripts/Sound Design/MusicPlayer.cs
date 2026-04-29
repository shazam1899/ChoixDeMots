using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    private AudioSource audioSource;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // Son 2D
        audioSource.volume = 0f;
    }

    public void PlayMusic(AudioClip clip, float fadeDuration)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeTo(clip, fadeDuration));
    }

    private IEnumerator FadeTo(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        audioSource.volume = 1f;
    }
}

