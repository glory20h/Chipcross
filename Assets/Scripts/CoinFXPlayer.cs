using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFXPlayer : MonoBehaviour
{
    public static AudioClip coin;
    static AudioSource audioSrc;
    
    void Start()
    {
        coin = Resources.Load<AudioClip>("Audio/FX/Quick_Coin");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void Play()
    {
        audioSrc.PlayOneShot(coin);
    }
}
