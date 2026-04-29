using UnityEngine;

public class MusicTriggerZone : MonoBehaviour
{
    [Header("Musique quand le joueur ENTRE")]
    public AudioClip musicOnEnter;

    [Header("Musique quand le joueur SORT")]
    public AudioClip musicOnExit;

    [Header("Durée du fade (secondes)")]
    public float fadeDuration = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && musicOnEnter != null)
        {
            MusicPlayer.instance.PlayMusic(musicOnEnter, fadeDuration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && musicOnExit != null)
        {
            MusicPlayer.instance.PlayMusic(musicOnExit, fadeDuration);
        }
    }
}



