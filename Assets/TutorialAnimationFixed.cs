using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnimationFixed : MonoBehaviour
{
    bool goBack = false;

    bool Noprefabyet = true;
    int piece = 1;
    public GameObject Westleft;
    public GameObject Westright;
    public GameObject Eastleft;
    public GameObject Eastright;
    public GameObject North;
    public GameObject South;
    public GameObject Center;
    public GameObject finger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("tutorial") == 0/*&& PlayerPrefs.GetInt("Piecedata") == 1*/)// 튜토리얼이 아니라면 2~5번 타일인지 "Piecedata"이게 판단함 타일판단은
        {
            if (Noprefabyet)// 한번만 생성하면 되기때문에
            {
                switch (piece)// 이건 다른 스크립트의 콜라이더를 사용할거기때문에  playerprefabs나 public사용해야됨
                {
                    case 1:// -> 이거 타일
                        Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                        Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                        North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile3");
                        finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                        Noprefabyet = false;
                        break;
                    case 2:// 위쪽 향하는 타일 ㅗ
                        Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                        Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                        North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile5");
                        finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                        Noprefabyet = false;
                        break;
                    case 3:// <- 이 타일
                        Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                        North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                        South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile2");
                        finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                        Noprefabyet = false;
                        break;
                    case 4:// 아래쪽 ㅜ 이방향
                        Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                        North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                        Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile4");
                        finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                        Noprefabyet = false;
                        break;
                }
            }
        }
        else if (PlayerPrefs.GetInt("tutorial") == 1 && PlayerPrefs.GetInt("Piecedata") == 2)// 6~7타일인지
        {
            if (Noprefabyet)// 한번만 생성하면 되기때문에
            {
                switch (piece)
                {
                    case 1: //Tile6 즉 뱅뱅이면 위로
                        Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                        Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                        South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile6");
                        finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                        Noprefabyet = false;
                        break;
                    case 2: //Tile7 즉 뱅뱅이면 아래
                        Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                        Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                        Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile7");
                        finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                        Noprefabyet = false;
                        break;
                }
            }
        }
        else// 8,9 타일인지
        {
            if (Noprefabyet)
            {
                Westleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Boy");
                Westright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile8");
                Eastleft.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Theme1Tile9");
                Eastright.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                North.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                South.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Girl");
                Center.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");
                finger.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/Nothing");

                Noprefabyet = false;
            }
        }
    }
}
