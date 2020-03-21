using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPlayer : MonoBehaviour
{
    public static AudioClip flick, fail, pick, put, go;
    static AudioSource audioSrc;

    void Start()
    {
        flick = Resources.Load<AudioClip>("Audio/Arcade_Jump");
        fail = Resources.Load<AudioClip>("Audio/Cute_Water_Bubble");
        pick = Resources.Load<AudioClip>("Audio/Piece_Pick_Up");
        put = Resources.Load<AudioClip>("Audio/Piece_Put_Down");
        go = Resources.Load<AudioClip>("Audio/Go_Button");

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
            case "pick":
                audioSrc.PlayOneShot(pick);
                break;
            case "put":
                audioSrc.PlayOneShot(put);
                break;
            case "go":
                audioSrc.pitch = 0.95f;
                audioSrc.PlayOneShot(go);
                break;
        }
    }
}
