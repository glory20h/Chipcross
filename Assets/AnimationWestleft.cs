using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWestleft : MonoBehaviour
{
    AnimationClip clip;
    Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
     if(PlayerPrefs.GetInt("tutorial") == 0)
        {
            anim.AddClip(clip, "Tutorial Finger");
        }
    }
}
