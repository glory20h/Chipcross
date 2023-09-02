using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private Event ev;

    private List<int> audioPool = new List<int>();
    private static AudioClip[] audioLibrary;
    private static AudioSource audioSrc;

    private int level;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = false;
        level = CurrentLevel();

        LoadAudioLibrary();
        LoadRandomAudioClip();
        audioSrc.Play();
    }

    void Update()
    {
        if (!audioSrc.isPlaying)
        {
            LoadRandomAudioClip();
            audioSrc.Play();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainBoard")
        {
            ev = FindObjectOfType<Event>();
        }
    }

    void LoadRandomAudioClip()
    {
        if (level != CurrentLevel())
        {
            level = CurrentLevel();
            PlayerPrefs.SetInt("CurrentLevel", level);
            LoadAudioLibrary();
        }
        audioSrc.clip = audioLibrary[audioPool[Random.Range(0, audioPool.Count)]];
        if (ev) ev.DisplayBGMTitle(audioSrc.clip.name);
    }

    void LoadAudioLibrary()
    {
        string path = "Audio/BGM/lib" + (level % 2 == 0 ? "1-2" : "3-4");
        audioLibrary = Resources.LoadAll<AudioClip>(path);

        audioPool.Clear();
        for (int i = 0; i < audioLibrary.Length; i++)
        {
            audioPool.Add(i);
        }
    }

    int CurrentLevel()
    {
        if (ev)
        {
            if (ev.levelDFactor < -0.55f) return 1;
            if (ev.levelDFactor < 0f) return 2;
            if (ev.levelDFactor < 0.5f) return 3;
            return 4;
        }
        return PlayerPrefs.GetInt("CurrentLevel", 1);
    }
}
