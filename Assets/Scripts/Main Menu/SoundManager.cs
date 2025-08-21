using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    private Dictionary<Sound, AudioClip> soundDictionary;
    private AudioSource audioSource;

    public enum Sound {
        SnakeMove,
        SnakeDie,
        SnakeEat,
        ButtonClick,
        ButtonOver,
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        soundDictionary = new Dictionary<Sound, AudioClip>();

        foreach (var clip in soundAudioClipArray)
        {
            soundDictionary[clip.sound] = clip.audioClip;
        }
    }

    public void PlaySound(Sound sound) {
        if (soundDictionary.TryGetValue(sound, out AudioClip audioClip)) {
            audioSource.PlayOneShot(audioClip);
        } else {
            Debug.LogError($"Sound '{sound}' not found in dictionary!");
        }
    }

    public void SetButtonSounds(Button_UI buttonUI)
    {
        buttonUI.MouseOverOnceFunc += () => PlaySound(Sound.ButtonOver);
        buttonUI.ClickFunc += () => PlaySound(Sound.ButtonClick);
    }

}
