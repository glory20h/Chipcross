using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueEvent : MonoBehaviour
{
    [SerializeField] private Button displayButton;
    private Image displayImage;

    private enum State { FadeIn, FadeOut }
    private State currentState;

    private int index;
    private float alphaChange;

    void Start()
    {
        index = 1;
        alphaChange = 0.002f;//내리면 속도 감소
        currentState = State.FadeIn;
        displayImage = displayButton.GetComponent<Image>();
    }

    void Update()
    {
        Color newColor = displayImage.color + new Color(0, 0, 0, alphaChange);
        displayImage.color = newColor;

        switch (currentState)
        {
            case State.FadeIn:
                if (newColor.a > 1f)
                {
                    currentState = State.FadeOut;
                    alphaChange = -alphaChange;
                }
                break;
            case State.FadeOut:
                if (newColor.a < 0f)
                {
                    currentState = State.FadeIn;
                    alphaChange = -alphaChange;
                    LoadNextCut();
                }
                break;
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
            displayImage.sprite = Resources.Load<Sprite>($"Arts/Story/{index}");
        }
    }

    public void SkipPrologue()
    {
        SceneManager.LoadScene("MainBoard");
    }
}
