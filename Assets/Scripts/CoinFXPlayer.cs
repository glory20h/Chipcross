using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFXPlayer : MonoBehaviour
{
    public static AudioClip coin;
    static AudioSource audioSrc;
    //void Awake()
    //{
    //    //Debug.Log("CoinFXPlayer Awake called");
    //    // ... (기존 코드)
    //}
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        
        coin = Resources.Load<AudioClip>("Audio/FX/Quick_Coin");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void Play()
    {
        audioSrc.PlayOneShot(coin);
    }
}
