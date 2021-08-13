using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public Event ev;
    public Text titleTxt;

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

    void LoadRandomAudioClip()
    {
        if (level != CurrentLevel())
        {
            level = CurrentLevel();
            PlayerPrefs.SetInt("BGMLevel", level);
            LoadAudioLibrary();
        }
        audioSrc.clip = audioLibrary[Random.Range(1, audioLibrary.Length)];
        if (titleTxt) titleTxt.text = audioSrc.clip.name;
    }

    void LoadAudioLibrary()
    {
        if (level == 1)
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level1");
        else if (level == 2)
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level2");
        else if (level == 3)
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level3");
        else
            audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level4");
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

    public void Transition()
    {
        ev = FindObjectOfType<Event>();
        titleTxt = GameObject.Find("BGMTitle").GetComponent<Text>();
        titleTxt.text = audioSrc.clip.name;
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
