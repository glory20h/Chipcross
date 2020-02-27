using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    public GameObject AudioManager;

    public Transform TileBoard;     //빈 타일 Parent
    public Transform BlockPieces;   //아직 타일 위에 안 놓아진 퍼즐 조각 Parent
    public Transform BlockOnBoard;  //타일위에 놓아진 퍼즐 조각 Parent
    public GameObject Boy;          //파란 불꽃
    public GameObject Girl;         //분홍 불꽃

    public Button ResetBtn;         //퍼즐 초기화 버튼
    public Button GonfasterBtn;     //출발/가속 버튼

    public GameObject OptionMenu; //옵션 panel
    public static bool GameIsPaused = false; // Game pause
    public Button OptionExit; // 옵션 나가기

    private AudioSource AudioSrc; //Audio Source
    private float AudioVolume = 1f;

    Vector2 mousePos;               //마우스의 2차원상 위치
    Transform objToFollowMouse;     //마우스를 따라 다닐 물체(퍼즐 조각)
    GameObject[] triggeredObjects; //Array stores info on EmptyTiles        //퍼즐 조각의 빈 타일 탐지용
    Vector3[] PieceInitPosition;   //Stores initial position of Pieces      //초기 퍼즐 조각 위치 저장

    float UIPieceScale = 0.4f;      //UI에서의 퍼즐 조각 크기. 화면/퍼즐에 놓았을 때는 1, UI상에서는 현재 값으로 축소

    int goNFastBtnState;            //1 -> Move Boy!, 2 -> Make Boy Faster!, 3 -> Make Boy back to normal speed     출발/가속 버튼용 변수

    //Variables for Level Loading
    LevelDatabase levelData;
    int levelNum;
    float scaleFactor;
    float distanceBetweenTiles;
    float emptyTileScale;
    float pieceScale;

    //boolean for Update Function //Update에 쓸 bool 변수
    public bool MovePieceMode;
    bool BlockPieceMoveLeft;
    bool BlockPieceMoveRight;

    //Temporary Variables (For Testing Purposes) 테스트용 변수들
    int hohoho; //개발자 버튼용 변수

    void Start()
    {
        //변수 초기화
        InitializeVariables();
        
        //levelData 게임 스테이지 데이터베이스에서 데이터를 불러와서 현재 스테이지 생성
        LoadLevel();

        //퍼즐 조각 초기 위치 저장
        SavePieceInitPosition();

        //개발자 버튼용
        hohoho = 1;
    }

    void InitializeVariables()
    {
        MovePieceMode = true;
        BlockPieceMoveLeft = false;
        BlockPieceMoveRight = false;

        goNFastBtnState = 1;
        GonfasterBtn.interactable = false;

        levelNum = 1;
        levelData = new LevelDatabase();

        AudioSrc = AudioManager.GetComponent<AudioSource>(); //오디오
    }

    void Update()
    {
        //퍼즐조각 움직임 enable/disable
        if (MovePieceMode && Time.timeScale != 0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //마우스 클릭시 충돌 물체 탐지
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {
                    //충돌 물체가 퍼즐 조각일 경우
                    if (hit.transform.tag == "Tile")
                    {
                        if(GonfasterBtn.interactable)
                        {
                            GonfasterBtn.interactable = false;
                        }

                        objToFollowMouse = hit.transform.parent;
                        objToFollowMouse.localScale = new Vector3(pieceScale, pieceScale, 1);

                        //Enable EmptyTile Box Collider2D & Detectors
                        for (int i = 0; i < objToFollowMouse.childCount; i++)
                        {
                            objToFollowMouse.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 200;

                            //if Tile is on the board
                            if (objToFollowMouse.IsChildOf(BlockOnBoard))
                            {
                                objToFollowMouse.GetChild(i).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject.GetComponent<BoxCollider2D>().enabled = true;  //Disable Box Collider of EmptyTile
                                objToFollowMouse.GetChild(i).GetChild(0).GetComponent<BoxCollider2D>().enabled = true;  //Enable Detector Box Collider
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                //마우스로 퍼즐 조각 드래그
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (objToFollowMouse != null)
                {
                    objToFollowMouse.position = new Vector3(mousePos.x, mousePos.y, 0);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (objToFollowMouse != null)
                {
                    bool isPiecePlaceable = true;
                    triggeredObjects = new GameObject[objToFollowMouse.childCount];

                    //Check if isPiecePlaceable
                    for (int i = objToFollowMouse.childCount - 1; i >= 0; i--)
                    {
                        if (objToFollowMouse.GetChild(i).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject != null)
                        {
                            triggeredObjects[i] = objToFollowMouse.GetChild(i).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject;
                            if (triggeredObjects[i].tag != "EmptyTile")
                            {
                                isPiecePlaceable = false;
                            }
                            if (!triggeredObjects[i].GetComponent<BoxCollider2D>().bounds.Contains(objToFollowMouse.GetChild(i).position))       //If the bounds of an EmptyTile doesn't contain the center point of the tile
                            {
                                isPiecePlaceable = false;
                            }
                        }
                        else
                        {
                            isPiecePlaceable = false;
                        }
                    }

                    if (isPiecePlaceable)
                    {
                        //Place piece on the board
                        objToFollowMouse.position = triggeredObjects[0].transform.position - (objToFollowMouse.GetChild(0).localPosition * scaleFactor);
                        objToFollowMouse.SetParent(BlockOnBoard, true);

                        //Disable Corresponding EmptyTile BoxCollider2D & Detectors
                        for (int i = 0; i < objToFollowMouse.childCount; i++)
                        {
                            triggeredObjects[i].GetComponent<BoxCollider2D>().enabled = false;
                            objToFollowMouse.GetChild(i).GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
                            objToFollowMouse.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 10;
                        }

                        CheckIfAllTilesInPlace();
                    }
                    else
                    {
                        ResetPiecePosition(objToFollowMouse);
                    }
                    
                    objToFollowMouse = null;
                }
            }
        }

        //조각 왼쪽으로 이동 버튼 눌렀을때
        if(BlockPieceMoveLeft && Time.timeScale != 0f)
        {
            BlockPieces.transform.position = Vector3.MoveTowards(BlockPieces.transform.position, new Vector3(9 - (1.5f * levelData.NumberOfPieces), -3.75f, 0), 0.2f);
        }

        //조각 오른쪽으로 이동 버튼 눌렀을때
        if (BlockPieceMoveRight && Time.timeScale != 0f)
        {
            BlockPieces.transform.position = Vector3.MoveTowards(BlockPieces.transform.position, new Vector3(-(9 - (1.5f * levelData.NumberOfPieces)), -3.75f, 0), 0.2f);
        }
    }

    //게임 레벨 불러오기
    void LoadLevel()
    {
        Debug.Log("LoadLevel() Start");

        GameObject prefab;
        GameObject obj;
        GameObject obj2;

        levelData.LoadLevelData(levelNum);
        int typeIndex;
        int pieceHeight;
        int pieceWidth;

        //Reset Position of BlockPieces
        BlockPieces.transform.position = new Vector3(0, -3.75f, 0);

        //Add Scaling by scaleSize!
        scaleFactor = 1 - 0.2f * (levelData.scaleSize - 1);
        distanceBetweenTiles = 2 * scaleFactor;
        emptyTileScale = 0.25f * scaleFactor;
        pieceScale = 1 * scaleFactor;

        //Instantiate 'EmptyTile'
        typeIndex = 0;
        for (int i = 0; i < levelData.BoardHeight; i++)
        {
            for (int j = 0; j < levelData.BoardWidth; j++)
            {
                //Get prefab information from array                                                             //May Need for Optimization in the future
                if (levelData.BoardEmptyTileTypeInfo[typeIndex] == 1)
                {
                    prefab = Resources.Load("Prefabs/EmptyTile") as GameObject;
                }
                else if (levelData.BoardEmptyTileTypeInfo[typeIndex] == 0)
                {
                    prefab = Resources.Load("Prefabs/VoidTile") as GameObject;
                }
                else
                {
                    prefab = Resources.Load("Prefabs/FixedTile" + levelData.BoardEmptyTileTypeInfo[typeIndex].ToString()) as GameObject;
                }

                obj = Instantiate(prefab, new Vector3((-levelData.BoardWidth + 1) * (distanceBetweenTiles / 2f) + distanceBetweenTiles * j, (levelData.BoardHeight - 1) * (distanceBetweenTiles / 2f) - distanceBetweenTiles * i, 0), Quaternion.identity);
                obj.transform.localScale = new Vector3(emptyTileScale, emptyTileScale, 1);
                obj.transform.SetParent(TileBoard, false);

                //Set Boy & Girl Position
                if (i == levelData.BoyPos && j == 0)
                {
                    Boy.transform.position = obj.transform.position - new Vector3(distanceBetweenTiles, 0, 0);
                    Boy.GetComponent<MoveBoi>().initTargetPosition = obj.transform.position;
                    Boy.GetComponent<MoveBoi>().distanceBetweenTiles = distanceBetweenTiles;
                    Boy.transform.localScale = new Vector3(emptyTileScale, emptyTileScale, 1);
                }

                if(i == levelData.GirlPos && j == levelData.BoardWidth - 1)
                {
                    Girl.transform.position = obj.transform.position + new Vector3(distanceBetweenTiles, 0, 0);
                    Girl.transform.localScale = new Vector3(emptyTileScale, emptyTileScale, 1);
                }
                typeIndex++;
            }
        }

        //Instantiate 'Piece'
        /*for (int i = 0; i < levelData.NumberOfPieces; i++)
        {
            prefab = Resources.Load("Prefabs/Piece") as GameObject;
            obj = Instantiate(prefab, new Vector3(-3 * ((levelData.NumberOfPieces - 1) / 2f) + 3 * i, 0, 0), Quaternion.identity);           // 3 is the distance between pieces
            obj.transform.SetParent(BlockPieces, false);
            obj.GetComponent<VariableProvider>().pieceNum = i;

            typeIndex = 0;
            pieceHeight = levelData.pieceDatas[i].PieceHeight;
            pieceWidth = levelData.pieceDatas[i].PieceWidth;
            for(int j = 0; j < pieceHeight; j++)
            {
                for(int k = 0; k < pieceWidth; k++)
                {
                    if(levelData.pieceDatas[i].TileType[typeIndex] != 0)
                    {
                        prefab = Resources.Load("Prefabs/Tile" + levelData.pieceDatas[i].TileType[typeIndex].ToString()) as GameObject;
                        obj2 = Instantiate(prefab, new Vector3(-pieceWidth + 1 + 2 * k, pieceHeight - 1 - 2 * j, 0), Quaternion.identity);
                        obj2.transform.SetParent(obj.transform, false);
                    }
                    typeIndex++;
                }
            }
        }*/

        //Random Puzzle Piece Position Version
        //PieceInitPosition = new Vector3[levelData.NumberOfPieces];
        for (int i = 0; i < levelData.NumberOfPieces; i++)
        {
            prefab = Resources.Load("Prefabs/Piece") as GameObject;
            obj = Instantiate(prefab, new Vector3(Random.value < 0.5 ? Random.Range(-7.6f, -5.9f) : Random.Range(5.9f, 7.6f), Random.Range(0, 7.4f)), Quaternion.identity);           // 3 is the distance between pieces
            obj.transform.SetParent(BlockPieces, false);
            obj.GetComponent<VariableProvider>().pieceNum = i;

            typeIndex = 0;
            pieceHeight = levelData.pieceDatas[i].PieceHeight;
            pieceWidth = levelData.pieceDatas[i].PieceWidth;
            for (int j = 0; j < pieceHeight; j++)
            {
                for (int k = 0; k < pieceWidth; k++)
                {
                    if (levelData.pieceDatas[i].TileType[typeIndex] != 0)
                    {
                        prefab = Resources.Load("Prefabs/Tile" + levelData.pieceDatas[i].TileType[typeIndex].ToString()) as GameObject;
                        obj2 = Instantiate(prefab, new Vector3(-pieceWidth + 1 + 2 * k, pieceHeight - 1 - 2 * j, 0), Quaternion.identity);
                        obj2.transform.SetParent(obj.transform, false);
                        obj2.GetComponent<SpriteRenderer>().sortingOrder = 75 + i;
                    }
                    typeIndex++;
                }
            }
        }

        Debug.Log("LoadLevel() Finished");
    }

    void SavePieceInitPosition()
    {
        PieceInitPosition = new Vector3[BlockPieces.childCount];
        for (int i = 0; i < BlockPieces.childCount; i++)
        {
            PieceInitPosition[i] = BlockPieces.GetChild(i).localPosition;
        }
    }

    //퍼즐 조각들이 모두 타일위에 놓아졌는지 확인
    void CheckIfAllTilesInPlace()
    {
        if(PieceInitPosition.Length == BlockOnBoard.childCount)
        {
            GonfasterBtn.interactable = true;
        }
        else
        {
            GonfasterBtn.interactable = false;
        }
    }

    //퍼즐 조각 하나의 위치 초기화
    void ResetPiecePosition(Transform piece)     //For some reason this works!
    {
        piece.SetParent(BlockPieces);
        piece.localPosition = PieceInitPosition[piece.GetComponent<VariableProvider>().pieceNum];
        piece.localScale = new Vector3(UIPieceScale, UIPieceScale, 1);
        for (int i = 0; i < piece.childCount; i++)
        {
            piece.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 75 + piece.GetComponent<VariableProvider>().pieceNum;
        }
    }

    //현재 스테이지 요소들 삭제
    void DeleteLevel()
    {
        Debug.Log("DeleteLevel() Start");
        foreach (Transform child in TileBoard)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in BlockPieces)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in BlockOnBoard)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("DeleteLevel() Finished");
    }

    //출발/가속 버튼 State 1 -> 누르면 이동 시작, 2 -> 누르면 빨라짐, 3 -> 누르면 다시 원래 속도로 돌아옴
    public void GoNFastForwardClick()
    {
        if (goNFastBtnState == 1)
        {
            Boy.GetComponent<MoveBoi>().MoveDaBoi();
            MovePieceMode = false;
            goNFastBtnState = 2;
            GonfasterBtn.image.sprite = Resources.Load<Sprite>("Arts/FastForward");
        }
        else if(goNFastBtnState == 2)
        {
            //Boy FastForward
            Boy.GetComponent<MoveBoi>().FastForward();
            goNFastBtnState = 3;
            GonfasterBtn.image.sprite = Resources.Load<Sprite>("Arts/NormalSpeed");
        }
        else
        {
            //Boy Back to normal speed
            Boy.GetComponent<MoveBoi>().BackToNormalSpeed();
            goNFastBtnState = 2;
            GonfasterBtn.image.sprite = Resources.Load<Sprite>("Arts/FastForward");
        }
    }

    //출발/가속 버튼 State 초기화
    public void ResetGoNFasterButton()
    {
        goNFastBtnState = 1;
        GonfasterBtn.image.sprite = Resources.Load<Sprite>("Arts/Goooo");
    }

    //초기화 버튼 눌렀을때
    public void ResetLevelClick()
    {
        if(Boy.GetComponent<MoveBoi>().isMoving) //During Boy Moving Phase
        {
            //Reset the boy moving
            Boy.GetComponent<MoveBoi>().ResetBoyMove();
            ResetGoNFasterButton();
        }
        else //During Puzzle Solving Phase
        {
            //Reset Position of BlockPieces
            BlockPieces.transform.position = new Vector3(0, -3.75f, 0);

            Transform objToReset;
            while (BlockOnBoard.childCount > 0)
            {
                objToReset = BlockOnBoard.GetChild(0);

                for (int i = 0; i < objToReset.childCount; i++)
                {
                    objToReset.GetChild(i).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject.GetComponent<BoxCollider2D>().enabled = true;
                    objToReset.GetChild(i).GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
                }

                ResetPiecePosition(BlockOnBoard.GetChild(0));
            }

            GonfasterBtn.interactable = false;
        }
    }

    //다음 스테이지 불러오기
    public void GoToNextLevel()
    {
        levelNum++;
        MovePieceMode = true;
        ResetBtn.interactable = true;
        DeleteLevel();
        LoadLevel();
        SavePieceInitPosition();
        /*Debug.Log("BlockPieces.childCount : " + BlockPieces.childCount);
        for (int i = 0; i < PieceInitPosition.Length; i++)
        {
            Debug.Log(" i = " + i + " : " + PieceInitPosition[i]);
        }*/
    }

    //테스트용 개발자 버튼용
    public void DevBtnAct()  //Go To Level X
    {
        levelNum++;
        DeleteLevel();
        LoadLevel();
        Debug.Log("BlockPieces.childCount : " + BlockPieces.childCount);
        SavePieceInitPosition();
        for(int i = 0; i < PieceInitPosition.Length; i++)
        {
            Debug.Log(" i = " + i + " : " + PieceInitPosition[i]);
        }
        Debug.Log("BlockPieces.childCount : " + BlockPieces.childCount);

        /*
        //Change to next Level
        if(hohoho == 1)
        {
            DeleteLevel();
            //Debug.Log("BlockPieces.childCount : " + BlockPieces.childCount);
            hohoho++;
        }
        else
        {
            levelNum++;
            LoadLevel();
            SavePieceInitPosition();
            //Debug.Log("BlockPieces.childCount : " + BlockPieces.childCount);
            hohoho = 1;
        }
        */
    }

    public void OnMoveLeftBtnDown(bool set)
    {
        BlockPieceMoveLeft = set;
    }

    public void OnMoveRightBtnDown(bool set)
    {
        BlockPieceMoveRight = set;
    }

    public void OpenOptionPanel()
    {
        OptionMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseOptionPanel()
    {
        OptionMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetVolume(float vol)
    {
        AudioVolume = vol;
        AudioSrc.volume = AudioVolume;
    }

    public void Hintsystem()
    {
        Debug.Log("Hi");
        
    }
    // 생각하고 있는 힌트 방식은 우리가 밑에 있는 블럭중 하나를 클릭하게 되면 정답 레벨 데이터를 정한 부분을 해서 그것을 새롭게 레벨 데이터를 가져오는 방식으로 하는 것을 생각중임.
    // 해보니까 구성상으로 안됨 각타일의 것들이 각각있음
    // 이러면 힌트를 구성을 어떻게 해야되는 것인가
    // 쉬운것부터 우리가 강제로 하나씩 위치를 정해줘야되는것인가? 음....
}
