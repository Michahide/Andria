using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musikMixerGroup;
    // [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private Sound[] sounds;

    private static AudioManager instance;

    private void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                // case Sound.AudioTypes.soundEffect:
                //     s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                //     break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musikMixerGroup;
                    break;
            }

            if (s.playOnAwake)
                s.source.Play();

            if (AudioManager.instance == null)
            {
                AudioManager.instance = this;
                GameObject.DontDestroyOnLoad(this.gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

     void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
       if (scene.name == "Death" || scene.name == "HappyEnding") {
            this.gameObject.SetActive(false);
             Debug.Log("I am inside the if statement");
       }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void Play(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does NOT exist!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does NOT exist!");
            return;
        }
        s.source.Stop();
    }

    public void UpdateMixerVolume()
    {
        musikMixerGroup.audioMixer.SetFloat("Music", Mathf.Log10(AudioOptionsManager.musicVolume) * 20);
        // soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(AudioOptionsManager.soundEffectsVolume) * 20);
    }
}