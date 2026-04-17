using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds;

    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Création des sources
        foreach (var s in sounds)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.clip = s.clip;
            src.volume = s.volume;
            src.loop = s.loop;

            s.source = src;
        }
    }

    public void Play(string name)
    {
        var s = FindSound(name);
        if (s != null)
            s.source.Play();
    }

    public void Stop(string name)
    {
        var s = FindSound(name);
        if (s != null)
            s.source.Stop();
    }

    private Sound FindSound(string name)
    {
        foreach (var s in sounds)
            if (s.name == name)
                return s;

        Debug.LogWarning("Son introuvable : " + name);
        return null;
    }
}

