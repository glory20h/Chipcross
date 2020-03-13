using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXScript : MonoBehaviour
{
    public static AudioClip boyFlick, fail;
    static AudioSource audioSrc;

    void Start()
    {
        boyFlick = Resources.Load<AudioClip>("Audio/Arcade_Jump");
        fail = Resources.Load<AudioClip>("Audio/Cute_Water_Bubble");
    }

    
    void Update()
    {
        
    }
}
