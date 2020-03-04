using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNewMap : MonoBehaviour
{
    public int scaleSize;
    public int BoardWidth;
    public int BoardHeight;
    public int BoyPos;
    public int GirlPos;
    public int NumberOfPieces;
    public int[] BoardEmptyTileTypeInfo;
    public PieceData[] pieceDatas;
    public string Newmap;
    timestoper timer;

    public class PieceData
    {
        public int PieceWidth;
        public int PieceHeight;
        public int[] TileType;
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

    public void makeNewlevel()
    {
        BoardWidth = UnityEngine.Random.Range(3, 5);
        BoardHeight = UnityEngine.Random.Range(3, 5);
        int scalesizechanger = 0;
        scalesizechanger = (BoardWidth + BoardHeight) / 2;
        switch(BoardHeight)
        {
            case 1:
            case 2:
            case 3:
                scaleSize = 1;
                break;
            case 4:
                scaleSize = 2;
                break;
            case 5:
                scaleSize = 3;
                break;
        }
        BoyPos = UnityEngine.Random.Range(0, BoardHeight-1);
        GirlPos = UnityEngine.Random.Range(0, BoardHeight-1);
        int tilevalue = 0;
        int rangeoftile = 9;
        bool tile8yes = false;
        while (tile8yes == false)
        {
            for (int i = 0; i < BoardHeight; i++)
                for (int j = 0; j < BoardWidth; j++)
                {
                    tilevalue = UnityEngine.Random.Range(1, rangeoftile);
                    if (tilevalue == 8 || tilevalue == 9)
                    {
                        rangeoftile = 8;
                        if (tilevalue == 8)
                        {
                            if (tile8yes == false)
                            {
                                tilevalue = 9;
                                rangeoftile = 7;
                            }
                            tile8yes = true;
                        }
                        else if(tilevalue == 9)
                        {
                            tile8yes = false;
                        }
                    }
                    Newmap += tilevalue;
                }
        }

        BoardEmptyTileTypeInfo = ConvertStringToIntArray(Newmap); // 이걸 통해 만들어서 확인후에 돌아가는지 확인하고 pieces 나누기로 합시당.
    }

    public void LoadLevelData(int num)
    {
        makeNewlevel();
    }

    string SetDefaultBoard()  //Return Default Board with all standard EmptyTiles
    {
        string board = "";
        for (int i = 0; i < BoardHeight * BoardWidth; i++)
        {
            board += "1";
        }
        return board;
    }
}
