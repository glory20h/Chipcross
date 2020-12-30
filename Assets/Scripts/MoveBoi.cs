using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBoi : MonoBehaviour
{
    /* 파랭이를 움직이게 만드는 Script */

    public Event eventChanger;
    LevelDatabase level;

    public GameObject TileBoard;
    public Button GoToNextLevelBtn;
    public Button GoNFasterButton;
    public Button ResetButton;

    public GameObject PuzzleSolvedPanel;

    public float speed;
    [HideInInspector]
    public float distanceBetweenTiles;

    [HideInInspector]
    public Vector3 initTargetPosition;
    Vector3 targetPosition; // 이거 중요함....
    Vector3 boiInitPos;

    [HideInInspector]
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
    }

    void Update()
    {
        if (isMoving)
        {
            //targetPosition(목표 타일의 중심)을 향해서 이동...
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * fastForwardFactor * flickForce * Time.deltaTime);
            //목표 지점에 도착
            if(transform.position == targetPosition)
            {
                if(isThereNextTile)     //파랭이가 아직 타일 위에 있는가?
                {
                    if(warp)            //Warp 해야 하는가?
                    {
                        Sprite boySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                        gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Arts/Nothing", typeof(Sprite));

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

                        StartCoroutine(Waitsecond(boySprite));

                        warp = false;
                        warpDone = true;
                    }

                    //사운드 FX 재생
                    if (tileType != '1')
                    {
                        SoundFXPlayer.Play("flick");
                        flickForce = 2f;
                    }

                    //xdir, ydir로 targetPosition 갱신
                    targetPosition = transform.position + new Vector3(xdir * distanceBetweenTiles, ydir * distanceBetweenTiles, 0);
                    isThereNextTile = false;
                }
                else                    //Finish moving; Reached Girl? Or try try again??
                {
                    eventChanger.ResetGoNFaster();
                    isMoving = false;
                    StopCoroutine(addFriction);

                    if (metGirl) //Correct Solution
                    {
                        Debug.Log("Puzzle solved!! Congrats!! :)");
                        StartCoroutine(PuzzleSolved());
                        GoNFasterButton.interactable = false;
                        ResetButton.interactable = false;
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
        isThereNextTile = true;
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

    public void ResetBoyMove()
    {
        //Make the boy stop moving and return to its original position
        eventChanger.ResetGoNFaster();
        eventChanger.MovePieceMode = true;
        ResetBoyPosition();
        isMoving = false;
        StopCoroutine(addFriction);
    }

    public void ResetBoyPosition()
    {
        transform.position = boiInitPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile" || collision.tag == "Hint")
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

            /*if (warpDone && (tileType == '8' || tileType == '9'))
            {
                tileType = '1';
                warpDone = false;
            }*/

        }
        else if(collision.tag == "FixedTile")
        {
            if (warpDone)           //워프에서 반대편으로 나왔을때 바로 타일 탐지하는것 방지
            {
                warpDone = false;
            }
            else
            {
                isThereNextTile = true;
                tileType = collision.gameObject.name[9];

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
                        targetPosition = GameObject.Find("FixedTile9(Clone)").transform.position;
                        warpDone = true;
                        */
                        warp = true;
                        break;
                    case '9':
                        /*
                        gameObject.transform.position = GameObject.Find("Tile8(Clone)").transform.position;
                        targetPosition = GameObject.Find("FixedTile8(Clone)").transform.position;
                        warpDone = true;
                        */
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
        else if(collision.gameObject.name == "Girl")
        {
            xdir = 0;
            ydir = 0;
            metGirl = true;
        }
    }

    IEnumerator AddFriction()
    {
        while(true)
        {
            if(flickForce >= 1f)
            {
                flickForce -= Time.deltaTime * 1.2f;
            }
            yield return null;
        }
    }

    IEnumerator DelayBoyFail(float time)
    {
        yield return new WaitForSeconds(time);
        eventChanger.MovePieceMode = true;
        transform.position = boiInitPos;
        eventChanger.timeCount = true;
    }

    //퍼즐 완료시
    IEnumerator PuzzleSolved()
    {
        /* This part is related to CoinAnimation -> Disabled for now
        if (eventChanger.coinChangeToggle == false)
        {
            StopCoroutine(eventChanger.CoinIncreaseAnimation());
        }
        */
        yield return new WaitForSeconds(0.6f);
        /*
        eventChanger.UsingHint.text = "";
        eventChanger.UsingTouch.text = "";
        eventChanger.UsingRestart.text = "";
        eventChanger.UsingHint.text = "UsingHint:" + eventChanger.usingHint.ToString();
        eventChanger.UsingTouch.text = "UsingTouch:" + eventChanger.usingTouch.ToString();
        eventChanger.UsingRestart.text = "UsingRestart: " + eventChanger.usingRestart.ToString();
        */
        eventChanger.DisplayPlayData();
        eventChanger.DisplayTime();
        PuzzleSolvedPanel.SetActive(true);
        SoundFXPlayer.Play("positiveVibe");
        //yield return StartCoroutine(eventChanger.CoinIncreaseAnimation()); //This part is related to CoinAnimation -> Disabled for now
    }

    IEnumerator Waitsecond(Sprite boySprite)
    {
        speed = 0f;
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().sprite = boySprite;
        speed = 1.9f;
    }

    
    void Ratingsys()
    {
        float rate = 0f;
        if(level.BoardHeight * level.BoardWidth < 9)//Hint max: 0
        {
            rate = 0.05f * (-eventChanger.usingHint); //Hint 사용여부
            rate += 0.05f * (level.BoardHeight * level.BoardWidth * 30 - eventChanger.elapsedTime)/ level.BoardHeight * level.BoardWidth * 30;//0.5min
            rate += 0.01f*(level.BoardHeight * level.BoardWidth*0.9f - eventChanger.usingTouch + eventChanger.usingRestart);
        }
        else if(level.BoardHeight * level.BoardWidth < 15)//Hint max: 1
        {
            rate = 0.05f * (1 - eventChanger.usingHint); //Hint 사용여부
            rate += 0.05f * (level.BoardHeight * level.BoardWidth * 60 * 0.8f - eventChanger.elapsedTime)/ level.BoardHeight * level.BoardWidth * 60 * 0.8f;//1min * 0.8
            rate += 0.01f * (level.BoardHeight * level.BoardWidth * 2.5f - eventChanger.usingTouch + eventChanger.usingRestart);
        }
        else//Hint max: 2
        {
            rate = 0.05f * (2 - eventChanger.usingHint); //Hint 사용여부
            rate += 0.05f * (level.BoardHeight * level.BoardWidth * 60 - eventChanger.elapsedTime)/ level.BoardHeight * level.BoardWidth * 60;//1min
            rate += 0.01f * (level.BoardHeight * level.BoardWidth * 3f - eventChanger.usingTouch + eventChanger.usingRestart);
        }
        //rate = 0.06f * (eventChanger.usingHint * 12 - level.BoardHeight * level.BoardWidth);//3*4가 마지노선이니까 5*5는 최종보스니까 2개까지 쓰게하자고
        //rate += eventChanger.elapsedTime;
        Debug.Log(eventChanger.elapsedTime);
    }
    
}
