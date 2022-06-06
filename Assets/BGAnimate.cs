using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BGAnimate : MonoBehaviour
{
    public Transform cloud1;
    public Transform cloud2;
    public Transform meteor1;
    public Transform meteor2;

    public Transform star1_2;
    public Transform starRing;
    public Transform star2_2;
    public Transform star3_2;
    public Transform star4_2;
    public Transform wave_2;
    public Transform planet_2;

    Vector3 star1ScaleChange;
    Vector3 star1RingScaleChange;

    public Transform star1_3;
    public Transform star2_3;
    public Transform star3_3;
    public Transform star4_3;
    public Transform star5_3;
    public Transform wave1_3;
    public Transform wave2_3;
    public Transform wave3_3;
    public Transform planet_3;
    public Transform planet_light_3;

    public Transform stars_4;
    public Transform bigstar_4;
    public Transform midstar_4;
    public Transform smallstar_4;
    public VideoPlayer videoPlayer_4;

    int level;

    // Configuration Variables

    [Header("- Level 1")]
    public float cloud1Speed = 0.2f;
    public float cloud2Speed = 0.16f;
    public float meteor1Speed = 2f;
    public float meteor2Speed = 1.5f;

    [Header("- Level 2")]
    public float star1_2_Scale = 0.0001f;
    public float star1_2_RingScale = 0.015f;
    public float star2_2_AlphaChange = 0.018f;
    public float star3_2_AlphaChange = 0.016f;
    public float star4_2_AlphaChange = 0.017f;

    public float wave2Speed = 0.1f;

    [Header("- Level 3")]
    public float star1_3_AlphaChange = 0.018f;
    public float star2_3_AlphaChange = 0.018f;
    public float star3_3_AlphaChange = 0.018f;
    public float star4_3_AlphaChange = 0.018f;
    public float star5_3_AlphaChange = 0.018f;
    public float plantlight_3_AlphaChange = 0.018f;
    public float wave1_3_Speed = 0.1f;
    public float wave2_3_Speed = 0.1f;
    public float wave3_3_Speed = 0.1f;

    [Header("- Level 4")]
    public float starsSpeed = 0.015f;
    public GameObject backgroundObject;
    public GameObject backgroundsplite;

    void Start()
    {
        star1ScaleChange = new Vector3(star1_2_Scale, star1_2_Scale, 0);
        star1RingScaleChange = new Vector3(star1_2_RingScale, star1_2_RingScale, 0);

        star2_2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star3_2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star4_2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));

        star1_3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star2_3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star3_3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star4_3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star5_3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 1f));

        videoPlayer_4.Prepare();
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
            if (meteor1.localPosition.y < -100f)
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
            if (meteor2.localPosition.y < -250f)
            {
                meteor2.localPosition = new Vector3(0, 3f, 0);
            }
        }

        if (level == 2)
        {
            // Animate Star & Star ring
            star1_2.localScale += star1ScaleChange;
            starRing.localScale += star1RingScaleChange;
            starRing.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -0.02f);
            if (star1_2.localScale.x < 0.55f)
            {
                star1ScaleChange = -star1ScaleChange;
                starRing.localScale = new Vector3(0.7f, 0.7f, 1);
                starRing.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            else if (star1_2.localScale.x > 0.7f)
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

            star2_2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star2_2_AlphaChange);
            if (star2_2.GetComponent<SpriteRenderer>().color.a >= 1f || star2_2.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star2_2_AlphaChange = -star2_2_AlphaChange;
            }

            star3_2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star3_2_AlphaChange);
            if (star3_2.GetComponent<SpriteRenderer>().color.a >= 1f || star3_2.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star3_2_AlphaChange = -star3_2_AlphaChange;
            }

            star4_2.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star4_2_AlphaChange);
            if (star4_2.GetComponent<SpriteRenderer>().color.a >= 1f || star4_2.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star4_2_AlphaChange = -star4_2_AlphaChange;
            }

            wave_2.Translate(new Vector3(-1, 0) * Time.deltaTime * wave2Speed);
            if (wave_2.position.x < -13.77f)
            {
                wave_2.position = new Vector3(13.8f, -0.54f);
            }
        }

        if (level == 3)
        {
            star1_3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star1_3_AlphaChange);
            if (star1_3.GetComponent<SpriteRenderer>().color.a >= 1f || star1_3.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star1_3_AlphaChange = -star1_3_AlphaChange;
            }

            star2_3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star2_3_AlphaChange);
            if (star2_3.GetComponent<SpriteRenderer>().color.a >= 1f || star2_3.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star2_3_AlphaChange = -star2_3_AlphaChange;
            }

            star3_3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star3_3_AlphaChange);
            if (star3_3.GetComponent<SpriteRenderer>().color.a >= 1f || star3_3.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star3_3_AlphaChange = -star3_3_AlphaChange;
            }

            star4_3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star4_3_AlphaChange);
            if (star4_3.GetComponent<SpriteRenderer>().color.a >= 1f || star4_3.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star4_3_AlphaChange = -star4_3_AlphaChange;
            }

            star5_3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -star5_3_AlphaChange);
            if (star5_3.GetComponent<SpriteRenderer>().color.a >= 1f || star5_3.GetComponent<SpriteRenderer>().color.a <= 0)
            {
                star5_3_AlphaChange = -star5_3_AlphaChange;
            }

            wave1_3.Translate(new Vector3(1.778f, -1) * Time.deltaTime * wave1_3_Speed);
            if (wave1_3.position.x > 3.1f)
            {
                wave1_3.position = new Vector3(-14.83f, 1.19f);
            }

            wave2_3.Translate(new Vector3(1, 0) * Time.deltaTime * wave2_3_Speed);
            if (wave2_3.position.x > 9.05f)
            {
                wave2_3.position = new Vector3(-8.6f, -3.15f);
            }

            wave3_3.Translate(new Vector3(-1.777f, 1) * Time.deltaTime * wave3_3_Speed);
            if (wave3_3.position.x < -5.09f)
            {
                wave3_3.position = new Vector3(12.818f, -3.674f);
            }

            planet_light_3.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -plantlight_3_AlphaChange);
            if (planet_light_3.GetComponent<SpriteRenderer>().color.a >= 2f || planet_light_3.GetComponent<SpriteRenderer>().color.a <= 0f)
            {
                plantlight_3_AlphaChange = -plantlight_3_AlphaChange;
            }
        }

        if (level == 4)
        {
            stars_4.Translate(new Vector3(1.777f, -1) * Time.deltaTime * starsSpeed);
            if (stars_4.position.x > 8.96f)
            {
                stars_4.position = new Vector3(-8.96f, 4.98f);
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
            star1_2.gameObject.SetActive(true);
            starRing.gameObject.SetActive(true);
            star2_2.gameObject.SetActive(true);
            star3_2.gameObject.SetActive(true);
            star4_2.gameObject.SetActive(true);
            wave_2.gameObject.SetActive(true);
            planet_2.gameObject.SetActive(true);
            level = 2;
        }
        else if (levelDFactor < 0.5f)
        {
            star1_3.gameObject.SetActive(true);
            star2_3.gameObject.SetActive(true);
            star3_3.gameObject.SetActive(true);
            star4_3.gameObject.SetActive(true);
            star5_3.gameObject.SetActive(true);
            wave1_3.gameObject.SetActive(true);
            wave2_3.gameObject.SetActive(true);
            wave3_3.gameObject.SetActive(true);
            planet_3.gameObject.SetActive(true);
            planet_light_3.gameObject.SetActive(true);
            level = 3;
        }
        else
        {
            stars_4.gameObject.SetActive(true);
            bigstar_4.gameObject.SetActive(true);
            midstar_4.gameObject.SetActive(true);
            smallstar_4.gameObject.SetActive(true);
            
            videoPlayer_4.Play();

            backgroundsplite.SetActive(true);
            Invoke("backgroundActive", 1);
            level = 4;
        }
    }
    void backgroundActive()
    {
        backgroundObject.SetActive(true);
    }
    void TurnOffAllBGAnim()
    {
        cloud1.gameObject.SetActive(false);
        cloud2.gameObject.SetActive(false);
        meteor1.gameObject.SetActive(false);
        meteor2.gameObject.SetActive(false);
        star1_2.gameObject.SetActive(false);
        starRing.gameObject.SetActive(false);
        star2_2.gameObject.SetActive(false);
        star3_2.gameObject.SetActive(false);
        star4_2.gameObject.SetActive(false);
        wave_2.gameObject.SetActive(false);
        planet_2.gameObject.SetActive(false);
        star1_3.gameObject.SetActive(false);
        star2_3.gameObject.SetActive(false);
        star3_3.gameObject.SetActive(false);
        star4_3.gameObject.SetActive(false);
        star5_3.gameObject.SetActive(false);
        wave1_3.gameObject.SetActive(false);
        wave2_3.gameObject.SetActive(false);
        wave3_3.gameObject.SetActive(false);
        planet_3.gameObject.SetActive(false);
        planet_light_3.gameObject.SetActive(false);
        stars_4.gameObject.SetActive(false);
        bigstar_4.gameObject.SetActive(false);
        midstar_4.gameObject.SetActive(false);
        smallstar_4.gameObject.SetActive(false);
        videoPlayer_4.Stop();
        backgroundObject.SetActive(false);
        backgroundsplite.SetActive(false);
    }
}
