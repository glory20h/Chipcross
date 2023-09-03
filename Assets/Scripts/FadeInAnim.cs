using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAnim : MonoBehaviour
{
    private const float MAX_TIME = 3f;
    private const float INITIAL_ALPHA = 0f;
    private const float MAX_ALPHA = 1f;

    private float time = 0;
    [SerializeField]
    private Image Text;
    private Color textColor;
    // Update is called once per frame
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        textColor = new Color(1, 1, 1, INITIAL_ALPHA);
        Text.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < MAX_TIME)
        {
            textColor.a = time / MAX_TIME;
            Text.color = textColor;
        }
        else
        {
            ResetAnim();
        }
        time += Time.deltaTime;
    }

    public void ResetAnim()
    {
        Initialize();
        this.gameObject.SetActive(true);
        time = 0;
    }
}
