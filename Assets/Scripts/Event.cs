using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Event : MonoBehaviour
{
    public AudioMixer audioMixer;                   //오디오 믹서

    public Transform TileBoard;                     //빈 타일(퍼즐판)의 Parent
    public Transform BlockPieces;                   //아직 타일 위에 안 놓아진 퍼즐 조각들의 Parent
    public Transform HintPieces;                    //힌트 조각들이 들어갈 Parent
    public Transform BlockOnBoard;                  //타일위에 놓아진 퍼즐 조각들의 Parent
    public GameObject Boy;                          //파랭이
    public GameObject Girl;                         //분홍이

    public Button ResetBtn;                         //퍼즐 초기화 & 파랭이 움직임 리셋 버튼
    public Button GonfasterBtn;                     //출발/가속 버튼
    public Button hintBtn;                          //힌트 버튼

    ///옵션 창 관련
    public GameObject OptionMenu;
    public static bool GameIsPaused = false;        // Game pause
    public Button OptionExit;                       // 옵션 나가기
    ///옵션 창 관련

    ///퍼즐 완료 창 관련
    public GameObject PuzzleSolvedPanel;
    public Text coinText;
    public Text timeText;
    [HideInInspector] public bool coinChangeToggle = true;
    ///퍼즐 완료 창 관련

    ///튜토리얼 창 관련
    public GameObject tutorialPanel;
    int firstTime = 2;
    bool tutorialDo = true;
    ///튜토리얼 창 관련

    ///Dev Tools 관련
    public GameObject DevTools;
    ///Dev Tools 관련

    Vector2 mousePos;                               //마우스의 2차원상 위치
    Transform objToFollowMouse;                     //마우스를 따라 다닐 물체(퍼즐 조각)
    GameObject[] triggeredObjects;                  //Array stores info on EmptyTiles        //퍼즐 조각의 빈 타일 탐지용
    Vector3[] PiecePosition;                        //Stores initial position of Pieces      //초기 퍼즐 조각 위치 저장

    float UIPieceScale;                             //UI에서의 퍼즐 조각 크기. 화면/퍼즐에 놓았을 때는 1, UI상에서는 현재 값으로 축소
    int goNFastBtnState;                            //1 -> Move Boy!, 2 -> Make Boy Faster!, 3 -> Make Boy back to normal speed     출발/가속 버튼용 변수
    public GameObject backGround;                   //Player의 DifficultyFactor에 따라
    [HideInInspector] public bool MovePieceMode;    //boolean for Update Function //Update에 쓸 bool 변수

    /// Variables for Level Loading
    LevelDatabase levelData;
    int levelNum;
    float scaleFactor;
    float distanceBetweenTiles;
    float emptyTileScale;
    float pieceScale;
    /// Variables for Level Loading

    /// For DevTools Elements
    public Text PlayerDFactorText;
    public Text DfactorText;

    public Text UsedHintText;
    public Text UsedTouchText;
    public Text LogDisplayText;
    [HideInInspector] public int HintUsed = 0;
    [HideInInspector] public int TouchUsed = 0;
    /// For DevTools Elements

    /// For DFactor Rate Change
    bool applyRating;
    float levelDFactor;
    /// For DFactor Rate Change

    /// For Timer
    [HideInInspector] public float elapsedTime = 0f;
    [HideInInspector] public bool timeCount;
    /// For Timer

    void Start()
    {
        //변수, PlayerPrefs 초기화
        Initialize();

        //levelData 게임 스테이지 데이터베이스에서 데이터를 불러와서 현재 스테이지 생성
        LoadLevel();

        //퍼즐 조각 초기 위치 저장
        SavePiecePosition();

        //stageLoad();
    }

    void Initialize()
    {
        MovePieceMode = true;

        goNFastBtnState = 1;
        GonfasterBtn.interactable = false;

        levelNum = 1;                                        //MANUALLY SET STARTING LEVEL NUMBER BY CHANGING THIS VALUE
        levelData = new LevelDatabase();

        UIPieceScale = 0.5f;                                 //UI에서의 퍼즐 조각 크기. 화면/퍼즐에 놓았을 때는 1, UI상에서는 현재 값으로 축소

        applyRating = true;

        timeCount = false;

        //PlayerPrefs.SetInt("tutorial",0);//확인중임 없애도 됨. 기본은 0놓고했었음
        PlayerPrefs.SetInt("Piecedata", 1);

        //MANUALLY SET STARTING PLAYER'S DIFFICULTYFACTOR BY CHANGING THIS VALUE
        //PlayerPrefs.SetFloat("PlayerDFactor", -1f);
    }

    void Update()
    {
        if (timeCount)
        {
            elapsedTime += Time.deltaTime;
        }
            
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
                    //클릭한 물체가 퍼즐 조각일 경우
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

                            //If Tile is on the board
                            if (objToFollowMouse.IsChildOf(BlockOnBoard))
                            {
                                objToFollowMouse.GetChild(i).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject.GetComponent<BoxCollider2D>().enabled = true;  //Disable Box Collider of EmptyTile
                                objToFollowMouse.GetChild(i).GetChild(0).GetComponent<BoxCollider2D>().enabled = true;  //Enable Detector Box Collider
                            }
                        }

                        SoundFXPlayer.Play("pick");
                        timeCount = true;
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
                            triggeredObjects[i].GetComponent<BoxCollider2D>().enabled = false;                          //Disable EmptyTile's BoxCollider
                            objToFollowMouse.GetChild(i).GetChild(0).GetComponent<BoxCollider2D>().enabled = false;     //Disable Detector's BoxCollider
                            objToFollowMouse.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 10;
                        }

                        SoundFXPlayer.Play("put");

                        CheckIfAllTilesInPlace();
                    }
                    else
                    {
                        ResetPiecePosition(objToFollowMouse, Mathf.Abs(objToFollowMouse.localPosition.x) >= levelData.piecePlaceXMin && Mathf.Abs(objToFollowMouse.localPosition.x) < levelData.piecePlaceXMax && objToFollowMouse.localPosition.y >= levelData.piecePlaceYMin && objToFollowMouse.localPosition.y < levelData.piecePlaceYMax);
                    }
                    TouchUsed++;
                    objToFollowMouse = null;
                }
            }
        }
    }

    //게임 레벨 불러오기
    void LoadLevel(bool playAgain = false)
    {
        GameObject prefab;
        GameObject obj;
        GameObject obj2;

        //OLD : using int levelNum
        //levelData.LoadLevelData(levelNum);

        //NEW : using PlayerDFactor
        //int lineNum = levelData.LoadLevelData();
        if (!playAgain)
        {
            levelDFactor = levelData.LoadLevelData();
        }

        //DfactorText.text = levelData.ReadFileByLine("LevelDifficulty", lineNum);
        //levelDFactor = float.Parse(DfactorText.text);
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DfactorText.text = "Level: " + levelDFactor.ToString();

        int typeIndex;
        int pieceHeight;
        int pieceWidth;

        HintUsed = 0;
        TouchUsed = 0;
        elapsedTime = 0f;
        timeCount = false;

        //Reset Position of BlockPieces
        BlockPieces.transform.position = new Vector3(0, -3.75f, 0);

        //Add Scaling by scaleSize!
        scaleFactor = 1 - 0.2f * (levelData.scaleSize - 1);
        distanceBetweenTiles = 2 * scaleFactor;
        emptyTileScale = 0.25f * scaleFactor;
        pieceScale = 1 * scaleFactor;

        //Load map by levelfactor
        if(levelDFactor < -0.55f) //level factor < -0.55인데 여기서는 어쩔수 없이 갯수로
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/11");
        else if(levelDFactor < 0f)
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/22");
        else if (levelDFactor < 0.6f)
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/33");
        else
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/44");

        //Instantiate 'EmptyTile'
        typeIndex = 0;
        for (int i = 0; i < levelData.BoardHeight; i++)
        {
            for (int j = 0; j < levelData.BoardWidth; j++)
            {
                //Get prefab information from array
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

                obj = Instantiate(prefab, new Vector3((-levelData.BoardWidth + 1) * (distanceBetweenTiles / 2f) + distanceBetweenTiles * j, (levelData.BoardHeight - 1) * (distanceBetweenTiles / 2f) - distanceBetweenTiles * i, 0), Quaternion.identity);//이부분이 생성하는 부분
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
                else if(j == 0)
                {
                    //Detect Error #1
                    if (i >= levelData.BoyPos - 0.5 && i <= levelData.BoyPos + 0.5)
                    {
                        LogDisplayText.text = "Error #1";
                        Debug.LogError("Error : Set Boy Position");
                    }
                }

                if (i == levelData.GirlPos && j == levelData.BoardWidth - 1)
                {
                    Girl.transform.position = obj.transform.position + new Vector3(distanceBetweenTiles, 0, 0);
                    Girl.transform.localScale = new Vector3(emptyTileScale, emptyTileScale, 1);
                }
                typeIndex++;
            }
        }

        //Instantiate 'Piece' & 'Tile'
        //Random Puzzle Piece Position Version
        //PieceInitPosition = new Vector3[levelData.NumberOfPieces];
        for (int i = 0; i < levelData.NumberOfPieces; i++)
        {
            prefab = Resources.Load("Prefabs/Piece") as GameObject;
            obj = Instantiate(prefab, new Vector3(Random.value < 0.5 ? Random.Range(-(levelData.piecePlaceXMax - 0.5f), -(levelData.piecePlaceXMin + 0.5f)) : Random.Range(levelData.piecePlaceXMin + 0.5f, levelData.piecePlaceXMax - 0.5f), Random.Range(levelData.piecePlaceYMin + 0.4f, levelData.piecePlaceYMax)), Quaternion.identity);
            obj.transform.SetParent(BlockPieces, false);
            obj.GetComponent<VariableProvider>().pieceNum = i;
            obj.GetComponent<VariableProvider>().solutionLoc = levelData.pieceDatas[i].solutionLoc;

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

        //StartCoroutine(Waitsecond());
    }

    void SavePiecePosition()
    {
        PiecePosition = new Vector3[BlockPieces.childCount];
        for (int i = 0; i < BlockPieces.childCount; i++)
        {
            PiecePosition[i] = BlockPieces.GetChild(i).localPosition;
            BlockPieces.GetChild(i).GetComponent<VariableProvider>().pieceNum = i;
        }
    }

    //퍼즐 조각들이 모두 타일위에 놓아졌는지 확인
    void CheckIfAllTilesInPlace()
    {
        if(PiecePosition.Length == BlockOnBoard.childCount)
        {
            GonfasterBtn.interactable = true;
        }
        else
        {
            GonfasterBtn.interactable = false;
        }
    }

    //퍼즐 조각 하나의 위치 초기화
    void ResetPiecePosition(Transform piece, bool movePiecePosition = false)
    {
        piece.SetParent(BlockPieces);
        piece.localScale = new Vector3(UIPieceScale, UIPieceScale, 1);

        if (movePiecePosition)
        {
            PiecePosition[objToFollowMouse.GetComponent<VariableProvider>().pieceNum] = objToFollowMouse.localPosition;
        }
        else
        {
            piece.localPosition = PiecePosition[piece.GetComponent<VariableProvider>().pieceNum];
        }

        for (int i = 0; i < piece.childCount; i++)
        {
            piece.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 75 + piece.GetComponent<VariableProvider>().pieceNum;
        }
    }

    //현재 스테이지 요소들 삭제
    void DeleteLevel()
    {
        for (int i = TileBoard.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(TileBoard.GetChild(i).gameObject);
        }

        for (int i = BlockPieces.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(BlockPieces.GetChild(i).gameObject);
        }

        for (int i = BlockOnBoard.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(BlockOnBoard.GetChild(i).gameObject);
        }

        for (int i = HintPieces.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(HintPieces.GetChild(i).gameObject);
        }
    }

    //보드위 BlockOnBoard 초기화
    void ResetBoard()
    {
        Transform objToReset;
        for (int i = BlockOnBoard.childCount - 1; i >= 0; i--)
        {
            objToReset = BlockOnBoard.GetChild(0);

            for (int j = 0; j < objToReset.childCount; j++)
            {
                objToReset.GetChild(j).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject.GetComponent<BoxCollider2D>().enabled = true;
                objToReset.GetChild(j).GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
            }

            ResetPiecePosition(objToReset);
        }
    }

    //////////////////////////////////////////////// 아래로 public 함수들 ////////////////////////////////////////////////

    //출발/가속 버튼 State 1 -> 누르면 이동 시작, 2 -> 누르면 빨라짐, 3 -> 누르면 다시 원래 속도로 돌아옴
    public void GoNFastForwardClick()
    {
        timeCount = false;
        if (goNFastBtnState == 1)
        {
            Boy.GetComponent<MoveBoi>().MoveDaBoi();
            MovePieceMode = false;
            goNFastBtnState = 2;
            GonfasterBtn.image.sprite = Resources.Load<Sprite>("Arts/FastForward");
            SoundFXPlayer.Play("go");
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
    public void ResetGoNFaster()
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
            ResetGoNFaster();
        }
        else //During Puzzle Solving Phase
        {
            ResetBoard();
            CheckIfAllTilesInPlace();
        }
        
        timeCount = true;
    }

    //다음 스테이지 불러오기
    public void GoToNextLevel()
    {
        //OLD
        /*
        levelNum++;
        MovePieceMode = true;
        ResetBtn.interactable = true;
        hintBtn.interactable = true;

        DeleteLevel();
        levelData.LoadLevelData(levelNum);//For check new tile;
        if (levelData.tutorialCase)// 바꾸어야될듯? -> leveldata에서 CheckNewPieces앞에 false해서 이제 ㄱㅊ
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            LoadLevel();
            SavePiecePosition();
        }
        */

        //NEW
        MovePieceMode = true;
        ResetBtn.interactable = true;
        hintBtn.interactable = true;

        DeleteLevel();
        levelData.LoadLevelData();//For check new tile;
        if (levelData.tutorialCase)// 바꾸어야될듯? -> leveldata에서 CheckNewPieces앞에 false해서 이제 ㄱㅊ
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            LoadLevel();
            SavePiecePosition();
        }

        applyRating = true;

        /*
        levelNum = 0;
        levelData.dfac = -1;
        string s = levelData.ReadFileByFactor(levelData.dfac);
        levelData.GenerateSlicedPieces(s);
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
        MovePieceMode = true;
        */

        //퍼즐 완료창 종료
        PuzzleSolvedPanel.SetActive(false);
        coinChangeToggle = false;
    }

    public void PlayAgain()
    {
        MovePieceMode = true;
        ResetBtn.interactable = true;
        hintBtn.interactable = true;

        /* ResetBoard */
        DeleteLevel();
        LoadLevel(true);
        SavePiecePosition();
        /* ResetBoard */

        Boy.GetComponent<MoveBoi>().ResetBoyPosition();

        applyRating = false;

        //퍼즐 완료창 종료
        PuzzleSolvedPanel.SetActive(false);
        coinChangeToggle = false;
    }

    public void HintButtonClick()
    {
        ResetBoard();
        HintUsed++;
        /*if(Advertisement.IsReady())
        {
            Advertisement.Show("video");
        }*/ //hint activation
        if (BlockPieces.childCount != 0)
        {
            int random = Random.Range(0, BlockPieces.childCount);
            Transform hintPiece = BlockPieces.GetChild(random).transform;
            Vector3 solutionLoc = BlockPieces.GetChild(random).GetComponent<VariableProvider>().solutionLoc;

            //Place piece on the board
            hintPiece.SetParent(HintPieces, true);
            hintPiece.localPosition = solutionLoc * distanceBetweenTiles;
            hintPiece.localScale = new Vector3(pieceScale, pieceScale, 1);

            //Might need to change the mechanism of this
            StartCoroutine(SetColliders(hintPiece));

            SoundFXPlayer.Play("put"); //ADD MORE SOUNDS??
            SavePiecePosition();
            if (BlockPieces.childCount == 0)
            {
                GonfasterBtn.interactable = true;
            }
            /////////////////////////////////////////////////////////////
        }

        //바로 하면 Collider인식이 안되서 Delay를 줌
        IEnumerator SetColliders(Transform hintPiece)
        {
            yield return new WaitForSeconds(0.05f);

            //Disable Corresponding EmptyTile BoxCollider2D & Detectors
            //For each tile in piece
            for (int i = 0; i < hintPiece.childCount; i++)
            {
                hintPiece.GetChild(i).GetChild(0).GetComponent<TileCollideDetection>().overlappedObject.GetComponent<BoxCollider2D>().enabled = false; //Turn Off EmptyTile Collider
                hintPiece.GetChild(i).GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
                hintPiece.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 10;
                hintPiece.GetChild(i).gameObject.tag = "Hint";
            }
        }
    }

    //테스트용 개발자 버튼용
    public void DevBtnAct()  //Go To Level X
    {
        //바로 다음 퍼즐로 ㄱ
        levelNum++;
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
        //Change Text Display For difficultyfactor

        //GenerateSlicedPieces 테스트용 실행코드
        /*
        levelNum = 0;
        levelData.dfac = -1;
        string s = levelData.ReadFileByFactor(levelData.dfac);
        //string s = "13302141534313";
        levelData.GenerateSlicedPieces(s);
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
        */
        //levelData.PieceCutterModuleTEST();
    }

    //Go To Level +10
    public void DevBtnAct2()
    {
        levelNum = levelNum + 10;
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
    }

    //Go To Level +100
    public void DevBtnAct3()
    {
        levelNum = levelNum + 100;
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
    }

    public void DevDFactorIncrease()
    {
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");

        if(playerDFactor >= 0.99f)
        {
            playerDFactor = 1f;
        }
        else
        {
            playerDFactor += 0.01f;
        }

        PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
    }

    public void DevDFactorDecrease()
    {
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");

        if (playerDFactor <= -0.99f)
        {
            playerDFactor = -1f;
        }
        else
        {
            playerDFactor -= 0.01f;
        }

        PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DeleteLevel();
        LoadLevel();
        SavePiecePosition();
    }

    //옵션 버튼을 눌러 Option창 토글
    public void ToggleOptionPanel()
    {
        if(OptionMenu.activeSelf)
        {
            OptionMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            OptionMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    //옵션 창의 닫기 버튼을 눌러 Option창 닫기
    public void CloseOptionPanel()
    {
        OptionMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    //Dev Tools Toggle
    public void ToggleDevTools()
    {
        if (DevTools.activeSelf)
        {
            DevTools.SetActive(false);
        }
        else
        {
            DevTools.SetActive(true);
        }
    }

    //퍼즐 완료창 코인 또로로로 효과
    public IEnumerator CoinIncreaseAnimation(int coin = 100)
    {
        coinText.text = "";
        yield return new WaitForSeconds(0.5f);
        int i = 0;
        coinChangeToggle = true;
        while (i < coin + 1 && coinChangeToggle)
        {
            coinText.text = i.ToString();
            CoinFXPlayer.Play();
            i++;
            yield return null;
        }
    }

    //DevTools Display에 Hint, Touch, Restart 뜨게 함
    public void DisplayPlayData()
    {
        UsedHintText.text = "Hint : " + HintUsed.ToString();
        UsedTouchText.text = "Touch : " + TouchUsed.ToString();
    }

    //PuzzleSolved 창에 문제 푼 시간 뜨게 함
    public void DisplayTime()
    {
        int time_ = (int) elapsedTime;
        if (time_ < 60)
        {
            timeText.text = "00 : 00 : " + convFormat(time_);
        }
        else if (time_ < 3600)
        {
            timeText.text = "00 : " + convFormat(time_ / 60) + " : " + convFormat(time_ % 60);
        }
        else
        {
            timeText.text = convFormat(time_ / 3600) + " : " + convFormat(time_ / 60) + " : " + convFormat(time_ % 60);
        }

        string convFormat(int time)
        {
            if (time < 10)
            {
                return "0" + time;
            }
            else
            {
                return time.ToString();
            }
        }
    }

    //플레이어의 PlayerDFactor 변경
    public void ChangeRating()
    {
        // RATE ONLY WHEN IT IS FIRST TIME SOLVING
        if (applyRating)
        {
            //Base Starting Rate
            float rate = 0.01f;
            
            int boardSize = levelData.BoardWidth * levelData.BoardHeight;
            int numOfPieces = levelData.NumberOfPieces;
            float hintChange;
            float touchChange;
            float timeChange;

            //HINT
            if (boardSize < 17)
            {
                if (HintUsed < 3)
                {
                    hintChange = HintUsed * 0.005f;
                }
                else
                {
                    hintChange = 0.015f;
                }
            }
            else
            {
                if (HintUsed < 4)
                {
                    hintChange = HintUsed * 0.004f;
                }
                else
                {
                    hintChange = 0.015f;
                }
                
            }
            rate -= hintChange;

            //TOUCH
            touchChange = (TouchUsed - numOfPieces) * (-0.005f / (4 * numOfPieces));
            rate -= touchChange;
            
            //TIME
            int timeUsed = (int) elapsedTime;
            if (timeUsed <= 200)
            {
                timeChange = timeUsed * 0.00001f;
            }
            else if (timeUsed <= 300)
            {
                timeChange = 0.002f + (timeUsed - 200) * 0.000005f;
            }
            else
            {
                timeChange = 0.0025f;
            }

            //Adaptation by DFactor
            rate += levelData.DFactorDiff / 4;

            //Lower Level Rate Change Bonus
            if (boardSize < 9 && rate > 0)
            {
                rate = rate * 2;
            }

            //Higher Level Rate Change Decrease
            if (boardSize == 25 && rate > 0)
            {
                rate = rate * 0.75f;
            }

            //CHANGE RATE WITH RANDOM VALUE
            //rate = Random.Range(-0.005f, 0.01f);

            //DISPLAY RATE CHANGE INFO
            /*
            Debug.Log("Hint Change : " + hintChange);
            Debug.Log("Touch Change : " + touchChange);
            Debug.Log("Time Change : " + timeChange);
            Debug.Log("Diff Change : " + DFactorDiff / 2);
            Debug.Log("Rate Change : " + rate);
            */

            float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");
            playerDFactor += rate;
            if(playerDFactor <= -1f) playerDFactor = -1f;
            if(playerDFactor >= 1f) playerDFactor = 1f;
            PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        }
    }

    //오디오믹서의 배경음악 볼륨 조절
    public void SetMusicVolume(float vol)
    {
        audioMixer.SetFloat("MusicVol", vol);
    }

    //오디오믹서의 효과음 볼륨 조절
    public void SetSFXVolume(float vol)
    {
        audioMixer.SetFloat("SFXVol", vol);
    }

    //오디오믹서의 환경음 볼륨 조절
    public void SetAmbienceVolume(float vol)
    {
        audioMixer.SetFloat("AmbienceVol", vol);
    }

    //save
    void stageSave()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        PlayerPrefs.Save();
    }

    //load
    void stageLoad()
    {
        firstTime = PlayerPrefs.GetInt("tutorial");
    }

    //reset
    void stageReset()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        PlayerPrefs.Save();
    }

    public void tutorialOff() //tutorial on/off -> 버튼
    {
        if (PlayerPrefs.GetInt("tutorial") == 0) // 0이면 처음부터
        {
            PlayerPrefs.SetInt("tutorial", 1);// 1이면 loadlevel 사용
            tutorialPanel.SetActive(false);
            Initialize();
            LoadLevel();
            SavePiecePosition();
        }
        else if(PlayerPrefs.GetInt("tutorial") == 1 && tutorialDo)
        {
            tutorialPanel.SetActive(false);
            tutorialDo = false;
            //변수 초기화
            Initialize();
            LoadLevel();
            SavePiecePosition();
        }
        else//level불러오는거
        {
            tutorialPanel.SetActive(false);
            LoadLevel();
            SavePiecePosition();
        }
    }

    IEnumerator Waitsecond()
    {
        yield return new WaitForSeconds(0.3f);
    }

}
