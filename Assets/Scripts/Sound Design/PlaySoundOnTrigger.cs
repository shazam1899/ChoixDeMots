using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource source;
    public string targetTag;

    public bool useVelocity = true;
    public float minVelocity = 0;
    public float maxVelocity = 2;

    public bool randomizePitch = true;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag))
            return;

        if (randomizePitch)
            source.pitch = Random.Range(minPitch, maxPitch);

        float volume = 1f;

        if (useVelocity)
        {
            VelocityEstimator estimator = other.GetComponent<VelocityEstimator>();
            if (estimator)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                volume = Mathf.InverseLerp(minVelocity, maxVelocity, v);
            }
        }

        source.PlayOneShot(clip, volume);
    }
}

