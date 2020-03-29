using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPlayer : MonoBehaviour
{
    public static AudioClip flick, win, fail, pick, put, go, positiveVibe;
    static AudioSource audioSrc;

    void Start()
    {
        flick = Resources.Load<AudioClip>("Audio/Arcade_Jump");
        win = Resources.Load<AudioClip>("Audio/Positive_Bell_Sound");
        fail = Resources.Load<AudioClip>("Audio/Game_Over");
        pick = Resources.Load<AudioClip>("Audio/Piece_Pick_Up");
        put = Resources.Load<AudioClip>("Audio/Piece_Put_Down");
        go = Resources.Load<AudioClip>("Audio/Go_Button");
        positiveVibe = Resources.Load<AudioClip>("Audio/Vibrant_Positive");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void Play (string clip)
    {
        switch (clip)
        {
            case "flick":
                audioSrc.PlayOneShot(flick);
                break;
            case "win":
                audioSrc.PlayOneShot(win);
                break;
            case "fail":
                audioSrc.PlayOneShot(fail);
                break;
            case "pick":
                audioSrc.PlayOneShot(pick);
                break;
            case "put":
                audioSrc.PlayOneShot(put);
                break;
            case "go":
                audioSrc.pitch = 0.95f;
                audioSrc.PlayOneShot(go);
                audioSrc.pitch = 1f;
                break;
            case "positiveVibe":
                audioSrc.PlayOneShot(positiveVibe);
                break;
        }
    }
}
