using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public Event ev;
    public Text titleText;

    static AudioClip[] audioLibrary;
    public static AudioClip audioClip;
    static AudioSource audioSrc;

    public bool loadFromBPM90 = true;

    int repeatAmount = 2;
    int currentRepeat = 0;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        LoadAudioLibrary(loadFromBPM90);
        LoadRandomAudioClip();

        audioSrc.loop = true;
        audioSrc.Play();
        titleText.text = audioSrc.clip.name;
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

    void LoadAudioLibrary(bool loadFromBPM90)
    {
        if(loadFromBPM90 == false)
        {
            if (ev.levelDFactor < -0.55f)
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level1");
            else if (ev.levelDFactor < 0f)
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level2");
            else if (ev.levelDFactor < 0.5f)
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level3");
            else
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/BGM/Level4");
        }
        else
        {
            if (ev.levelDFactor < 0f)
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/Bpm90/12");
            else
                audioLibrary = Resources.LoadAll<AudioClip>("Audio/Bpm90/34");
        }
    }

    void LoadRandomAudioClip()
    {
        audioSrc.clip = audioLibrary[Random.Range(0, audioLibrary.Length)];
    }

    public void SwitchBGM()
    {
        LoadRandomAudioClip();
        titleText.text = audioSrc.clip.name;
        audioSrc.Play();
    }
}
