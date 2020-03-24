using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimation : MonoBehaviour
{
    Vector3 targetPosition;
    float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(75, 75, 0);
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
