﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBoy : MonoBehaviour
{
    TutorialAnimation Tutorialscript;
    public float speed;
    public float distanceBetweenTiles;

    public Vector3 initTargetPosition;
    Vector3 targetPosition; // 이거 중요함....
    Vector3 boiInitPos;

    public bool isMoving;
    bool isThereNextTile;
    bool metGirl;

    float fastForwardFactor;
    float flickForce;
    IEnumerator addFriction;

    char tileType;
    int xdir;
    int ydir;
    int temp;

    //Warp Tile용 변수
    bool warp = false;
    bool warpDone = false;

    void Start()
    {
        isMoving = false;           //..왜 주석 처리 했었더라???
        metGirl = false;
        addFriction = AddFriction();
        MoveDaBoi();
    }

    void Update()
    {
        if (isMoving)
        {
            //targetPosition(목표 타일의 중심)을 향해서 이동...
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
                else                    //Finish moving; Reached Girl? Or try try again??
                {
                    isMoving = false;
                    StopCoroutine(addFriction);

                    if (metGirl) //Correct Solution
                    {
                        metGirl = false;
                    }
                    else //Wrong Solution
                    {
                        SoundFXPlayer.Play("fail");
                        StartCoroutine(DelayBoyFail(1.5f));
                        Debug.Log("Try try again!");
                    }
                }
            }
        }
    }

    public void MoveDaBoi()
    {
        boiInitPos = transform.position;
        fastForwardFactor = 1f;
        xdir = 1;
        ydir = 0;
        targetPosition = initTargetPosition;
        GetComponent<BoxCollider2D>().enabled = true;
        isMoving = true;
        StartCoroutine(addFriction);
        flickForce = 2f;
    }

    public void FastForward()
    {
        fastForwardFactor = 3f;
    }

    public void BackToNormalSpeed()
    {
        fastForwardFactor = 1f;
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
                        warp = true;
                        break;
                    case '9':
                        warp = true;
                        break;
                }
            }

            /*if (warpDone && (tileType == '8' || tileType == '9'))
            {
                tileType = '1';
                warpDone = false;
            }*/

        }
        else if (collision.gameObject.name == "Girl")
        {
            xdir = 0;
            ydir = 0;
            metGirl = true;
        }
    }

    IEnumerator AddFriction()
    {
        while (true)
        {
            if (flickForce >= 1f)
            {
                flickForce -= Time.deltaTime * 1.2f;
            }
            yield return null;
        }
    }

    IEnumerator DelayBoyFail(float time)
    {
        yield return new WaitForSeconds(time);
        transform.position = boiInitPos;
    }

    public void DevBtn()
    {
        float force = 0.5f;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * force, ForceMode2D.Impulse);
    }
}

