using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWestleft : MonoBehaviour
{
    Animator anim;
    public TutorialAnimationFixed tuto;
    int tile5 = Animator.StringToHash("Tile5");
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     if(PlayerPrefs.GetInt("tutorial") == 0)
        {
            anim.enabled = false;
        }
     else if(PlayerPrefs.GetInt("tutorial") == 1/*&& PlayerPrefs.GetInt("Piecedata") == 1*/)
        {
            switch(tuto.piece)
            {
                case 1:
                    anim.SetInteger("Tilenumber", 5);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
     else if(PlayerPrefs.GetInt("tutorial") == 1 && PlayerPrefs.GetInt("Piecedata") == 2)
        {
            anim.enabled = true;
            switch (tuto.piece)
            {
                case 1:
                    break;
                case 2:
                    break;
            }
        }
        else
        {
            anim.enabled = true;
        }
    }
}
