using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Event : MonoBehaviour
{
    public AudioMixer audioMixer;                   //오디오 믹서
    public GameObject audioManager;

    public BGAnimate BgAnimate;

    public Transform MainBoard;
    Transform TileBoard;                            //빈 타일(퍼즐판)의 Parent
    Transform BlockPieces;                          //아직 타일 위에 안 놓아진 퍼즐 조각들의 Parent
    Transform HintPieces;                           //힌트 조각들이 들어갈 Parent
    Transform BlockOnBoard;                         //타일위에 놓아진 퍼즐 조각들의 Parent

    public GameObject Boy;                          //파랭이
    public GameObject Girl;                         //분홍이

    public Button ResetBtn;                         //퍼즐 초기화 & 파랭이 움직임 리셋 버튼
    public Button GonfasterBtn;                     //출발/가속 버튼
    public Button hintBtn;                          //힌트 버튼

    ///옵션 창 관련
    public GameObject OptionMenu;
    public static bool GameIsPaused = false;        // Game pause
    public Button OptionExit;                       // 옵션 나가기
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider AmbSlider;
    ///옵션 창 관련

    ///퍼즐 완료 창 관련
    public GameObject PuzzleSolvedPanel;
    public Text coinText;
    public Text timeText;
    [HideInInspector] public bool coinChangeToggle = true;
    ///퍼즐 완료 창 관련

    Vector2 mousePos;                               //마우스의 2차원상 위치
    Transform objToFollowMouse;                     //마우스를 따라 다닐 물체(퍼즐 조각)
    GameObject[] triggeredObjects;                  //Array stores info on EmptyTiles        //퍼즐 조각의 빈 타일 탐지용
    Vector3[] PiecePosition;                        //Stores initial position of Pieces      //초기 퍼즐 조각 위치 저장

    float UIPieceScale;                             //UI에서의 퍼즐 조각 크기. 화면/퍼즐에 놓았을 때는 1, UI상에서는 현재 값으로 축소
    int goNFastBtnState;                            //1 -> Move Boy!, 2 -> Make Boy Faster!, 3 -> Make Boy back to normal speed     출발/가속 버튼용 변수
    public GameObject backGround;                   //Player의 DifficultyFactor에 따라
    [HideInInspector] public bool MovePieceMode;    //boolean for Update Function //Update에 쓸 bool 변수

    /// For Level Loading
    LevelDatabase levelData;
    float scaleFactor;
    float distanceBetweenTiles;
    float emptyTileScale;
    float pieceScale;
    /// For Level Loading

    /// For Tutorial
    TutLevelDatabase tutLevelData;
    public Transform tutBoard;
    public GameObject finger1;
    public GameObject finger2;
    int tutLevel;
    bool isTutorial;
    int fingerAnimate = 0;
    Vector3 fingerTarget;
    Vector3 firstPlace;
    GameObject tilePlace;
    float fingerSpeed;
    float finger2min = 0f;
    float finger2max = 0.5f;
    float finger2t;
    /// For Tutorial

    /// For DevTools
    public GameObject DevTools;
    public Text PlayerDFactorText;
    public Text DfactorText;
    public Text UsedHintText;
    public Text UsedTouchText;
    public Text LogDisplayText;
    public Text BGMTitleText;
    [HideInInspector] public int HintUsed = 0;
    [HideInInspector] public int TouchUsed = 0;
    /// For DevTools

    /// For DFactor Rate Change
    bool applyRating; 
    [HideInInspector] public float levelDFactor;
    /// For DFactor Rate Change

    /// For Timer
    [HideInInspector] public float elapsedTime = 0f;
    [HideInInspector] public bool timeCount;
    /// For Timer

    /// For Application Quit
    float prevTime;
    /// For Application Quit

    void Start()
    {
        //변수, PlayerPrefs 초기화
        Initialize();

        //LevelDatabase에서 데이터 불러와서 현재 필요한 스테이지 생성
        LoadLevel();
    }

    void Initialize()
    {
        MovePieceMode = true;
        goNFastBtnState = 1;
        GonfasterBtn.interactable = false;
        UIPieceScale = 0.45f;                                //UI에서의 퍼즐 조각 크기. 화면/퍼즐에 놓았을 때는 1, UI상에서는 현재 값으로 축소
        applyRating = false;
        timeCount = false;

        //MANUALLY SET STARTING DIFFICULTYFACTOR BY CHANGING THIS VALUE
        //PlayerPrefs.SetFloat("PlayerDFactor", -1f);

        //MANUALLY SET STARTING TUTLEVEL BY CHANGING THIS VALUE; DEFAULT 0 -> 평소에는 주석처리 되어 있어야함
        //PlayerPrefs.SetInt("tutorial", 1);

        TileBoard = MainBoard.GetChild(0);
        BlockOnBoard = MainBoard.GetChild(1);
        BlockPieces = MainBoard.GetChild(2);
        HintPieces = MainBoard.GetChild(3);

        SetAudioSliderSettings();

        levelData = new LevelDatabase();

        fingerAnimate = 0;
        isTutorial = false;

        prevTime = -2f;                                      //For Quitting Program on Android Back Button

        finger2t = 0f;

        if (!GameObject.Find("AudioManager"))
        {
            audioManager.SetActive(true);
        }
    }

    void Update()
    {
        if (timeCount)
        {
            elapsedTime += Time.deltaTime;
        }

        //퍼즐조각 움직임 enable/disable
        //if (MovePieceMode && Time.timeScale != 0f)
        if (MovePieceMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MouseButtonDownExec();
            }

            if (Input.GetMouseButton(0))
            {
                MouseButtonExec();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseButtonUpExec();
            }
        }

        //Quit Program on Android Back Button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            float curTime = Time.time;
            if (curTime - prevTime <= 1.5f)
            {
                Application.Quit();
            }
            prevTime = curTime;
        }
    }

    void FixedUpdate()
    {
        //For tutorial finger animation
        if (fingerAnimate == 1)
        {
            FingerAnim1();
        }
        else if (fingerAnimate == 2)
        {
            FingerAnim2();
        }
    }

    void MouseButtonDownExec()
    {
        //마우스 클릭시 충돌 물체 탐지
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            //클릭한 물체가 퍼즐 조각일 경우
            if (hit.transform.tag == "Tile")
            {
                if (GonfasterBtn.interactable)
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

    void MouseButtonExec()
    {
        //마우스로 퍼즐 조각 드래그
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (objToFollowMouse != null)
        {
            objToFollowMouse.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    void MouseButtonUpExec()
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

                GonfasterBtn.interactable = CheckIfAllTilesInPlace();

                if (isTutorial)
                {
                    SetFingerAnim();
                }
            }
            else
            {
                ResetPiecePosition(objToFollowMouse, Mathf.Abs(objToFollowMouse.localPosition.x) >= levelData.piecePlaceXMin && Mathf.Abs(objToFollowMouse.localPosition.x) < levelData.piecePlaceXMax && objToFollowMouse.localPosition.y >= levelData.piecePlaceYMin && objToFollowMouse.localPosition.y < levelData.piecePlaceYMax);
            }

            TouchUsed++;
            objToFollowMouse = null;
        }
    }

    void FingerAnim1()
    {
        //Set finger speed dynamically for more realistic anim: Slow If near start/end, Fast otherwise
        fingerSpeed = 0.1f * (Mathf.Min(Vector3.Distance(firstPlace, finger1.transform.position), Vector3.Distance(fingerTarget, finger1.transform.position)) + 0.1f);

        finger1.transform.position = Vector3.MoveTowards(finger1.transform.position, fingerTarget, fingerSpeed);
        //If loop ended
        if (finger1.transform.position == tilePlace.transform.position)
            finger1.transform.position = firstPlace;
    }

    void FingerAnim2()
    {
        finger2.transform.localPosition = new Vector3(0, Mathf.Lerp(finger2min, finger2max, finger2t), 0);
        finger2t += Time.deltaTime * 2.5f;
        if (finger2t > 1f)
        {
            float temp = finger2max;
            finger2max = finger2min;
            finger2min = temp;
            finger2t = 0f;
        }
    }

    //게임 레벨 불러오기
    void LoadLevel(bool playAgain = false)
    {
        if (!playAgain)
        {
            /////////////////////////CHECK IF TUTORIAL NEEDS TO BE LOADED/////////////////////////
            tutLevel = PlayerPrefs.GetInt("tutorial", 1);
            levelDFactor = levelData.LoadLevelData();

            isTutorial = tutLevel < 8;
            /*
            if (tutLevel < 5)
            {
                isTutorial = true;
            }
            else if(tutLevel == 5 && levelData.contains67)
            {
                isTutorial = true;
            }
            else if(tutLevel == 6)
            {
                isTutorial = true;
            }
            else if(tutLevel == 7 && levelData.contains89)
            {
                isTutorial = true;
            }
            else
            {
                isTutorial = false;
            }
            */
            /////////////////////////CHECK IF TUTORIAL NEEDS TO BE LOADED/////////////////////////
        }
        else // -> PlayAgain true
        {
            applyRating = false;
        }

        //Load Level according to isTutorial
        if (isTutorial)
        {
            //tutLevelData.LoadTutorialData(tutLevel);
            //TutExec(tutLevel);
            levelData.LoadTutorialData(tutLevel);
            backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/tut_2");
        }
        else
        {
            //Toggle Off Tutorial Settings
            applyRating = true;
            finger1.SetActive(false);
            fingerAnimate = 0;

            PlayerDFactorText.text = "Player: " + PlayerPrefs.GetFloat("PlayerDFactor").ToString();
            DfactorText.text = "Level: " + levelDFactor.ToString();

            //Load Different Background according to corresponding levelDfactor
            if (levelDFactor < -0.55f)
                backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/1_1");
            else if (levelDFactor < 0f)
                backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/2_1");
            else if (levelDFactor < 0.5f)
                backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/3_1");
            else
                backGround.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/4_1");
            BgAnimate.ToggleBGAnim(levelDFactor);       //Toggle Background animation components by level

            hintBtn.interactable = true;
        }

        GameObject prefab;
        GameObject obj;
        GameObject obj2;

        int typeIndex;
        int pieceHeight;
        int pieceWidth;

        HintUsed = 0;
        TouchUsed = 0;
        elapsedTime = 0f;
        timeCount = false;

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

                //Set Boy Position
                if (i == levelData.BoyPos && j == 0)
                {
                    Boy.transform.position = obj.transform.position - new Vector3(distanceBetweenTiles, 0, 0);
                    Boy.GetComponent<MoveBoi>().initTargetPosition = obj.transform.position;
                    Boy.GetComponent<MoveBoi>().distanceBetweenTiles = distanceBetweenTiles;
                    Boy.transform.localScale = new Vector3(emptyTileScale, emptyTileScale, 1);
                }

                //Set Girl Position
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
        if (PlayerPrefs.GetInt("tutorial") != 1)
        {
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
        }

        SavePiecePosition();

        if (isTutorial) TutExec(tutLevel);
    }

    //퍼즐 조각 초기 위치 저장
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
    bool CheckIfAllTilesInPlace()
    {
        if(PiecePosition.Length == BlockOnBoard.childCount)
        {
            return true;
        }
        else if(PlayerPrefs.GetInt("tutorial") == 1)
        {
            return true;
        }
        else
        {
            return false;
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

            if (isTutorial)
            {
                SetFingerAnim();
            }
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
            GonfasterBtn.interactable = CheckIfAllTilesInPlace();
        }
        
        timeCount = true;
    }

    //다음 스테이지 불러오기
    public void GoToNextLevel()
    {
        int tutLevel = PlayerPrefs.GetInt("tutorial");

        if(isTutorial)
        {
            tutLevel++;
            PlayerPrefs.SetInt("tutorial", tutLevel);
        }

        DeleteLevel();
        LoadLevel();

        MovePieceMode = true;
        ResetBtn.interactable = true;

        //퍼즐 완료창 종료
        PuzzleSolvedPanel.SetActive(false);
        coinChangeToggle = false;
    }

    public void PlayAgain()
    {
        MovePieceMode = true;
        ResetBtn.interactable = true;
        hintBtn.interactable = !isTutorial;

        /* ResetBoard */
        DeleteLevel();
        LoadLevel(true);
        /* ResetBoard */

        Boy.GetComponent<MoveBoi>().ResetBoyPosition();

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

    public void DevDFactorIncrease()
    {
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");

        if(playerDFactor >= 0.97f)
        {
            playerDFactor = 0.98f;
        }
        else
        {
            playerDFactor += 0.01f;
        }

        PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DeleteLevel();
        LoadLevel();
    }

    public void DevDFactorDecrease()
    {
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");

        if (playerDFactor <= -0.98f)
        {
            playerDFactor = -0.99f;
        }
        else
        {
            playerDFactor -= 0.01f;
        }

        PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DeleteLevel();
        LoadLevel();
    }

    public void DevDFactorBigIncrease()
    {
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");

        if (playerDFactor >= 0.45f)
        {
            playerDFactor = 0.98f;
        }
        else
        {
            playerDFactor += 0.55f;
        }

        PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DeleteLevel();
        LoadLevel();
    }

    public void DevDFactorBigDecrease()
    {
        float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");

        if (playerDFactor <= -0.45f)
        {
            playerDFactor = -0.99f;
        }
        else
        {
            playerDFactor -= 0.55f;
        }

        PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        PlayerDFactorText.text = "Player: " + playerDFactor.ToString();
        DeleteLevel();
        LoadLevel();
    }

    //옵션 버튼을 눌러 Option창 토글
    public void ToggleOptionPanel()
    {
        if(OptionMenu.activeSelf)
        {
            OptionMenu.SetActive(false);
            if(fingerAnimate == 1)
            {
                finger1.SetActive(true);
            }
            else if(fingerAnimate == 2)
            {
                finger2.SetActive(true);
            }
            Time.timeScale = 1f;
        }
        else
        {
            OptionMenu.SetActive(true);
            if (fingerAnimate == 1)
            {
                finger1.SetActive(false);
            }
            else if (fingerAnimate == 2)
            {
                finger2.SetActive(false);
            }
            Time.timeScale = 0f;
        }
    }

    //옵션 창의 닫기 버튼을 눌러 Option창 닫기
    public void CloseOptionPanel()
    {
        OptionMenu.SetActive(false);
        if (fingerAnimate == 1)
        {
            finger1.SetActive(true);
        }
        else if (fingerAnimate == 2)
        {
            finger2.SetActive(true);
        }
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
    public void ChangeRating(int numOfSteps)
    {
        // RATE ONLY WHEN IT IS FIRST TIME SOLVING
        if (applyRating)
        {
            float playerDFactor = PlayerPrefs.GetFloat("PlayerDFactor");
            //Base Starting Rate
            float rate = 0.01f;
            
            int boardSize = levelData.BoardWidth * levelData.BoardHeight;
            int numOfPieces = levelData.NumberOfPieces;
            float hintChange;
            float touchChange;
            float timeChange;

            //HINT
            if (boardSize < 17)  //Small Board
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
            else   //Big Board
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

            //Warp Tile 꼼수 관련
            if (boardSize >= 16 && numOfSteps < 5)
            {
                rate = 0;
            }

            //Lower Level Rate Change Bonus
            if (playerDFactor < -0.5f && rate > 0)
            {
                rate = rate * 1.5f;
            }

            //Higher Level Rate Change Decrease
            if (playerDFactor > 0.5f && rate > 0)
            {
                rate = rate * 0.75f;
            }

            //CHANGE RATE WITH RANDOM VALUE
            //rate = Random.Range(-0.005f, 0.01f);

            //DISPLAY RATE CHANGE
            /*
            Debug.Log("Hint Change : " + hintChange);
            Debug.Log("Touch Change : " + touchChange);
            Debug.Log("Time Change : " + timeChange);
            Debug.Log("Diff Change : " + DFactorDiff / 2);
            */
            Debug.Log("Rating Change : " + rate);
            
            playerDFactor += rate;
            if(playerDFactor <= -1f) playerDFactor = -1f;
            if(playerDFactor >= 1f) playerDFactor = 1f;
            PlayerPrefs.SetFloat("PlayerDFactor", playerDFactor);
        }
    }

    //오디오믹서의 배경음악 볼륨 조절
    public void SetMusicVolume(float vol)
    {
        if (vol <= -4f)
        {
            audioMixer.SetFloat("MusicVol", -80f);
        }
        else
        {
            audioMixer.SetFloat("MusicVol", -4f * vol * vol);
        }
        PlayerPrefs.SetFloat("MusicVol", vol);
    }

    //오디오믹서의 효과음 볼륨 조절
    public void SetSFXVolume(float vol)
    {
        if (vol <= -4f)
        {
            audioMixer.SetFloat("SFXVol", -80f);
        }
        else
        {
            audioMixer.SetFloat("SFXVol", -4f * vol * vol);
        }
        PlayerPrefs.SetFloat("SFXVol", vol);
    }

    //오디오믹서의 환경음 볼륨 조절
    public void SetAmbienceVolume(float vol)
    {
        if (vol <= -4f)
        {
            audioMixer.SetFloat("AmbienceVol", -80f);
        }
        else
        {
            audioMixer.SetFloat("AmbienceVol", -4f * vol * vol);
        }
        PlayerPrefs.SetFloat("AmbVol", vol);
    }

    public void DisplayBGMTitle(string title)
    {
        if(BGMTitleText) BGMTitleText.text = title;
    }

    void SetAudioSliderSettings()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVol", -1f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol", 0f);
        AmbSlider.value = (PlayerPrefs.GetFloat("AmbVol", -1f));
    }

    IEnumerator Waitsecond(float time)
    {
        yield return new WaitForSeconds(time);
    }

    //////////////////////////////////////////////// 아래로 Tutorial용 함수들 ////////////////////////////////////////////////
    void TutExec(int tutLevel)
    {
        //Tutorial Settings
        applyRating = false;
        hintBtn.interactable = false;

        if (tutLevel == 1)
        {
            GonfasterBtn.interactable = true;
        }

        SetFingerAnim();
    }

    void SetFingerAnim() 
    {
        if(fingerAnimate == 2)
        {
            fingerAnimate = 0;
            finger2.SetActive(false);
        }
        else
        {
            try
            {
                //Grab next tutorial piece
                tilePlace = BlockPieces.GetChild(0).gameObject;
            }
            catch
            {   //No more tutorial piece
                finger1.SetActive(false);

                //Swap from 1 to 2
                finger2.SetActive(true);
                fingerAnimate = 2;

                return;
            }

            finger1.SetActive(true);
            firstPlace = tilePlace.transform.position;
            finger1.transform.position = tilePlace.transform.position;

            //Debug.Log(GameObject.FindGameObjectWithTag("EmptyTile").GetComponent<BoxCollider2D>().enabled);
            GameObject[] test = GameObject.FindGameObjectsWithTag("EmptyTile");
            for (int i = 0; i < test.Length; i++)
            {
                if (test[i].GetComponent<BoxCollider2D>().enabled == true)
                {
                    tilePlace = test[i];
                    break;
                }
            }
            //tilePlace = GameObject.FindGameObjectWithTag("EmptyTile");// Box colider on 되어있는거 가져와야됨

            fingerTarget = tilePlace.transform.position;

            fingerAnimate = 1;
        }
    }

    public void ActivateHelpTutBtn()
    {
        OptionMenu.SetActive(false);
        if (tutBoard.gameObject.activeSelf)
        {
            tutBoard.gameObject.SetActive(false);
            
        }
        else
        {
            tutBoard.gameObject.SetActive(true);
        }
    }
}
