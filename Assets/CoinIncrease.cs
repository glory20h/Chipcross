using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinIncrease : MonoBehaviour
{
    public AudioSource soundFXSource;
    Text coinText;

    void Start()
    {
        coinText = GetComponent<Text>();
    }

    /// TutorialPanelText 전용 함수로 변경 attach to Text element 
    public IEnumerator CoinIncreaseAnimation(int coin = 100)
    {
        int i = 0;
        soundFXSource.volume = 0.2f;
        while (i < coin + 1)
        {
            coinText.text = i.ToString();
            SoundFXPlayer.Play("coin");
            i += 1;
            yield return null;
        }
        soundFXSource.volume = 1f;
    }
}
