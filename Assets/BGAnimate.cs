using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnimate : MonoBehaviour
{
    public Transform cloud1;
    public Transform cloud2;
    public Transform meteor1;
    public Transform meteor2;

    public Transform star1;
    public Transform star2;
    public Transform starRing;

    Vector3 star1ScaleChange;
    Vector3 star1RingScaleChange;
    Vector3 star2ScaleChange;

    public Transform stars;

    int level;

    // Configuration Variables

    [Header("- Level1")]
    public float cloud1Speed = 0.2f;
    public float cloud2Speed = 0.16f;
    public float meteor1Speed = 1f;
    public float meteor2Speed = 1f;

    [Header("- Level2")]
    public float star1Scale = 0.005f;
    public float star1RingScale = 0.02f;
    public float star2Scale = 0.003f;

    [Header("- Level3")]

    [Header("- Level4")]
    public float starsSpeed = 0.015f;

    void Start()
    {
        star1ScaleChange = new Vector3(-star1Scale, -star1Scale, 0);
        star1RingScaleChange = new Vector3(star1RingScale, star1RingScale, 0);
        star2ScaleChange = new Vector3(star2Scale, star2Scale, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (level == 1)
        {
            // Animate clouds
            cloud1.Translate(Vector3.left * Time.deltaTime * cloud1Speed);
            if (cloud1.position.x < -12)
            {
                cloud1.position = new Vector3(12.5f, cloud1.position.y, cloud1.position.z);
            }

            cloud2.Translate(Vector3.left * Time.deltaTime * cloud2Speed);
            if (cloud2.position.x < -11)
            {
                cloud2.position = new Vector3(11.5f, cloud2.position.y, cloud2.position.z);
            }

            // Animate meteors
            meteor1.localPosition = meteor1.localPosition + (Vector3.down * Time.deltaTime * meteor1Speed);
            if (meteor1.localPosition.y > -1.5f)
            {
                meteor1.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, (3f - meteor1.localPosition.y) / (3f - (-1.5f)));
            }
            else
            {
                meteor1.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, (6f + meteor1.localPosition.y) / (-1.5f - (-6f)));
            }
            if (meteor1.localPosition.y < -20f)
            {
                meteor1.localPosition = new Vector3(0, 3f, 0);
                meteor1.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0f);
            }

            /*
            meteor2.localPosition = meteor2.localPosition + (Vector3.down * Time.deltaTime * meteor2Speed);
            if (meteor2.localPosition.y > -1.5f)
            {
                meteor2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.05f);
            }
            else
            {
                meteor2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -0.015f);
            }
            if (meteor2.localPosition.y < -20f)
            {
                meteor2.localPosition = new Vector3(0, 3f, 0);
                meteor2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0f);
            }
            */
        }

        if (level == 2)
        {
            star1.localScale += star1ScaleChange;
            if (star1.transform.localScale.y < 0.9f || star1.transform.localScale.y > 1.1f)
            {
                star1ScaleChange = -star1ScaleChange;
            }

            starRing.localScale += star1RingScaleChange;
            starRing.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -0.015f);
            if(starRing.localScale.x >= 2.5f)
            {
                starRing.localScale = new Vector3(1, 1, 1);
                starRing.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                star2.localScale = new Vector3(1, 1, 1);
            }

            // Animate Star & Star ring
            if(star2.localScale.x < 1.1f)
            {
                star2.localScale += star2ScaleChange;
            }
        }

        if (level == 3)
        {

        }

        if (level == 4)
        {
            stars.Translate(new Vector3(1.792f, -1) * Time.deltaTime * starsSpeed);
            if(stars.position.x > 9f)
            {
                stars.position = new Vector3(-9.23f, 5.14f);
            }
        }
    }

    public void ToggleBGAnim(float levelDFactor)
    {
        TurnOffAllBGAnim();
        if (levelDFactor < -0.55f)
        {
            cloud1.gameObject.SetActive(true);
            cloud2.gameObject.SetActive(true);
            level = 1;
        }
        else if (levelDFactor < 0f)
        {
            star1.gameObject.SetActive(true);
            star2.gameObject.SetActive(true);
            starRing.gameObject.SetActive(true);
            level = 2;
        }
        else if (levelDFactor < 0.5f)
        {
            level = 3;
        }
        else
        {
            stars.gameObject.SetActive(true);
            level = 4;
        }
    }

    void TurnOffAllBGAnim()
    {
        cloud1.gameObject.SetActive(false);
        cloud2.gameObject.SetActive(false);
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        starRing.gameObject.SetActive(false);
        stars.gameObject.SetActive(false);
    }
}
