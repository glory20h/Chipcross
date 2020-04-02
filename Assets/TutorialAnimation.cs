using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnimation : MonoBehaviour
{
    public GameObject west;
    public GameObject north;
    public GameObject south;
    public GameObject east;
    public GameObject westhide;
    public GameObject easthide;
    public GameObject Center;

    public Transform target;
    Vector3 targetPosition;
    float speed = 1.0f;
    bool goBack = false;

    bool tutorial = false;
    int piece = 1;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (PlayerPrefs.GetInt("tutorial") == 1)//튜토리얼 조건 일경우 원래0임 확인하려고 1로만든거
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            //westhide.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile" + PlayerPrefs.GetInt("Piecedata")); 
            westhide = Resources.Load("Prefabs/Theme1Tile" + 2) as GameObject;
            if (goBack)
            {
                transform.position = targetPosition;
                goBack = false;
            }
        }
        else if(PlayerPrefs.GetInt("tutorial") == 1 && PlayerPrefs.GetInt("Piecedata") == 1)// 튜토리얼이 아니라면 2~5번 타일인지
        {
            switch(piece)
            {
                case 1:
                    westhide = Resources.Load("Prefabs/Theme1Tile" + 2) as GameObject;
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
        else if(PlayerPrefs.GetInt("tutorial") == 1 && PlayerPrefs.GetInt("Piecedata") == 2)// 6~7타일인지
        {
            switch (piece)
            {
                case 1:
                    break;
                case 2:
                    break;
            }
        }
        else// 8,9 타일인지
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "position")
        {
            goBack = true;
        }
    }
}
