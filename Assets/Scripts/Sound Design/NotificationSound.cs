using UnityEngine;

public class NotificationSound : MonoBehaviour
{
    public static NotificationSound Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // Son 2D
    }

    public void PlayNotification(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}

