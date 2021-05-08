using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static AudioClip BGMClip;
    static AudioSource audioSrc;

    int repeatAmount = 3;
    int currentRepeat = 0;

    void Start()
    {
        LoadAudioClip();

        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = BGMClip;
        audioSrc.Play();
    }

    void Update()
    {
        if (!audioSrc.isPlaying)
        {
            if (currentRepeat < repeatAmount)
            {
                audioSrc.Play();
                currentRepeat++;
            }
            else
            {
                LoadAudioClip();
            }
        }
    }

    void LoadAudioClip()
    {
        currentRepeat = 0;

        // If Level 1,2 단계, Load from folder 12
        int r = Random.Range(1, 13);
        BGMClip = Resources.Load<AudioClip>("Audio/Bpm90/12/" + r.ToString());

        // If Level 3,4 단계, Load from folder 34
    }
}
