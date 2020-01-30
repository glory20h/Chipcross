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

    //텔레포트
    bool bJump = false;
    bool bJump2 = false;
    private GameObject objToTP;
    private Transform tpLoc;

    void Start()
    {
        isMoving = false;
        metGirl = false;
        GoToNextLevelBtn.interactable = false;
    }

    void Update()
    {
        if(isMoving)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            isThereNextTile = true;
            GameObject nextTile = collision.gameObject;
            int tileType = nextTile.name[4] - '0';
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
                    gameObject.transform.position = GameObject.Find("Tile9(Clone)").transform.position;
                    break;
                case 9:
                    gameObject.transform.position = GameObject.Find("Tile8(Clone)").transform.position;
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

    IEnumerator DelayBoyFail(float time)
    {
        yield return new WaitForSeconds(time);
        eventChanger.MovePieceMode = true;
        transform.position = boiInitPos;
    }

    /*
    void search()
    {
        tpLoc.transform.position = 
    }
    */
}
