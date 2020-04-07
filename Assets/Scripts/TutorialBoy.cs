using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBoy : MonoBehaviour
{
    bool metGirl = false;
    Vector3 targetPosition;
    bool isThereNextTile;
    bool warp = false;
    bool warpDone = false;
    char tileType;
    int xdir;
    int ydir;
    int temp;
    float flickForce;
    float distanceBetweenTiles =0;
    float speed = 1.9f;
    float fastForwardFactor = 1f;
    public Vector3 initTargetPosition;
    bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        metGirl = false;
        MoveDaBoi();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * fastForwardFactor * flickForce * Time.deltaTime);
            //목표 지점에 도착
            if (transform.position == targetPosition)
            {
                if (isThereNextTile)     //파랭이가 아직 타일 위에 있는가?
                {
                    if (warp)            //Warp 해야 하는가?
                    {
                        if (tileType == '8')
                        {
                            if (GameObject.Find("Tile9(Clone)"))        //반대편 Warp 출구가 FixedTile 인지 Tile 인지 확인
                            {
                                gameObject.transform.position = GameObject.Find("Tile9(Clone)").transform.position;
                            }
                            else
                            {
                                gameObject.transform.position = GameObject.Find("FixedTile9(Clone)").transform.position;
                            }
                        }
                        else if (tileType == '9')
                        {
                            if (GameObject.Find("Tile8(Clone)"))       //반대편 Warp 출구가 FixedTile 인지 Tile 인지 확인
                            {
                                gameObject.transform.position = GameObject.Find("Tile8(Clone)").transform.position;
                            }
                            else
                            {
                                gameObject.transform.position = GameObject.Find("FixedTile8(Clone)").transform.position;
                            }
                        }
                        warp = false;
                        warpDone = true;
                    }

                    //사운드 FX 재생
                    if (tileType != '1')
                    {
                        SoundFXPlayer.Play("flick");
                        flickForce = 2f;
                    }

                    targetPosition = transform.position + new Vector3(xdir * distanceBetweenTiles, ydir * distanceBetweenTiles, 0);
                    isThereNextTile = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            if (warpDone)           //워프에서 반대편으로 나왔을때 바로 타일 탐지하는것 방지
            {
                warpDone = false;
            }
            else
            {
                isThereNextTile = true;
                tileType = collision.gameObject.name[4];

                switch (tileType)
                {
                    case '1':
                        break;
                    case '2':
                        xdir = -1;
                        ydir = 0;
                        break;
                    case '3':
                        xdir = 1;
                        ydir = 0;
                        break;
                    case '4':
                        xdir = 0;
                        ydir = -1;
                        break;
                    case '5':
                        xdir = 0;
                        ydir = 1;
                        break;
                    case '6':
                        temp = xdir;
                        xdir = -ydir;
                        ydir = temp;
                        break;
                    case '7':
                        temp = xdir;
                        xdir = ydir;
                        ydir = -temp;
                        break;
                    case '8':
                        /*
                        gameObject.transform.position = GameObject.Find("Tile9(Clone)").transform.position;
                        targetPosition = GameObject.Find("Tile9(Clone)").transform.position;
                        warpDone = true;
                        */
                        warp = true;
                        break;
                    case '9':
                        /*
                        gameObject.transform.position = GameObject.Find("Tile8(Clone)").transform.position;
                        targetPosition = GameObject.Find("Tile8(Clone)").transform.position;
                        warpDone = true;
                        */
                        warp = true;
                        break;
                }
            }
        }
        else if (collision.gameObject.name == "Girl")
        {
            xdir = 0;
            ydir = 0;
            metGirl = true;
        }
    }

    void MoveDaBoi()
    {
        isMoving = true;
        fastForwardFactor = 1f;
        xdir = 1;
        ydir = 0;
        targetPosition = initTargetPosition;
        flickForce = 2f;
    }
}

