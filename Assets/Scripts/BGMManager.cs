using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public Event ev;

    int[] audioPool;
    static AudioClip[] audioLibrary;
    public static AudioClip audioClip;
    static AudioSource audioSrc;

    //Going to be used for Auto-Loop
    //int repeatAmount = 2;
    //int currentRepeat = 0;

    int level;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = false;
        level = CurrentLevel();

        LoadAudioLibrary();
        //LoadAudioClip(idx);
        LoadRandomAudioClip();
        
        audioSrc.Play();
        
    }

    //AUTO-LOOPING VERSION
    /*
    void Update()
    {
        if (!audioSrc.isPlaying)
        {
            if (currentRepeat < repeatAmount)
            {
                audioSrc.Play();
                currentRepeat++;
                Debug.Log("currentRepeat : " + currentRepeat);
            }
            else
            {
                LoadRandomAudioClip();
            }
        }
    }
    */

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
        if(scene.name == "MainBoard")
        {
            ev = FindObjectOfType<Event>();
        }
    }

    void LoadRandomAudioClip()
    {
        if (level != CurrentLevel())
        {
            level = CurrentLevel();
            PlayerPrefs.SetInt("BGMLevel", level);
            LoadAudioLibrary();
        }
        audioSrc.clip = audioLibrary[audioPool[Random.Range(1, audioPool.Length)]];
        if(ev) ev.DisplayBGMTitle(audioSrc.clip.name);
    }

    void LoadAudioLibrary()
    {
        if (level == 1)
        {
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/lib1-2");
            audioPool = new int[] { 0, 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13 };
        }
        else if (level == 2)
        {
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/lib1-2");
            audioPool = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 14, 15, 16, 17, 18 };
        }
        else if (level == 3)
        {
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/lib3-4");
            audioPool = new int[] { 0, 2, 4, 5, 6, 7, 8, 9, 10, 11 };
        }
        else
        {
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/lib3-4");
            audioPool = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        }
    }

    int CurrentLevel()
    {
        int level;

        if (ev)
        {
            if (ev.levelDFactor < -0.55f)
                level = 1;
            else if (ev.levelDFactor < 0f)
                level = 2;
            else if (ev.levelDFactor < 0.5f)
                level = 3;
            else
                level = 4;
        }
        else
        {
            level = PlayerPrefs.GetInt("BGMLevel", 1);
        }

        return level;
    }

    /*
    void LoadAudioClip(int r)
    {
        audioSrc.clip = audioLibrary[r];
    }
    */

    /*
    public void NextBGM()
    {
        if (level != CurrentLevel())
        {
            level = CurrentLevel();
            LoadAudioLibrary();
            idx = 0;
        }
        else
        {
            idx++;

            if (idx == audioLibrary.Length) 
                idx = 0;
        }

        LoadAudioClip(idx);
        titleTxt.text = audioSrc.clip.name;
        audioSrc.Play();
    }
    */
}
