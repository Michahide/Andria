using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musikMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private Sound[] sounds;


    private void Start()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
            // SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musikMixerGroup;
                    break;
            }
        }
    }

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "Death" || scene.name == "HappyEnding")
    //     {
    //         this.gameObject.SetActive(false);
    //         Debug.Log("I am inside the if statement");
    //     }
    //     else
    //     {
    //         this.gameObject.SetActive(true);
    //     }
    // }

    public void Play(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);

        Debug.Log("Clipname: " + clipname);
        if (s == null)
        {
            Debug.LogError("Sound: " + clipname + " does NOT exist!");
            return;
        }

        // if(s.source == null)
        //     gameObject.AddComponent<AudioSource>();

        Debug.Log("S.source: " + s.source);
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
        soundEffectsMixerGroup.audioMixer.SetFloat("SFX", Mathf.Log10(AudioOptionsManager.soundEffectsVolume) * 20);
    }

    public void muteMusic(bool isMusicMute)
    {
        if (isMusicMute)
        {
            musikMixerGroup.audioMixer.SetFloat("Music", -80);
        }
        else
        {
            musikMixerGroup.audioMixer.SetFloat("Music", AudioOptionsManager.musicVolume);
        }
    }

    public void muteSFX(bool isSFXMute)
    {
        if (isSFXMute)
        {
            soundEffectsMixerGroup.audioMixer.SetFloat("SFX", -80);
        }
        else
        {
            soundEffectsMixerGroup.audioMixer.SetFloat("SFX", AudioOptionsManager.soundEffectsVolume);
        }
    }
}