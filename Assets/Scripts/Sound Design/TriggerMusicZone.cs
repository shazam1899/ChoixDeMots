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

    private IEnumerator Start()
    {
        // Solution n°2 : attendre 1 frame pour laisser le XR Origin se charger
        yield return null;

        // Solution n°1 : vérifier si le joueur spawn déjà dans la zone
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && musicOnEnter != null)
            {
                MusicPlayer.instance.PlayMusic(musicOnEnter, fadeDuration);
            }
        }
    }

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
