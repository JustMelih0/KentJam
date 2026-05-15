using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private string currentMusicName;
    private int lastLevelMusicIndex = -1;

    private readonly List<string> levelMusicNames = new List<string>
    {
        "GameMusic2",
        "GameMusic3",
        "GameMusic4"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        HandleSceneMusic(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.StartsWith("Level"))
        {
            if (!musicSource.isPlaying)
            {
                PlayRandomLevelMusic();
            }
        }
        else if (currentScene == "Menu")
        {
            if (currentMusicName != "GameMusic1" || !musicSource.isPlaying)
            {
                PlayMusic("GameMusic1");
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleSceneMusic(scene.name);
    }

    private void HandleSceneMusic(string sceneName)
    {
        if (sceneName == "Menu")
        {
            if (currentMusicName != "GameMusic1")
            {
                PlayMusic("GameMusic1");
            }
        }
        else if (sceneName.StartsWith("Level"))
        {
            if (musicSource.isPlaying && IsCurrentMusicLevelMusic())
            {
                return;
            }

            PlayRandomLevelMusic();
        }
    }

    private bool IsCurrentMusicLevelMusic()
    {
        return levelMusicNames.Contains(currentMusicName);
    }

    private void PlayRandomLevelMusic()
    {
        if (levelMusicNames.Count == 0) return;

        int randomIndex;

        if (levelMusicNames.Count == 1)
        {
            randomIndex = 0;
        }
        else
        {
            do
            {
                randomIndex = UnityEngine.Random.Range(0, levelMusicNames.Count);
            }
            while (randomIndex == lastLevelMusicIndex);
        }

        lastLevelMusicIndex = randomIndex;
        PlayMusic(levelMusicNames[randomIndex]);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("ses yok: " + name);
            return;
        }

        musicSource.clip = s.clip;
        musicSource.Play();
        currentMusicName = name;
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("ses yok");
        }
        else
        {
            Debug.Log("ses çaldı: " + s.clip.name);
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleSound()
    {
        musicSource.mute = !musicSource.mute;
        sfxSource.mute = !sfxSource.mute;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}