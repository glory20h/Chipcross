using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratingsystem : MonoBehaviour
{
    //[HideInInspector]
    public float time = 0f;
    //[HideInInspector]
    public bool timeStop = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!timeStop)
            time += Time.deltaTime;
    }

    public void StopTime()
    {
        bool timeStop = true;
    }
}
