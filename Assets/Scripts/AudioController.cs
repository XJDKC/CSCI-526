using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 1f)]
    public float pitch = 0.7f;

    private AudioSource theAS;

    public void SetUpSound(AudioSource _theAS) {
        theAS = _theAS;
        theAS.clip = clip;
    }

    public void PlayOneShot() {
        theAS.pitch = pitch;
        theAS.volume = volume;
        theAS.PlayOneShot(clip, volume);
    }

    public void Play() {
        theAS.pitch = pitch;
        theAS.volume = volume;
        theAS.Play();
    }
}

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField]
    private Sound[] sounds;

    Dictionary<string, Sound> audioMap = new Dictionary<string, Sound>();
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            foreach(var sound in sounds){
                audioMap.Add(sound.name, sound);
            }
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var sound in sounds) {
            sound.SetUpSound(new GameObject().AddComponent<AudioSource>());
        }
    }

    public void Play(string _name) {
        if (audioMap.ContainsKey(_name))
        {
            audioMap[_name].Play();
        }
    }

    public void PlayOneShot(string _name) {
        if (audioMap.ContainsKey(_name))
        {
            audioMap[_name].PlayOneShot();
        }
    }
}
