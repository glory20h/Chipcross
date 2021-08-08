using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAnim : MonoBehaviour
{

    float time = 0;
    [SerializeField]Image Text;
    // Update is called once per frame
    void Update()
    {
        if (time < 3f)
        {
            Text.color = new Color(1, 1, 1, time/3);
        }
        else
        {
            time = 0;
            this.gameObject.SetActive(false);
        }
        time += Time.deltaTime;

    }

    public void resetAnim()
    {
        Text.color = new Color(1, 1, 1, 0);
        this.gameObject.SetActive(true);
        time = 0;
    }
}
