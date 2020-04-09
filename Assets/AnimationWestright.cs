using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWestright : MonoBehaviour
{
    Animator anim;
    public TutorialAnimationFixed tuto;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("tutorial") == 0)
        {
            anim.enabled = false;
        }
        else if (PlayerPrefs.GetInt("tutorial") == 1/*&& PlayerPrefs.GetInt("Piecedata") == 1*/)
        {
        }
    }
}
