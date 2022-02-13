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
    public Transform starRing;
    public Transform star2;
    public Transform star3;
    public Transform star4;
    public Transform wave_2;
    public Transform planet_2;

    Vector3 star1ScaleChange;
    Vector3 star1RingScaleChange;



    public Transform stars;

    int level;

    // Configuration Variables

    [Header("- Level 1")]
    public float cloud1Speed = 0.2f;
    public float cloud2Speed = 0.16f;
    public float meteor1Speed = 2f;
    public float meteor2Speed = 1.5f;

    [Header("- Level 2")]
    public float star1Scale = 0.005f;
    public float star1RingScale = 0.015f;
    public float star2AlphaChange = 0.018f;
    public float star3AlphaChange = 0.016f;
    public float star4AlphaChange = 0.017f;

    public float wave2Speed = 0.1f;

    [Header("- Level 3")]


    [Header("- Level 4")]
    public float starsSpeed = 0.015f;

    void Start()
    {
        star1ScaleChange = new Vector3(star1Scale, star1Scale, 0);
        star1RingScaleChange = new Vector3(star1RingScale, star1RingScale, 0);

        star2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star4.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
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
                meteor1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (4f - meteor1.localPosition.y) / (4f - (-1.5f)));
            }
            else
            {
                meteor1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (6f + meteor1.localPosition.y) / (-1.5f - (-6f)));
            }
            if (meteor1.localPosition.y < -200f)
            {
                meteor1.localPosition = new Vector3(0, 3f, 0);
            }

            meteor2.localPosition = meteor2.localPosition + (Vector3.down * Time.deltaTime * meteor2Speed);
            if (meteor2.localPosition.y > 0)
            {
                meteor2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (6f - meteor2.localPosition.y) / (6f - (0)));
            }
            else
            {
                meteor2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (4f + meteor2.localPosition.y) / (0 - (-4f)));
            }
            if (meteor2.localPosition.y < -500f)
            {
                meteor2.localPosition = new Vector3(0, 3f, 0);
            }
        }

        if (level == 2)
        {
            // Animate Star & Star ring
            star1.localScale += star1ScaleChange;
            starRing.localScale += star1RingScaleChange;
            starRing.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -0.02f);
            if (star1.localScale.x < 0.5f)
            {
                star1ScaleChange = -star1ScaleChange;
                starRing.localScale = new Vector3(0.7f, 0.7f, 1);
                starRing.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            else if (star1.localScale.x > 0.7f)
            {
                star1ScaleChange = -star1ScaleChange;
            }

            /*
            starRing.localScale += star1RingScaleChange;
            starRing.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -0.015f);
            if (starRing.localScale.x >= 1.5f)
            {
                starRing.localScale = new Vector3(1, 1, 1);
                starRing.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                star1.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            */

            star2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star2AlphaChange);
            if (star2.GetComponent<SpriteRenderer>().color.a >= 1f || star2.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star2AlphaChange = -star2AlphaChange;
            }

            star3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star3AlphaChange);
            if (star3.GetComponent<SpriteRenderer>().color.a >= 1f || star3.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star3AlphaChange = -star3AlphaChange;
            }

            star4.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star4AlphaChange);
            if (star4.GetComponent<SpriteRenderer>().color.a >= 1f || star4.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star4AlphaChange = -star4AlphaChange;
            }

            wave_2.Translate(new Vector3(-1, 0) * Time.deltaTime * wave2Speed);
            if (wave_2.position.x < -13.77f)
            {
                wave_2.position = new Vector3(13.8f, -0.54f);
            }
        }

        if (level == 3)
        {
            
        }

        if (level == 4)
        {
            stars.Translate(new Vector3(1.792f, -1) * Time.deltaTime * starsSpeed);
            if (stars.position.x > 9f)
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
            meteor1.gameObject.SetActive(true);
            meteor2.gameObject.SetActive(true);
            level = 1;
        }
        else if (levelDFactor < 0f)
        {
            star1.gameObject.SetActive(true);
            starRing.gameObject.SetActive(true);
            star2.gameObject.SetActive(true);
            star3.gameObject.SetActive(true);
            star4.gameObject.SetActive(true);
            wave_2.gameObject.SetActive(true);
            planet_2.gameObject.SetActive(true);
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
        meteor1.gameObject.SetActive(false);
        meteor2.gameObject.SetActive(false);
        star1.gameObject.SetActive(false);
        starRing.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        star4.gameObject.SetActive(false);
        wave_2.gameObject.SetActive(false);
        planet_2.gameObject.SetActive(false);
        stars.gameObject.SetActive(false);
    }
}
