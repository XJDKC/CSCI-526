using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 1f)]
    public float pitch = 0.7f;

    private AudioSource _theAs;

    public void SetUpSound(AudioSource theAs) {
        _theAs = theAs;
        _theAs.clip = clip;
    }

    public void PlayOneShot() {
        SetPitchAndVolume();
        _theAs.PlayOneShot(clip, volume);
    }

    public void Play() {
        SetPitchAndVolume();
        _theAs.Play();
    }

    public void PlayBackgroundMusic()
    {
        SetPitchAndVolume();
        _theAs.loop = true;
        _theAs.Play();
    }

    public void SetPitchAndVolume()
    {
        _theAs.pitch = pitch;
        _theAs.volume = volume;
    }

    public void Stop()
    {
        _theAs.Stop();
    }
}

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance { get { return _instance; } }

    [SerializeField]
    private Sound[] sounds;

    private readonly Dictionary<string, Sound> _audioMap = new Dictionary<string, Sound>();
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            foreach(var sound in sounds){
                _audioMap.Add(sound.name, sound);
                sound.SetUpSound(gameObject.AddComponent<AudioSource>());
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    public void Play(string _name) {
        if (_audioMap.ContainsKey(_name))
        {
            _audioMap[_name].Play();
        }
    }

    public void PlayOneShot(string _name) {
        if (_audioMap.ContainsKey(_name))
        {
            _audioMap[_name].PlayOneShot();
        }
    }

    public void PlayBackgroundMusic(string _name) {
        if (_audioMap.ContainsKey(_name))
        {
            _audioMap[_name].PlayBackgroundMusic();
        }
    }

    //stop all bgms and sounds
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        foreach(var sound in sounds){
            sound.Stop();
        }
        String sceneName = scene.name;
        if (sceneName == "StartScene")
        {
            PlayBackgroundMusic("Main");
            return;
        }
        if (sceneName == "Menu")
        {
            PlayBackgroundMusic("Menu");
            return;
        }

        // now have 4 bgm music, loop to use them
        int level = int.Parse(sceneName.Substring(sceneName.IndexOf('l') + 1, 1));
        String bgmName = "BGM" + (level % 4 + 1);
        PlayBackgroundMusic(bgmName);
    }
}
