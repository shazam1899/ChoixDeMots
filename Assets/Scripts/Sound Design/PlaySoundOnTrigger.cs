using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource source;
    public string targetTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag(targetTag))
        {
            source.PlayOneShot(clip);
        }
    }
}
