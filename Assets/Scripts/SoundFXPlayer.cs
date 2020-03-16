using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPlayer : MonoBehaviour
{
    public static AudioClip flick, fail;
    static AudioSource audioSrc;

    void Start()
    {
        flick = Resources.Load<AudioClip>("Audio/Arcade_Jump");
        fail = Resources.Load<AudioClip>("Audio/Cute_Water_Bubble");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void Play (string clip)
    {
        switch (clip)
        {
            case "flick":
                audioSrc.PlayOneShot(flick);
                break;
            case "fail":
                audioSrc.PlayOneShot(fail);
                break;
        }
    }
}
