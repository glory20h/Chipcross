using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MakeNewMap : MonoBehaviour
{
    [HideInInspector]
    public int scaleSize;
    [HideInInspector]
    public int BoardWidth;
    [HideInInspector]
    public int BoardHeight;
    [HideInInspector]
    public int BoyPos;
    [HideInInspector]
    public int GirlPos;
    [HideInInspector]
    public int NumberOfPieces;
    [HideInInspector]
    public int[] BoardEmptyTileTypeInfo;
    public PieceData[] pieceDatas;
    [HideInInspector]
    public string Newmap;
    public timestoper timer;
    [HideInInspector]
    public string nevergiveup;
    private moveboy boys;
    string check;
    int Girlposcheck = 10;
    int Boyposcheck = 10;
    int passivemap = 8119;
    int passivemapcheck = 0;
    int[,] passivemapmatrix;
    [HideInInspector]
    public float difficultyFactor = 0;

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
        if(Newmap == null)//이부분 중요했음 ㅋㅋ 기존 맵 Destoryer
        {

        }
        else
        {
            Newmap = null;
        }
        BoardWidth = Random.Range(3, 5);
        BoardHeight = Random.Range(3, 5);
        int scalesizechanger = 0;
        scalesizechanger = (BoardWidth + BoardHeight) / 2;
        switch (BoardHeight)
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
        BoyPos = Random.Range(0, BoardHeight - 1);
        GirlPos = Random.Range(0, BoardHeight - 1);
        int tilevalue = 0;
        int rangeoftile = 9;
        bool tile8yes = true;//tile8yes 없애면 tile9 1개랑 나머지는 tile8개로 나옴
        bool tile9yes = true;
        for (int i = 0; i < BoardHeight; i++)
            for (int j = 0; j < BoardWidth; j++)
            {
                tilevalue = Random.Range(1, rangeoftile);// 1부터
                if (tilevalue == 8 && tile8yes)
                {
                    if (tile9yes)
                    {
                        tilevalue = 9;
                        tile9yes = false;
                    }
                    else
                    {
                        tile8yes = false;
                        rangeoftile = 8;
                    }
                }
                //타일 Exception
                if (j == 0)//<-
                {
                    while (tilevalue == 2)
                    {
                        tilevalue = Random.Range(1, rangeoftile);
                    }
                }
                else if (i == 0)
                {
                    while (tilevalue == 5)
                    {
                        tilevalue = Random.Range(1, rangeoftile);
                    }
                }
                else if (j == BoardHeight - 1)
                {
                    while (tilevalue == 4)
                    {
                        tilevalue = Random.Range(1, rangeoftile);
                    }
                }
                else if (j == GirlPos && i == BoardWidth - 1)
                {
                    while (tilevalue == 2)
                    {
                        tilevalue = Random.Range(1, rangeoftile);
                    }
                }
                Newmap += tilevalue;
            }
        BoardEmptyTileTypeInfo = ConvertStringToIntArray(Newmap); // 이걸 통해 만들어서 확인후에 돌아가는지 확인하고 pieces 나누기로 합시당.
    }

    public void LoadLevelData(int num)
    {
        /*switch (num)
        {
            case 1:
            scaleSize = 1;
            BoardWidth = 3;
            BoardHeight = 2;
            BoyPos = 0;
            GirlPos = 0;
            NumberOfPieces = 0;
            Newmap = "333333";
            BoardEmptyTileTypeInfo = ConvertStringToIntArray(Newmap);
            break;
            case 2:
                makeNewlevel();
            break;
        }*/
        //makeNewlevel();
        /*passivemaker();
        Checkingsame();*/

        Mapmakingbydfactor(-1);
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

    void Checkingsame()
    {
        if (check == Newmap)
        {
            if (GirlPos == Girlposcheck && BoyPos == Boyposcheck)
                LoadLevelData(0);
        }
        else
        {
            check = Newmap;
            Girlposcheck = GirlPos;
            Boyposcheck = BoyPos;
        }
    }

    void passivemaker()
    {
        float difficultyFactor = Random.Range(-1, 1);

        if (Newmap == null)//이부분 중요했음 ㅋㅋ 기존 맵 Destoryer
        {

        }
        else
        {
            Newmap = null;
        }

        if (passivemapcheck == passivemap)
            passivemap++;

        BoardWidth = 2; // 2*2타일같이 여기서 수정함
        BoardHeight = 2;
        scaleSize = 1;
        //여자애 남자애 포지션
        BoyPos = 0;
        GirlPos = 0;

        mapmatrix();

        //Debug.Log(passivemap);

        Newmap += passivemap;// 이 증가의 형태를 설명한다!
        /* 0 0  -> 0 0 -> 0 0 -> 0 1
           0 0     0 1    1 0    1 1 */

        passivemapcheck = passivemap;
        passivemap++;
        /* 다음에 할거
         Pin = 0000
print (Pin)
while Pin < 10000:
    print (Pin)
    Pin = Pin + 1
         */
        BoardEmptyTileTypeInfo = ConvertStringToIntArray(Newmap);
    }

    void mapmatrix()
    {
        //difficultyFactor가 -1~1 -> size스케일러
        int sizescaler = 0;
        sizescaler = (passivemapmatrix.GetLength(0) + passivemapmatrix.GetLength(1))/2;
        //ez case check
        switch(sizescaler)
        {
            case 1:
                difficultyFactor = -1;
                break;
            case 2:
                difficultyFactor = -0.7f;
                break;
            case 3:
                difficultyFactor = -0.2f;
                break;
            case 4:
                difficultyFactor = 0.2f;
                break;
            case 5:
                difficultyFactor = 0.7f;
                break;
        }

        passivemapmatrix[0, 0] = passivemap / 1000;
        passivemapmatrix[0, 1] = (passivemap % 1000) / 100;
        passivemapmatrix[1, 0] = (passivemap % 100) / 10;
        passivemapmatrix[1, 1] = passivemap % 10;


        /*Debug.Log("00:"+passivemapmatrix[0, 0]);
        Debug.Log("01:" + passivemapmatrix[0, 1]);
        Debug.Log("10:" + passivemapmatrix[1, 0]);
        Debug.Log("11:" + passivemapmatrix[1, 1]);*/

        int checkmatrixnum8 = 0;
        int checkmatrixnum9 = 0;
        //1천대
        if (passivemapmatrix[0, 0] == 0)
        {
            passivemapmatrix[0, 0] += 1;
        }
        else if(passivemapmatrix[0, 0] == 8)
        {
            checkmatrixnum8++;
        }
        else if (passivemapmatrix[0, 0] == 9)
        {
            checkmatrixnum9++;
        }
        else if (passivemapmatrix[0, 0] == 6 || passivemapmatrix[0, 0] == 7)
        {
            difficultyFactor += 0.1f;
        }

        //Debug.Log("00:" + passivemapmatrix[0, 0]);

        //1백대
        if (passivemapmatrix[0, 1] == 0)
        {
            passivemapmatrix[0, 1] += 1;
        }
        else if (passivemapmatrix[0, 1] == 8)
        {
            checkmatrixnum8++;
        }
        else if (passivemapmatrix[0, 1] == 9)
        {
            checkmatrixnum9++;
        }
        else if (passivemapmatrix[0, 1] == 6 || passivemapmatrix[0, 1] == 7)
        {
            difficultyFactor += 0.1f;
        }

        //Debug.Log("01:" + passivemapmatrix[0, 1]);

        //10대
        if (passivemapmatrix[1, 0] == 0)
        {
            passivemapmatrix[1, 0] += 1;
        }
        else if (passivemapmatrix[1, 0] == 8)
        {
            checkmatrixnum8++;
        }
        else if (passivemapmatrix[1, 0] == 9)
        {
            checkmatrixnum9++;
        }
        else if (passivemapmatrix[1, 0] == 6 || passivemapmatrix[1, 0] == 7)
        {
            difficultyFactor += 0.1f;
        }

        //Debug.Log("10:" + passivemapmatrix[1, 0]);

        //1대
        if (passivemapmatrix[1, 1] == 0)
        {
            passivemapmatrix[1, 1] += 1;
        }
        else if (passivemapmatrix[1, 1] == 8)
        {
            checkmatrixnum8++;
        }
        else if (passivemapmatrix[1, 1] == 9)
        {
            checkmatrixnum9++;
        }
        else if (passivemapmatrix[1, 1] == 6 || passivemapmatrix[1, 1] == 7)
        {
            difficultyFactor += 0.1f;
        }

        //Debug.Log("11:" + passivemapmatrix[1, 1]);


        // 확인
        //Debug.Log("8:"+ checkmatrixnum8);
        //Debug.Log("9:" + checkmatrixnum9);
        if ((checkmatrixnum8 == 0 && checkmatrixnum9 == 0) || (checkmatrixnum8 == 1 && checkmatrixnum9==1))
        {
            Debug.Log("ok");
            difficultyFactor += 0.05f;
        }
        else
        {
            Debug.Log("notok");
            passivemap++;
            mapmatrix();
        }
        
        if (passivemapmatrix[0, 0] == 2 || passivemapmatrix[0, 0] == 6 || passivemapmatrix[0, 0] == 5)
            passivemapmatrix[0, 0] += 1;
        if (passivemapmatrix[0, 1] == 2 || passivemapmatrix[0, 1] == 6 || passivemapmatrix[0, 1] == 5)
            passivemapmatrix[0, 1] += 1;
        if (passivemapmatrix[1, 0]== 4 || passivemapmatrix[1, 0] == 7)
            passivemapmatrix[1, 0] += 1;
        if (passivemapmatrix[1, 1] == 4 || passivemapmatrix[1, 1] == 7)
            passivemapmatrix[1, 1] += 1;

        passivemap = passivemapmatrix[0, 0] * 1000 + passivemapmatrix[0, 1] * 100 + passivemapmatrix[1, 0] * 10 + passivemapmatrix[1, 1];

        if (difficultyFactor<-1)
        {
            difficultyFactor = -1;
        }
        else if(difficultyFactor>1)
        {
            difficultyFactor = 1;
        }
        /*
        if (난이도 >= 0.5)
        {
            워프 타일 X
            6,7번 타일 X
            공백 타일 갯수 = 난이도 * 0.5f
        }
        */
    }

    void Mapmakingbydfactor(float dfactor)
    {
        float levelfactor = 0;
        var listoftile = new ArrayList();

        if (dfactor<-1)
        {
            dfactor = -1;
        }
        else if(dfactor>1)
        {
            dfactor = 1;
        }
        //여기까지는 일단 초기 설정
        //2+2 = 4. -1~?, 2+3=5, 3+3=6, 4+4=8,5+5=10 즉 4~10 7단계 를 21로 나누니까 0.3기준으로 factor 형성
        int sum = 0;
        if(dfactor>=-1f || dfactor< -0.8f)//-1,-0.9
        {
            sum = 4;
            levelfactor = -1f;
        }
        else if(dfactor >= -0.8f || dfactor < -0.5f)//0.7,0.6,0.5
        {
            sum = 5;
            levelfactor = -0.8f;
        }
        else if (dfactor >= -0.5f || dfactor < -0.2f)//0.4,0.3,0.2
        {
            sum = 6;
            levelfactor = -0.5f;
        }
        else if (dfactor >= -0.2f || dfactor < 0.1f)//0.1,0.0,0.1
        {
            sum = 7;
            levelfactor = -0.2f;
        }
        else if (dfactor >= 0.1f || dfactor < 0.4f)//0.2,0.3,0.4
        {
            sum = 8;
            levelfactor = 0.1f;
        }
        else if (dfactor >= 0.4f || dfactor < 0.7f)//0.5,0.6,0.7
        {
            sum = 9;
            levelfactor = 0.4f;
        }
        else if (dfactor >= 0.7f || dfactor <= 1f)//0.8,0.9,1.0
        {
            sum = 10;
            levelfactor = 0.7f;
        }
        //sum 분리하기
        BoardWidth = Random.Range(2, sum);
        BoardHeight = sum - BoardWidth;
        passivemapmatrix = new int[BoardHeight, BoardWidth];//여기까지 dfactor에 따른 틀을 만드는 거고 내부를 어떻게할까...

        //Girlpos, Boypos
        BoyPos = Random.Range(0, BoardHeight - 1);
        GirlPos = Random.Range(0, BoardHeight - 1);

        //scalesizechanger
        switch (sum/2)
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

        int Big = 0;
        if(BoardWidth > BoardHeight)
        {
            Big = BoardWidth;
        }
        else
        {
            Big = BoardHeight;
        }


        //여기까지가 초기 설정값이다.

        /*2*2 -> boypos height 
        2*3 -> 차원축소해도 된다 -2*2
        3*3 -> 3*4이상의차원으로가니까 안되더라고 차원축소가 x*/
        //dfactor에 감소가 없는 것 -> row나 col중에서 큰 숫자만큼 타일이 있을 경우 1번인 공백타일 빼고
        //그 중에서 6,7번은 0.05의 난이도를 올리고, 8,9가 같이 있다면 0.03의 난이도를 올린다.
        //row =3인데 2~5의 타일이 4개라묜 난이도를 0.02를 올린다

        levelfactor = dfactor - levelfactor; // 남아있는 levelconstruction value
        int Tilerange = 5;
        if(levelfactor>=0.1)
        {
            Tilerange = 9;
        }
        //listoftil 만든다.
        //이걸 다시 배정하는것인데 흠
        //Random으로하지말고 Boy하고 Gril pos의 차이에 따라 우리가 줘야될것은 아래로 가는거랑 옆으로 가는거
        if(GirlPos>BoyPos)
        {
            listoftile.Add(3);
            listoftile.Add(5);
            Big = Big - 2;
        }
        else if(GirlPos < BoyPos)
        {
            listoftile.Add(3);
            listoftile.Add(4);
            Big = Big - 2;
        }

        //남은 Big
        for (int i = 0; i < Big; i++)
        {
            listoftile.Add(Random.Range(2, 9));
        }

        listoftile.Clear();
    }

}
