using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBoi : MonoBehaviour
{
    /* 파랑이를 움직이게 만드는 Script */

    public Event eventChanger;

    public GameObject TileBoard;
    public Button GoToNextLevelBtn;
    public Button GoNFasterButton;
    public Button ResetButton;

    public float speed;
    public float distanceBetweenTiles;

    public Vector3 initTargetPosition;
    Vector3 targetPosition;
    Vector3 boiInitPos;

    public bool isMoving;
    bool isThereNextTile;
    bool metGirl;

    float fastForwardFactor;

    int xdir;
    int ydir;
    int temp;
    int beforexdir;
    int beforeydir;

    //텔레포트
    bool warpDone = false;

    void Start()
    {
        isMoving = false;
        metGirl = false;
        GoToNextLevelBtn.interactable = false;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * fastForwardFactor * Time.deltaTime);
            if(transform.position == targetPosition)
            {
                if(isThereNextTile)    //If the Boi is still on the tile
                {
                    targetPosition = targetPosition + new Vector3(xdir * distanceBetweenTiles, ydir * distanceBetweenTiles, 0);
                    isThereNextTile = false;
                }
                else                   //Finish moving; Reached Girl? Or try try again??
                {
                    eventChanger.ResetGoNFasterButton();
                    isMoving = false;
                    if (metGirl) //Correct Solution
                    {
                        Debug.Log("You met the girl!! Congrats!! :)");
                        GoToNextLevelBtn.interactable = true;
                        GoNFasterButton.interactable = false;
                        ResetButton.interactable = false;
                        metGirl = false;
                    }
                    else //Wrong Solution
                    {
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
    }

    public void FastForward()
    {
        fastForwardFactor = 5f;
    }

    public void BackToNormalSpeed()
    {
        fastForwardFactor = 1f;
    }

    public void ResetBoyMove()
    {
        //Make the boy stop moving and return to its original position
        eventChanger.ResetGoNFasterButton();
        eventChanger.MovePieceMode = true;
        transform.position = boiInitPos;
        isMoving = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            isThereNextTile = true;
            GameObject nextTile = collision.gameObject;
            int tileType = nextTile.name[4] - '0';          //현재 숫자 한자리수 까지밖에 지원안되는데 (9까지) 나중에 두자리수까지 지원되게 고쳐야 됨.
            switch (tileType)
            {
                case 1:
                    break;
                case 2:
                    xdir = -1;
                    ydir = 0;
                    break;
                case 3:
                    xdir = 1;
                    ydir = 0;
                    break;
                case 4:
                    xdir = 0;
                    ydir = -1;
                    break;
                case 5:
                    xdir = 0;
                    ydir = 1;
                    break;
                case 6:
                    temp = xdir;
                    xdir = -ydir;
                    ydir = temp;
                    break;
                case 7:
                    temp = xdir;
                    xdir = ydir;
                    ydir = -temp;
                    break;
                case 8:
                    if (warpDone == false)
                    {
                        beforexdir = xdir;
                        beforeydir = ydir;
                        xdir = 0;
                        ydir = 0;
                        isMoving = false;
                    }
                    break;
                case 9:
                    if(warpDone == false)
                    {
                        beforexdir = xdir;
                        beforeydir = ydir;
                        xdir = 0;
                        ydir = 0;
                        isMoving = false;
                    }
                    break;
            }
        }
        if(collision.tag == "FixedTile")
        {
            isThereNextTile = true;
            GameObject nextTile = collision.gameObject;
            int tileType = nextTile.name[9] - '0';
            switch(tileType)
            {
                case 2:
                    xdir = -1;
                    ydir = 0;
                    break;
                case 3:
                    xdir = 1;
                    ydir = 0;
                    break;
                case 4:
                    xdir = 0;
                    ydir = -1;
                    break;
                case 5:
                    xdir = 0;
                    ydir = 1;
                    break;
                case 6:
                    temp = xdir;
                    xdir = -ydir;
                    ydir = temp;
                    break;
                case 7:
                    temp = xdir;
                    xdir = ydir;
                    ydir = -temp;
                    break;
              //case 8:
              //case 9:
                    //private Transform target = nextTile.name[8];
                    //private Transform target2 = nextTile.name[9];
            }
        }
        if(collision.gameObject.name == "Girl")
        {
            metGirl = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            isThereNextTile = true;
            GameObject nextTile = collision.gameObject;
            int tileType = nextTile.name[4] - '0';          //현재 숫자 한자리수 까지밖에 지원안되는데 (9까지) 나중에 두자리수까지 지원되게 고쳐야 됨.
            switch (tileType)
            {
                case 8:
                    if (warpDone == false)
                    {
                        targetPosition = GameObject.Find("Tile9(Clone)").transform.position;
                        warpDone = true;
                        collision.enabled = false;
                        isMoving = true;
                        xdir = beforexdir;
                        ydir = beforeydir;
                    }
                    break;
                case 9:
                    if(warpDone == false)
                    {
                        targetPosition = GameObject.Find("Tile8(Clone)").transform.position;
                        warpDone = true;
                        collision.enabled = false;
                        isMoving = true;
                        xdir = beforexdir;
                        ydir = beforeydir;
                    }
                    break;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            isThereNextTile = true;
            GameObject nextTile = collision.gameObject;
            int tileType = nextTile.name[4] - '0';          //현재 숫자 한자리수 까지밖에 지원안되는데 (9까지) 나중에 두자리수까지 지원되게 고쳐야 됨.
            switch (tileType)
            {
                case 8:
                    collision.enabled = true;
                    break;
                case 9:
                    collision.enabled = true;
                    break;
            }
        }
    }

    IEnumerator DelayBoyFail(float time)
    {
        yield return new WaitForSeconds(time);
        eventChanger.MovePieceMode = true;
        transform.position = boiInitPos;
    }

}
