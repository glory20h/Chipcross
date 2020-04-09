using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("tutorial") != 0)
        {
            GetComponent<Animator>().enabled = false;
        }
        else if(PlayerPrefs.GetInt("tutorial") == 0)
        {
            GetComponent<Animator>().enabled = true;
        }
    }
}
