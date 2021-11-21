using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnimate : MonoBehaviour
{
    public Transform cloud1;
    public Transform cloud2;
    public float speed1;
    public float speed2;

    void Start()
    {
        //speed1 = 0.04f;
        //speed2 = 0.032f;
        speed1 = 0.2f;
        speed2 = 0.16f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cloud1.Translate(Vector3.left * Time.deltaTime * speed1);
        cloud2.Translate(Vector3.left * Time.deltaTime * speed2);
        if(cloud1.position.x < -12)
        {
            cloud1.position = new Vector3(12.5f, cloud1.position.y, cloud1.position.z); ;
        }
        if (cloud2.position.x < -11)
        {
            cloud2.position = new Vector3(11.5f, cloud2.position.y, cloud2.position.z); ;
        }
    }
}
