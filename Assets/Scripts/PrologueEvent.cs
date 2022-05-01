using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueEvent : MonoBehaviour
{
    public Button display_button;

    int index;
    float a_change;
    float cum_change;

    // Start is called before the first frame update
    void Start()
    {
        index = 1;
        a_change = 0.02f;
    }

    void Update()
    {
        display_button.GetComponent<Image>().color += new Color(0, 0, 0, a_change);

        if (display_button.GetComponent<Image>().color.a > 6f)
        {
            display_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            a_change = -a_change;
        }
        else if (display_button.GetComponent<Image>().color.a < -1f)
        {
            display_button.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            a_change = -a_change;
            LoadNextCut();
        }
    }

    public void LoadNextCut()
    {
        index++;
        if (index == 21)
        {
            SceneManager.LoadScene("MainBoard");
        }
        else
        {
            display_button.image.sprite = Resources.Load<Sprite>("Arts/Story/" + index.ToString());
        }
    }

    public void SkipPrologue()
    {
        SceneManager.LoadScene("MainBoard");
    }
}
