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

    bool loadFromBPM90;

    int repeatAmount = 2;
    int currentRepeat = 0;

    int r;
    int level;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = true;
        loadFromBPM90 = false;
        r = 0;
        level = CurrentLevel();

        LoadAudioLibrary();
        LoadAudioClip(r);
        
        audioSrc.Play();
        titleTxt.text = audioSrc.clip.name;
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
    
    void LoadRandomAudioClip()
    {
        currentRepeat = 0;

        int r = Random.Range(1, 13);
        audioClip = Resources.Load<AudioClip>("Audio/Bpm90/12/" + r.ToString());
        audioSrc.clip = audioClip;
    }
    */

    void LoadAudioLibrary()
    {
        if(loadFromBPM90 == false)
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
        else
        {
            if (level == 1 || level == 2)
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/Bpm90/12");
            else
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/Bpm90/34");
        }
    }

    void LoadAudioClip(int r)
    {
        audioSrc.clip = audioLibrary[r];
    }

    int CurrentLevel()
    {
        int level;

        if (ev.levelDFactor < -0.55f)
            level = 1;
        else if (ev.levelDFactor < 0f)
            level = 2;
        else if (ev.levelDFactor < 0.5f)
            level = 3;
        else
            level = 4;

        return level;
    }

    public void NextBGM()
    {
        if (level != CurrentLevel())
        {
            level = CurrentLevel();
            LoadAudioLibrary();
            r = 0;
        }
        else
        {
            r++;

            if (r == audioLibrary.Length) 
                r = 0;
        }

        LoadAudioClip(r);
        titleTxt.text = audioSrc.clip.name;
        audioSrc.Play();
    }

    public void SwitchAudioLibrary()
    {
        loadFromBPM90 = !loadFromBPM90;
        LoadAudioLibrary();
        r = 0;

        LoadAudioClip(r);
        titleTxt.text = audioSrc.clip.name;
        audioSrc.Play();
    }
}
