using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBoi : MonoBehaviour
{
    /* 파랭이를 움직이게 만드는 Script */
    public Event eventChanger;

    public GameObject TileBoard;
    public Button GoToNextLevelBtn;
    public Button GoNFasterButton;
    public Button ResetButton;
    public Button HintButton;

    public GameObject PuzzleSolvedPanel;

    //public Animator boyAnimator;

    [HideInInspector] public float speed;
    [HideInInspector] public float distanceBetweenTiles;

    [HideInInspector] public Vector3 initTargetPosition;
    Vector3 targetPosition; // 이거 중요함....
    Vector3 boiInitPos;

    [HideInInspector] public bool isMoving;

    float fastForwardFactor;
    float flickForce;
    IEnumerator addFriction;

    int numOfSteps;

    private enum TileType
    {
        None = 0,
        Normal = 1,
        Left = 2,
        Right = 3,
        Up = 4,
        Down = 5,
        RotateClockwise = 6,
        RotateCounterClockwise = 7,
        WarpA = 8,
        WarpB = 9
    }

    private int xdir = 1;
    private int ydir = 0;
    private int temp = 0;
    private bool warpDone = false;
    private bool isThereNextTile = false;
    private bool metGirl = false;
    private bool warp = false;
    private TileType tileType = TileType.None;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        isMoving = false;
        metGirl = false;
        speed = 2f;
        warp = false;
        warpDone = false;
        addFriction = AddFriction();
        //boyAnimator = gameObject.GetComponent<Animator>()
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            BoyMove();
        }
    }

    private void BoyMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * fastForwardFactor * flickForce * Time.deltaTime);
        
        if (transform.position == targetPosition)
        {
            if (isThereNextTile)
            {
                if (warp)
                {
                    //boyAnimator.enabled = false;// -> Pause Animator
                    gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                    if (tileType == TileType.WarpA)
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
                    else if (tileType == TileType.WarpB)
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

                    StartCoroutine(Waitsecond(0.3f));

                    gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(eventChanger.SkinPrefix() + "Boy");

                    warp = false;
                    warpDone = true;
                }

                //사운드 FX 재생
                if (tileType != TileType.Normal)
                {
                    SoundFXPlayer.Play("flick");
                    flickForce = 2.5f;
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
                    //Debug.Log("Puzzle solved!! Congrats!! :)");
                    StartCoroutine(PuzzleSolved());
                    GoNFasterButton.interactable = false;
                    ResetButton.interactable = false;
                    HintButton.interactable = false;
                    metGirl = false;
                }
                else //Wrong Solution
                {
                    SoundFXPlayer.Play("fail");
                    StartCoroutine(DelayBoyFail(1.5f));
                    //Debug.Log("Try try again!");
                }
            }

            numOfSteps++;
        }
    }

    public void MoveDaBoi()
    {
        boiInitPos = transform.position;
        isThereNextTile = true;
        fastForwardFactor = 0.5f;
        xdir = 1;
        ydir = 0;
        targetPosition = initTargetPosition;
        GetComponent<BoxCollider2D>().enabled = true;
        isMoving = true;
        warp = false;
        StartCoroutine(addFriction);
        flickForce = 2.5f;
        numOfSteps = 0;
    }

    public void FastForward()
    {
        fastForwardFactor = 2f;
    }

    public void BackToNormalSpeed()
    {
        fastForwardFactor = 0.5f;
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
            HandleTileCollision(collision, 4);
        }
        else if (collision.tag == "FixedTile")
        {
            HandleTileCollision(collision, 9);
        }
        else if (collision.gameObject.name == "Girl")
        {
            xdir = 0;
            ydir = 0;
            metGirl = true;
        }
    }

    private void HandleTileCollision(Collider2D collision, int nameIndex)
    {
        if (warpDone)
        {
            warpDone = false;
        }
        else
        {
            isThereNextTile = true;
            tileType = (TileType)int.Parse(collision.gameObject.name[nameIndex].ToString());

            switch (tileType)
            {
                case TileType.Normal:
                    break;
                case TileType.Left:
                    xdir = -1;
                    ydir = 0;
                    break;
                case TileType.Right:
                    xdir = 1;
                    ydir = 0;
                    break;
                case TileType.Up:
                    xdir = 0;
                    ydir = -1;
                    break;
                case TileType.Down:
                    xdir = 0;
                    ydir = 1;
                    break;
                case TileType.RotateClockwise:
                    temp = xdir;
                    xdir = -ydir;
                    ydir = temp;
                    break;
                case TileType.RotateCounterClockwise:
                    temp = xdir;
                    xdir = ydir;
                    ydir = -temp;
                    break;
                case TileType.WarpA:
                case TileType.WarpB:
                    warp = true;
                    break;
                default:
                    Debug.LogWarning("Invalid tileType");
                    break;
            }
        }
    }


    IEnumerator AddFriction()
    {
        while(true)
        {
            if(flickForce >= 1.5f)
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

    //퍼즐 완료시(When Boi successfully met girl)
    IEnumerator PuzzleSolved()
    {
        /* This part related to CoinAnimation -> Disabled for now
        if (eventChanger.coinChangeToggle == false)
        {
            StopCoroutine(eventChanger.CoinIncreaseAnimation());
        }
        */
        yield return new WaitForSeconds(0.6f);

        eventChanger.DisplayPlayData();
        eventChanger.DisplayTime();
        eventChanger.ChangeRating(numOfSteps - 1);

        if (eventChanger.isTutorial)
        {
            PuzzleSolvedPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/clear_2");
        }

        PuzzleSolvedPanel.SetActive(true);
        eventChanger.finger1.SetActive(false);
        SoundFXPlayer.Play("positiveVibe");

        eventChanger.EarnCoins();
    }

    IEnumerator Waitsecond(float time)
    {
        speed = 0f;
        yield return new WaitForSeconds(time);
        //boyAnimator.enabled = true;
        speed = 2f;
    }
}
