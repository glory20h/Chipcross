using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BGAnimate : MonoBehaviour
{
    int level;

    // Transform components
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

    // SpriteRenderer components
    private SpriteRenderer star2_2SpriteRenderer;
    private SpriteRenderer star3_2SpriteRenderer;
    private SpriteRenderer star4_2SpriteRenderer;

    private SpriteRenderer star1_3SpriteRenderer;
    private SpriteRenderer star2_3SpriteRenderer;
    private SpriteRenderer star3_3SpriteRenderer;
    private SpriteRenderer star4_3SpriteRenderer;
    private SpriteRenderer star5_3SpriteRenderer;
    private SpriteRenderer planet_light_3SpriteRenderer;

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

    // Vector3 variables
    private Vector3 star1ScaleChange;
    private Vector3 star1RingScaleChange;

    void Start()
    {
        // Store references to the SpriteRenderer components
        star2_2SpriteRenderer = star2_2.GetComponent<SpriteRenderer>();
        star3_2SpriteRenderer = star3_2.GetComponent<SpriteRenderer>();
        star4_2SpriteRenderer = star4_2.GetComponent<SpriteRenderer>();

        star1_3SpriteRenderer = star1_3.GetComponent<SpriteRenderer>();
        star2_3SpriteRenderer = star2_3.GetComponent<SpriteRenderer>();
        star3_3SpriteRenderer = star3_3.GetComponent<SpriteRenderer>();
        star4_3SpriteRenderer = star4_3.GetComponent<SpriteRenderer>();
        star5_3SpriteRenderer = star5_3.GetComponent<SpriteRenderer>();
        planet_light_3SpriteRenderer = planet_light_3.GetComponent<SpriteRenderer>();

        // Initialize Vector3 variables
        star1ScaleChange = new Vector3(star1_2_Scale, star1_2_Scale, 0);
        star1RingScaleChange = new Vector3(star1_2_RingScale, star1_2_RingScale, 0);

        // Set initial alpha values for SpriteRenderer components
        star2_2SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star3_2SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star4_2SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));

        star1_3SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star2_3SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star3_3SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star4_3SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        star5_3SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
        planet_light_3SpriteRenderer.color = new Color(1, 1, 1, Random.Range(0f, 1f));
    }

    void Update()
    {
        // Level 1 animations
        cloud1.position += new Vector3(cloud1Speed * Time.deltaTime, 0, 0);
        cloud2.position += new Vector3(cloud2Speed * Time.deltaTime, 0, 0);
        meteor1.position += new Vector3(-meteor1Speed * Time.deltaTime, 0, 0);
        meteor2.position += new Vector3(-meteor2Speed * Time.deltaTime, 0, 0);

        // Level 2 animations
        float scaleFactor = Mathf.Sin(Time.time * 0.5f)*500;
        star1_2.localScale = star1ScaleChange * scaleFactor;
        starRing.localScale = star1RingScaleChange * scaleFactor;
        star2_2SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star2_2SpriteRenderer.color.a + star2_2_AlphaChange * Time.deltaTime));
        star3_2SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star3_2SpriteRenderer.color.a + star3_2_AlphaChange * Time.deltaTime));
        star4_2SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star4_2SpriteRenderer.color.a + star4_2_AlphaChange * Time.deltaTime));
        wave_2.position += new Vector3(0, -wave2Speed * Time.deltaTime, 0);

        // Level 3 animations
        star1_3SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star1_3SpriteRenderer.color.a + star1_3_AlphaChange * Time.deltaTime));
        star2_3SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star2_3SpriteRenderer.color.a + star2_3_AlphaChange * Time.deltaTime));
        star3_3SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star3_3SpriteRenderer.color.a + star3_3_AlphaChange * Time.deltaTime));
        star4_3SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star4_3SpriteRenderer.color.a + star4_3_AlphaChange * Time.deltaTime));
        star5_3SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(star5_3SpriteRenderer.color.a + star5_3_AlphaChange * Time.deltaTime));
        planet_light_3SpriteRenderer.color = new Color(1, 1, 1, Mathf.Clamp01(planet_light_3SpriteRenderer.color.a + plantlight_3_AlphaChange * Time.deltaTime));
        wave1_3.position += new Vector3(0, -wave1_3_Speed * Time.deltaTime, 0);
        wave2_3.position += new Vector3(0, -wave2_3_Speed * Time.deltaTime, 0);
        wave3_3.position += new Vector3(0, -wave3_3_Speed * Time.deltaTime, 0);

        // Level 4 animations
        stars_4.Rotate(0, 0, starsSpeed);
        bigstar_4.Rotate(0, 0, -starsSpeed * 2);
        midstar_4.Rotate(0, 0, -starsSpeed * 1.5f);
        smallstar_4.Rotate(0, 0, -starsSpeed * 1.25f);
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
