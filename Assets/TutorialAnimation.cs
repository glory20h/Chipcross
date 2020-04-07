using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnimation : MonoBehaviour
{
    TutorialBoy TutorialBoyMove;
    public Transform TutorialPanel;
    public Transform target;
    Vector3 targetPosition;
    float speed = 1.0f;
    bool goBack = false;

    bool Noprefabyet = true;
    int piece = 1;
    GameObject prefab;
    GameObject prefab1;
    GameObject prefab2;
    GameObject prefab3;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()//여기는 움직이는 것만 하면 될거 같다. 그러면 image랑 sprite 생성은 어디서 할까? 여기서 하기에는 부담이 있다. 하지만 여기서 안해도 부담이 있다! 답은 노가다군...
    {
        float step = speed * Time.deltaTime;
        if (PlayerPrefs.GetInt("tutorial") == 1)//튜토리얼 조건 일경우 원래0임 확인하려고 1로만든거
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            if (Noprefabyet)
            {
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Arts/finger");
                prefab = Resources.Load("Prefabs/Tile" + 1) as GameObject; 
                prefab = Instantiate(prefab, new Vector3(-200f, 0, 0), Quaternion.identity);
                prefab.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                prefab.transform.SetParent(TutorialPanel, false);

                prefab1 = Resources.Load("Prefabs/Tile" + 3) as GameObject; 
                prefab1 = Instantiate(prefab1, new Vector3(-100f, 0, 0), Quaternion.identity);
                prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                prefab1.transform.SetParent(TutorialPanel, false);

                prefab2 = Resources.Load("Prefabs/EmptyTile") as GameObject; 
                prefab2 = Instantiate(prefab2, new Vector3(100f, 0, 0), Quaternion.identity);
                prefab2.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                prefab2.transform.SetParent(TutorialPanel, false);

                prefab3 = Resources.Load("Prefabs/EmptyTile") as GameObject; 
                prefab3 = Instantiate(prefab3, new Vector3(200f, 0, 0), Quaternion.identity);
                prefab3.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                prefab3.transform.SetParent(TutorialPanel, false);

                Noprefabyet = false;
            }
            if (goBack)
            {
                transform.position = targetPosition;
                goBack = false;
            }
        }
        else if(PlayerPrefs.GetInt("tutorial") == 0 /*&& PlayerPrefs.GetInt("Piecedata") == 1*/)// 튜토리얼이 아니라면 2~5번 타일인지 "Piecedata"이게 판단함 타일판단은
        {
            if (Noprefabyet)// 한번만 생성하면 되기때문에
            {
                switch (piece)// 이건 다른 스크립트의 콜라이더를 사용할거기때문에  playerprefabs나 public사용해야됨
                {
                    case 1:// -> 이거 타일
                        prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; // boy
                        prefab = Instantiate(prefab, new Vector3(-250f, 0, 0), Quaternion.identity);
                        prefab.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab.transform.SetParent(TutorialPanel, false);

                        prefab1 = Resources.Load("Prefabs/Tile" + 3) as GameObject; // 가운데 tile
                        prefab1 = Instantiate(prefab1, new Vector3(0f, 0, 0), Quaternion.identity);
                        prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                        prefab1.transform.SetParent(TutorialPanel, false);

                        prefab2 = Resources.Load("Prefabs/TutorialGirl") as GameObject; // 목표점(Gril로 하자)
                        prefab2 = Instantiate(prefab2, new Vector3(250f, 0, 0), Quaternion.identity);
                        prefab2.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab2.transform.SetParent(TutorialPanel, false);

                        Noprefabyet = false;
                        break;
                    case 2:// 위쪽 향하는 타일 ㅗ
                        prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; // boy
                        prefab = Instantiate(prefab, new Vector3(-250f, 0, 0), Quaternion.identity);
                        prefab.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab.transform.SetParent(TutorialPanel, false);

                        prefab1 = Resources.Load("Prefabs/Tile" + 5) as GameObject; // 가운데 tile
                        prefab1 = Instantiate(prefab1, new Vector3(0f, 0, 0), Quaternion.identity);
                        prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                        prefab1.transform.SetParent(TutorialPanel, false);

                        prefab2 = Resources.Load("Prefabs/TutorialGirl") as GameObject; // 목표점(Gril로 하자)
                        prefab2 = Instantiate(prefab2, new Vector3(0, 200f, 0), Quaternion.identity);
                        prefab2.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab2.transform.SetParent(TutorialPanel, false);

                        Noprefabyet = false;
                        break;
                    case 3:// <- 이 타일
                        prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; // boy
                        prefab = Instantiate(prefab, new Vector3(250f, 0, 0), Quaternion.identity);
                        prefab.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab.transform.SetParent(TutorialPanel, false);

                        prefab1 = Resources.Load("Prefabs/Tile" + 2) as GameObject; // 가운데 tile
                        prefab1 = Instantiate(prefab1, new Vector3(0f, 0, 0), Quaternion.identity);
                        prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                        prefab1.transform.SetParent(TutorialPanel, false);

                        prefab2 = Resources.Load("Prefabs/TutorialGirl") as GameObject; // 목표점(Gril로 하자)
                        prefab2 = Instantiate(prefab2, new Vector3(-250f, 0, 0), Quaternion.identity);
                        prefab2.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab2.transform.SetParent(TutorialPanel, false);

                        Noprefabyet = false;
                        break;
                    case 4:// 아래쪽 ㅜ 이방향
                        prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; // boy
                        prefab = Instantiate(prefab, new Vector3(250f, 0, 0), Quaternion.identity);
                        prefab.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab.transform.SetParent(TutorialPanel, false);

                        prefab1 = Resources.Load("Prefabs/Tile" + 4) as GameObject; // 가운데 tile
                        prefab1 = Instantiate(prefab1, new Vector3(0f, 0, 0), Quaternion.identity);
                        prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                        prefab1.transform.SetParent(TutorialPanel, false);

                        prefab2 = Resources.Load("Prefabs/TutorialGirl") as GameObject; // 목표점(Gril로 하자)
                        prefab2 = Instantiate(prefab2, new Vector3(0, -200f, 0), Quaternion.identity);
                        prefab2.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab2.transform.SetParent(TutorialPanel, false);

                        Noprefabyet = false;
                        break;
                }
            }

            //prefab.transform.position = Vector3.MoveTowards(transform.position, prefab1.transform.position, step);
        }
        else if(PlayerPrefs.GetInt("tutorial") == 1 && PlayerPrefs.GetInt("Piecedata") == 2)// 6~7타일인지
        {
            if (Noprefabyet)// 한번만 생성하면 되기때문에
            {
                switch (piece)
                {
                    case 1: //Tile6 즉 뱅뱅이면 위로
                        prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; // boy
                        prefab = Instantiate(prefab, new Vector3(-250f, 0, 0), Quaternion.identity);
                        prefab.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab.transform.SetParent(TutorialPanel, false);

                        prefab1 = Resources.Load("Prefabs/Tile" + 6) as GameObject; // 가운데 tile
                        prefab1 = Instantiate(prefab1, new Vector3(0f, 0, 0), Quaternion.identity);
                        prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                        prefab1.transform.SetParent(TutorialPanel, false);

                        prefab2 = Resources.Load("Prefabs/TutorialGirl") as GameObject; // 목표점(Gril로 하자)
                        prefab2 = Instantiate(prefab2, new Vector3(0, 200f, 0), Quaternion.identity);
                        prefab2.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab2.transform.SetParent(TutorialPanel, false);

                        Noprefabyet = false;
                        break;
                    case 2: //Tile7 즉 뱅뱅이면 아래
                        prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; // boy
                        prefab = Instantiate(prefab, new Vector3(-250f, 0, 0), Quaternion.identity);
                        prefab.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab.transform.SetParent(TutorialPanel, false);

                        prefab1 = Resources.Load("Prefabs/Tile" + 7) as GameObject; // 가운데 tile
                        prefab1 = Instantiate(prefab1, new Vector3(0f, 0, 0), Quaternion.identity);
                        prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                        prefab1.transform.SetParent(TutorialPanel, false);

                        prefab2 = Resources.Load("Prefabs/TutorialGirl") as GameObject; // 목표점(Gril로 하자)
                        prefab2 = Instantiate(prefab2, new Vector3(0, -200f, 0), Quaternion.identity);
                        prefab2.transform.localScale = new Vector3(20f, 20f, 1);
                        prefab2.transform.SetParent(TutorialPanel, false);

                        Noprefabyet = false;
                        break;
                }
            }
        }
        else// 8,9 타일인지
        {
            if (Noprefabyet)
            {
                prefab = Resources.Load("Prefabs/TutorialBoy") as GameObject; 
                prefab = Instantiate(prefab, new Vector3(-200f, 0, 0), Quaternion.identity);
                prefab.transform.localScale = new Vector3(20f, 20f, 1);
                prefab.transform.SetParent(TutorialPanel, false);

                prefab1 = Resources.Load("Prefabs/Tile" + 8) as GameObject; 
                prefab1 = Instantiate(prefab1, new Vector3(-100f, 0, 0), Quaternion.identity);
                prefab1.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                prefab1.transform.SetParent(TutorialPanel, false);

                prefab2 = Resources.Load("Prefabs/Tile" + 9) as GameObject; 
                prefab2 = Instantiate(prefab2, new Vector3(100f, 0, 0), Quaternion.identity);
                prefab2.transform.localScale = new Vector3(12.5f, 12.5f, 1);
                prefab2.transform.SetParent(TutorialPanel, false);

                prefab3 = Resources.Load("Prefabs/TutorialGirl") as GameObject; 
                prefab3 = Instantiate(prefab3, new Vector3(200f, 0, 0), Quaternion.identity);
                prefab3.transform.localScale = new Vector3(20f, 20f, 1);
                prefab3.transform.SetParent(TutorialPanel, false);

                Noprefabyet = false;
            }
        }
    }

    private void LateUpdate()//여기서 destroy를 행할거다 if문으로
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)// 이건 finger 결국 collider이용하려면 moveboi script를 가져와서 조금 개조해서 하면 된다.
    {
        if (collision.gameObject.name == "position")
        {
            goBack = true;
            //Destroy(west);
        }
    }
}
