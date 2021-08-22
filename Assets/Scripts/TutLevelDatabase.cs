using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutLevelDatabase
{
    [HideInInspector] public int scaleSize;
    [HideInInspector] public int BoardWidth;
    [HideInInspector] public int BoardHeight;
    [HideInInspector] public int BoyPos;
    [HideInInspector] public int GirlPos;
    [HideInInspector] public int NumberOfPieces;
    [HideInInspector] public int[] BoardEmptyTileTypeInfo;
    [HideInInspector] public List<PieceData> pieceDatas;

    [HideInInspector] public float piecePlaceXMax = 8.5f;
    [HideInInspector] public float piecePlaceXMin = 4.6f;
    [HideInInspector] public float piecePlaceYMax = 6.8f;
    [HideInInspector] public float piecePlaceYMin = -0.4f;

    public class PieceData
    {
        public int PieceWidth;
        public int PieceHeight;
        public List<int> TileType; //Array -> List migration
        public Vector3 solutionLoc; //solution for hint
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
        int d = 0;
        int pieceSize;
        //pieceDatas = new PieceData[NumberOfPieces]; //Array -> List migration
        pieceDatas = new List<PieceData>(); //Array -> List migration
        for (int i = 0; i < NumberOfPieces; i++) //piece index
        {
            //pieceDatas[i] = new PieceData(); //Array -> List migration
            pieceDatas.Add(new PieceData()); //Array -> List migration
            pieceSize = 1;

            pieceDatas[i].PieceWidth = data[d];
            pieceSize *= data[d];
            d++;
            pieceDatas[i].PieceHeight = data[d];
            pieceSize *= data[d];
            d++;
            //pieceDatas[i].TileType = new int[pieceSize]; //Array -> List migration
            pieceDatas[i].TileType = new List<int>();
            for (int j = 0; j < pieceSize; j++)
            {
                pieceDatas[i].TileType.Add(data[d]); //Array -> List migration
                //pieceDatas[i].TileType[j] = data[d]; //Array -> List migration
                d++;
            }
        }
    }

    public void LoadTutorialData(int num)
    {
        switch (num)
        {
            case 1://empty
                scaleSize = 1;
                BoardWidth = 2;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 0;
                NumberOfPieces = 1;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("3333");
                //ConvertStringToPieceInfo("111");
                break;
            case 2://empty ->
                scaleSize = 1;
                BoardWidth = 2;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 0;
                NumberOfPieces = 1;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("4315");
                ConvertStringToPieceInfo("113");
                break;
            case 3://empty 아래
                scaleSize = 1;
                BoardWidth = 3;
                BoardHeight = 3;
                BoyPos = 0;
                GirlPos = 1;
                NumberOfPieces = 2;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("135133135");
                ConvertStringToPieceInfo("12411131241");
                break;
            case 4://empty 위
                scaleSize = 1;
                BoardWidth = 2;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 0;
                NumberOfPieces = 1;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("4331");
                ConvertStringToPieceInfo("115");
                break;
            case 5://back 넘기고 6번타일
                scaleSize = 1;
                BoardWidth = 2;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 0;
                NumberOfPieces = 1;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("4315");
                ConvertStringToPieceInfo("116");
                break;
            case 6://empty 7번타일
                scaleSize = 1;
                BoardWidth = 2;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 0;
                NumberOfPieces = 1;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("4135");
                ConvertStringToPieceInfo("117");
                break;
            case 7://empty 8번
                scaleSize = 1;
                BoardWidth = 2;
                BoardHeight = 2;
                BoyPos = 0;
                GirlPos = 0;
                NumberOfPieces = 1;
                BoardEmptyTileTypeInfo = ConvertStringToIntArray("1395");
                ConvertStringToPieceInfo("118");
                break;
            default:
                break;
        }
    }
}
