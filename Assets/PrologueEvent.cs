using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueEvent : MonoBehaviour
{
    public Button display_button;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 1;
    }

    public void LoadNextCut()
    {
        index++;
        if (index == 21)
        {
            SceneManager.LoadSceneAsync("MainBoard");
        }
        else
        {
            display_button.image.sprite = Resources.Load<Sprite>("Arts/Story/" + index.ToString());
        }
    }
}
