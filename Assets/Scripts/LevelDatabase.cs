﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelDatabase
{
    public int scaleSize;
    public int BoardWidth;
    public int BoardHeight;
    public int BoyPos;
    public int GirlPos;
    public int NumberOfPieces;
    public int[] BoardEmptyTileTypeInfo;
    public PieceData[] pieceDatas;
    [HideInInspector]
    public bool tutorialCase = false;
    [HideInInspector]
    public float dfac = 0;

    public class PieceData
    {
        public int PieceWidth;
        public int PieceHeight;
        public int[] TileType;
        public Vector3 solutionLoc; //solutions for hint here
    }

    class LoadedMapTile //need public?
    {
        public int LoadedTileCode;
        public bool isBoardSelected = false;
    }

    public int[] ConvertStringToIntArray(string data)
    {
        int[] arr = new int[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] < 58 && data[i] > 47)
            {
                arr[i] = data[i] - '0';
            }
            else if (data[i] > 64 && data[i] < 91)
            {
                arr[i] = data[i] - '7';
            }
            else
            {
                Debug.LogError("string to int[] Conversion Failed!! :(");
            }
        }
        return arr;
    }

    public void ConvertStringToPieceInfo(string s)
    {
        int[] data = ConvertStringToIntArray(s);
        tutorialCase = false;
        CheckNewPieces(data);
        int d = 0;
        int pieceSize;
        pieceDatas = new PieceData[NumberOfPieces];
        for (int i = 0; i < NumberOfPieces; i++) //piece index
        {
            pieceDatas[i] = new PieceData();
            pieceSize = 1;

            pieceDatas[i].PieceWidth = data[d];
            pieceSize *= data[d];
            d++;
            pieceDatas[i].PieceHeight = data[d];
            pieceSize *= data[d];
            d++;
            pieceDatas[i].TileType = new int[pieceSize];
            for (int j = 0; j < pieceSize; j++)
            {
                pieceDatas[i].TileType[j] = data[d];
                d++;
            }
        }
    }

    public void LoadLevelData(int num)
    {
        switch (num)
        {
            case 1:
                scaleSize = 1;
                BoardWidth = 3;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 1;
                NumberOfPieces = 3;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray(SetDefaultBoard());
                ConvertStringToPieceInfo("211421131241");
                break;
            case 2:
                scaleSize = 1;
                BoardWidth = 3;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 1;
                NumberOfPieces = 3;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray(SetDefaultBoard());
                ConvertStringToPieceInfo("211821131291");
                break;
            case 3:
                scaleSize = 1;
                BoardWidth = 3;
                BoardHeight = 3;
                BoyPos = 0;
                GirlPos = 2;
                NumberOfPieces = 5;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray(SetDefaultBoard());
                ConvertStringToPieceInfo("22301321141214113115");
                break;
            case 4:
                scaleSize = 2;
                BoardWidth = 4;
                BoardHeight = 4;
                BoyPos = 1;
                GirlPos = 2;
                NumberOfPieces = 7;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray(SetDefaultBoard());
                ConvertStringToPieceInfo("1335122102112131234311522131114");
                break;
            case 5:
                scaleSize = 2;
                BoardWidth = 4;
                BoardHeight = 4;
                BoyPos = 0;
                GirlPos = 3;
                NumberOfPieces = 8;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray(SetDefaultBoard());
                ConvertStringToPieceInfo("12572176117117226477127512461263");
                break;
            case 6:
                scaleSize = 3;
                BoardWidth = 5;
                BoardHeight = 5;
                BoyPos = 1;
                GirlPos = 3;
                NumberOfPieces = 10;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("1111111111116111111111111");
                ConvertStringToPieceInfo("13113224013220112221340215131531121311221141231");
                break;
        }
    }

    public string ReadFileByFactor(float dfactor)
    {
        string path = Application.dataPath + "/"+ dfactor + ".txt";
        if (!File.Exists(path))
        {
            Debug.Log("error");
        }
        string testdata = File.ReadLines(path).Skip(3).First();
        //Debug.Log(testdata);
        return testdata;
    }

    public void GenerateSlicedPieces(string s)
    {
        //Process map string input
        char sp = ' ';
        string[] temp = s.Split(sp);
        int[] t = new int[5];
        for (int i = 0; i < temp.Length-1; i++)//마지막꺼는 너무커서 패스
        {
            t[i] = int.Parse(temp[i]);
        }
        //입력값
        scaleSize = t[0];
        BoardWidth = t[1];
        BoardHeight = t[2];
        BoyPos = t[3];
        GirlPos = t[4];
        string BoardInput = temp[5]; //Board 입력받아오는값
        Debug.Log("BoardInput : " + BoardInput);

        int boardSize = BoardWidth * BoardHeight;
        //Debug.Log("BoardSize : " + boardSize);

        //float difficultyFactor = dfac; // 난이도 조절용 -> (-1 ~ 1) -> 나중에 입력값으로 받음
        float difficultyFactor = Random.Range(-1f, 1f); //임시 Random 값

        //조각의 최대 크기(최대 타일 갯수) : boardSize = 4 -> 2, boardSize = 16 -> 4 // 수정 및 조절 필요
        int maxPieceSize = Mathf.FloorToInt(Mathf.Sqrt(boardSize));
        //Debug.Log("MaxPieceSize : " + maxPieceSize);

        //생성할 조각 갯수 : BoardSize * (0.35 ~ 0.6) -> ex) boardSize = 16 -> (5.6 ~ 10.4)개, boardSize = 6 -> (2.1 ~ 3.9)개, difficultyFactor에 따라 범위 안에서 선택 // 수정 및 조절 필요
        int NumberOfPieces = Mathf.Max(Mathf.RoundToInt(boardSize * (0.475f + (0.125f * difficultyFactor))), boardSize / maxPieceSize);
        //Debug.Log("NumberOfPieces : " + NumberOfPieces);

        //조각들 1차원 Array -> 조각 크기 할당용
        int[] pieceSizeArray = new int[NumberOfPieces];

        for (int i = 0; i < NumberOfPieces; i++)
        {
            pieceSizeArray[i] = 1;
        }

        ////////각 조각 갯수 할당 1번째 방법 ex) [boardSize : 9, NumberOfPieces : 5] -> [3,2,2,1,1]
        /*
        int remainingPieces = boardSize - NumberOfPieces; //boardSize piece중 할당하고 난 나머지
        for (int piecesize = 2; piecesize <= maxPieceSize; piecesize++)
        {
            int allocate = Random.Range(DivCeil(remainingPieces, maxPieceSize - (piecesize - 1)), Mathf.Min(remainingPieces, NumberOfPieces) + 1); //int의 '/'연산 잘 작동하는지 검증 필요, Random.Range difficultyFactor의 영향을 받도록 조정 필요
            for(int i = 0; i < allocate; i++)
            {
                pieceSizeArray[i] += 1;
            }
            remainingPieces -= allocate;
        }

        //유틸리티
        // int 두 개 나누고 소수는 올림 ex) [8 / 3 = 2.667 -> 3 return], [6 / 3 = 2 -> 2 return]
        int DivCeil(int a, int b) 
        {
            int c = a % b == 0 ? a / b : (a / b) + 1;
            return c;
        }
        */
        ////////각 조각 갯수 할당 1번째 방법

        ////////각 조각 갯수 할당 2번째 방법 ex) [boardSize : 9, NumberOfPieces : 5] -> [2,2,1,3,1]
        int remainingPieces = boardSize - NumberOfPieces;
        int randomIndex;
        while(remainingPieces != 0) //[1,1,1,1,1]로 시작해서 Random으로 나오는 index의 값에 1씩 더함
        {
            randomIndex = Random.Range(0, NumberOfPieces);
            if(pieceSizeArray[randomIndex] != maxPieceSize) //maxPieceSize 넘어가는 것 방지
            {
                pieceSizeArray[randomIndex] += 1;
            }
            else
            {
                for(int i=0; i<NumberOfPieces; i++)
                {
                    if(pieceSizeArray[i] != maxPieceSize)
                    {
                        pieceSizeArray[i] += 1;
                        break;
                    }
                }
            }
            remainingPieces--;
        }
        ////////각 조각 갯수 할당 2번째 방법

        //조각을 자름
        SliceBoard(BoardInput, pieceSizeArray);

        //TESTING
        for (int i = 0; i < pieceSizeArray.Length; i++)//들어가있는 갯수들이 pieceSizeArray = [2,2,1,3,1]꼴을 뛴다
        {
            Debug.Log("Piece " + i + ": " + pieceSizeArray[i]);
        }
    }

    void SliceBoard(string Board, int[] pieceSizeArray)
    {
        //조각 자르기 시작
        //bool[,] isBoardSelected = new bool[BoardHeight,BoardWidth]; //2차원 배열, 기본적으로 false
        int pieceWidth;
        int pieceHeight;
        //int firstTileX = 0;
        //int firstTileY = 0;
        int remainingTiles;

        //데이터 받아오기
        LoadedMapTile[,] LoadedMap = new LoadedMapTile[BoardHeight, BoardWidth];
        int index = 0; //Temporary Index
        Debug.Log(LoadedMap[0, 0].isBoardSelected);
        /*
        for (int i = 0; i < BoardHeight; i++) 
        {
            for (int j = 0; j < BoardWidth; j++)
            {
                LoadedMap[i, j].LoadedTileCode = Board[index] - '0';
                Debug.Log(LoadedMap[i, j].LoadedTileCode);
                index++;
            }
        }
        */
    }

    string SetDefaultBoard()  //Return Default Board with all standard EmptyTiles
    {
        string board = "";
        for(int i = 0; i < BoardHeight*BoardWidth; i++)
        {
            board += "1";
        }
        return board;
    }

    void CheckNewPieces(int[] pieceinfo)//pieceinfo = stringinfo -> 바꾸는 형식은 PlayerPrefs.SetInt("Piecedata"+i, 0); -> 0이면 false 1이면 True
    {
        int i = 0;
        for (; i < pieceinfo.Length; i++)
        {
            switch (pieceinfo[i])// 그 타일이 나오면 True 값인 1로 만듬 작동 잘됨 전부 1로 나옴. case1은 빈타일이라 필요 없음
            {
                case 2:
                    if (PlayerPrefs.GetInt("Piecedata") < 2)
                    {
                        PlayerPrefs.SetInt("Piecedata", 1);
                        tutorialCase = true;
                    }
                    break;
                case 3:
                    if (PlayerPrefs.GetInt("Piecedata") < 2)
                    {
                        PlayerPrefs.SetInt("Piecedata", 1);
                        tutorialCase = true;
                    }
                    break;
                case 4:
                    if (PlayerPrefs.GetInt("Piecedata") < 2)
                    {
                        PlayerPrefs.SetInt("Piecedata", 1);
                        tutorialCase = true;
                    }
                    break;
                case 5:
                    if (PlayerPrefs.GetInt("Piecedata") < 2)
                    {
                        PlayerPrefs.SetInt("Piecedata", 1);
                        tutorialCase = true;
                    }
                    break;
                case 6:
                    if (PlayerPrefs.GetInt("Piecedata") < 3)
                    {
                        PlayerPrefs.SetInt("Piecedata", 2);
                        tutorialCase = true;
                    }
                    break;
                case 7:
                    if (PlayerPrefs.GetInt("Piecedata") < 3)
                    {
                        PlayerPrefs.SetInt("Piecedata", 2);
                        tutorialCase = true;
                    }
                    break;
                case 8:
                    if (PlayerPrefs.GetInt("Piecedata") < 4)
                    {
                        PlayerPrefs.SetInt("Piecedata", 3);
                        tutorialCase = true;
                    }
                    break;
                case 9:
                    if (PlayerPrefs.GetInt("Piecedata") < 4)
                    {
                        PlayerPrefs.SetInt("Piecedata", 3);
                        tutorialCase = true;
                    }
                    break;
            }
        }
    }
}
