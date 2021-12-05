using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnimate : MonoBehaviour
{
    public Transform cloud1;
    public Transform cloud2;
    public Transform star1;
    public Transform star2;
    public Transform star_ring;
    public Transform stars;
    float speed1;
    float speed2;
    float speed3;
    Vector3 scaleChange1;

    int level;

    void Start()
    {
        speed1 = 0.2f;
        speed2 = 0.16f;
        speed3 = 0.04f;
        scaleChange1 = new Vector3(-0.005f, -0.005f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (level == 1)
        {
            cloud1.Translate(Vector3.left * Time.deltaTime * speed1);
            cloud2.Translate(Vector3.left * Time.deltaTime * speed2);
            if (cloud1.position.x < -12)
            {
                cloud1.position = new Vector3(12.5f, cloud1.position.y, cloud1.position.z); ;
            }
            if (cloud2.position.x < -11)
            {
                cloud2.position = new Vector3(11.5f, cloud2.position.y, cloud2.position.z); ;
            }
        }
        if (level == 2)
        {
            star1.localScale += scaleChange1;
            if (star1.transform.localScale.y < 0.9f || star1.transform.localScale.y > 1.1f)
            {
                scaleChange1 = -scaleChange1;
            }
            // Animate Star & Star ring
        }
        if (level == 4)
        {
            stars.Translate(new Vector3(1, -1) * Time.deltaTime * speed3);
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
        star_ring.gameObject.SetActive(false);
        stars.gameObject.SetActive(false);
    }
}
