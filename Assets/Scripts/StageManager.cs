using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // タイルの種類
    public enum TILE_TYPE
    {
        WALL,         //0 壁
        GROUND,       //1 地面
        BLOCK_POINT,  //2 ブロックを置くところ
        BLOCK,        //3 移動させるもの
        PLAYER,       //4 プレイヤー
        Goal,         //5 ゴール
        //DOOR, //6 ドア
        //REVOLVING_DOOR, //7 ドア回転部

        BLOCK_ON_POINT,   // プレイヤー（目的地の上）
        PLAYER_ON_POINT,  // ブロック（目的地の上）
        
    }

    public TextAsset stageFile; 　// ステージ構造が記述されたテキストファイル
    public GameObject[] prefabs;  // ゲームオブジェクトをプレハブしリスト化
    public PlayerManager player; // playermanager

    TILE_TYPE[,] tileTable;　　// タイル情報を管理する二次元配列
    float tileSize;　　　　　　// タイルのサイズ
    int BlockCount;　　　　　　// ブロックの数

    Vector2 centerPosition;
    TILE_TYPE tileType;

    public Dictionary<GameObject, Vector2Int> moveObjPositionOnTile = new Dictionary<GameObject, Vector2Int>();

    private void Start()
    {

    }


    // タイルの情報を読み込む
    public void LoadTileData()
    {
        // タイルの列数を計算
        string[] lines = stageFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        // [X] 列の長さを取得(lines[0]で1行取得してその中のsplitを,区切りで分けた場合の長さ)
        int columns = lines[0].Split(new[] { ',' }).Length;
        // [Y] 行の長さを取得
        int rows = lines.Length;
        // tileTableのコード[X,Y] = [列,行]を代入
        tileTable = new TILE_TYPE[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            string[] values = lines[y].Split(new[] { ',' });
            for (int x = 0; x < columns; x++)
            {
                tileTable[x, y] = (TILE_TYPE)int.Parse(values[x]);

            }
        }
    }

    //tileTableを使ってタイルを生成＆配置する
    public void CreateStage()
    {
        tileSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
        centerPosition.x = (tileTable.GetLength(0) / 2) * tileSize;
        centerPosition.y = (tileTable.GetLength(1) / 2) * tileSize;

        for (int y = 0; y < tileTable.GetLength(1); y++)
        {
            for (int x = 0; x < tileTable.GetLength(0); x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                //　groundを予め敷き詰める
                GameObject ground = Instantiate(prefabs[(int)TILE_TYPE.GROUND]);
                ground.transform.position = GetScreenPositionFromTileTable(position);

                //　タイルを配置する⇒この時PLAYERからPlayerManagerを取得する
                TILE_TYPE tileType = tileTable[x, y];
                GameObject obj = Instantiate(prefabs[(int)tileType]);
                obj.transform.position = GetScreenPositionFromTileTable(position);
                if (tileType == TILE_TYPE.PLAYER)
                {
                    player = obj.GetComponent<PlayerManager>();
                    moveObjPositionOnTile.Add(obj, position);
                }
                if (tileType == TILE_TYPE.BLOCK)
                {
                    BlockCount++;
                    moveObjPositionOnTile.Add(obj, position);
                }
                /*
                //回転扉を追加
                if (tileType == TILE_TYPE.REVOLVING_DOOR)
                {
                    moveObjPositionOnTile.Add(obj, position);
                }
                */
            }
        }
    }


    //GetScreenPositionFromTileTable();の関数を使ってタイルを敷き詰めるサイズを算出
    //GetScreenPositionFromTileTableが画面内のサイズの比率を定義している
    //描画の反転を修正するには、y座標を-にすればいい。
    public Vector2 GetScreenPositionFromTileTable(Vector2Int position)
    {
        return new Vector2(position.x * tileSize - centerPosition.x, -(position.y * tileSize - centerPosition.y));
    }
    /*
    // 指定された位置のタイルがDoorなら true を返す
    public bool IsDoor(Vector2Int position)
    {
        if (tileTable[position.x, position.y] == TILE_TYPE.DOOR)
        {
            return true;
        }
        return false;
    }
    // 指定された位置のタイルがRevolving_Doorなら true を返す
    public bool IsRevolvingDoor(Vector2Int position)
    {
        if (tileTable[position.x, position.y] == TILE_TYPE.REVOLVING_DOOR)
        {
            return true;
        }
        return false;
    }
    */
    // 指定された位置のタイルがWallなら true を返す
    public bool IsWall(Vector2Int position)
    {
        if (tileTable[position.x, position.y] == TILE_TYPE.WALL)
        {
            return true;
        }
        return false;
    }

    // 指定された位置のタイルがBlockなら true を返す
    public bool IsBlock(Vector2Int position)
    {
        if (tileTable[position.x, position.y] == TILE_TYPE.BLOCK)
        {
            return true;
        }
        if (tileTable[position.x, position.y] == TILE_TYPE.BLOCK_ON_POINT)
        {
            return true;
        }
        return false;
    }

    // // 指定された位置に存在するゲームオブジェクトを返します
    GameObject GetBlockObjAt(Vector2Int position)
    {
        // Dictionary型の性質を利用
        // pairにはキー(Obj)とvalue(位置)が入っている
        foreach (var pair in moveObjPositionOnTile)
        {
            if (pair.Value == position)
            {
                return pair.Key;
            }
        }
        return null;
    }

    // Blockを移動させる
    public void UpdateBlockPosition(Vector2Int currentBlockPosition, Vector2Int nextBlockPosition)
    {
        // Blockを取得
        GameObject block = GetBlockObjAt(currentBlockPosition);
        // 移動する
        block.transform.position = GetScreenPositionFromTileTable(nextBlockPosition);
        // 位置データを修正、更新する
        moveObjPositionOnTile[block] = nextBlockPosition;

        //tileTableの更新
        //次にBlockが置かれる場所をBLOCKとする(ただし、BLOCK_POINTならBLOCK_ON_POINTにする)
        if (tileTable[nextBlockPosition.x, nextBlockPosition.y] == TILE_TYPE.BLOCK_POINT)
        {
            tileTable[nextBlockPosition.x, nextBlockPosition.y] = TILE_TYPE.BLOCK_ON_POINT;
        }
        else
        {
            tileTable[nextBlockPosition.x, nextBlockPosition.y] = TILE_TYPE.BLOCK;
        }
    }
    /*
    // Doorを移動させる
    public void UpdateDoorPosition(Vector2Int currentDoorPosition, Vector2Int nextDoorPosition)
    {
        // Doorを取得
        GameObject door = GetBlockObjAt(currentDoorPosition);

        // 移動する
        door.transform.position = GetScreenPositionFromTileTable(nextDoorPosition);
        // 位置データを修正、更新する
        moveObjPositionOnTile[door] = nextDoorPosition;

        //tileTableの更新
        //次にDoorが置かれる場所をDoorとする
        
        tileTable[nextDoorPosition.x, nextDoorPosition.y] = TILE_TYPE.REVOLVING_DOOR;
        
    }
    */
    public void UpdateTileTableForPlayer(Vector2Int currentPosition, Vector2Int nextPosition)
    {
        //tileTableの更新
        //次にPlayerが置かれる場所をPLAYERとする(ただし、BLOCK_POINTならPLAYER_ON_POINTにする)
        if (tileTable[nextPosition.x, nextPosition.y] == TILE_TYPE.BLOCK_POINT)
        {
            tileTable[nextPosition.x, nextPosition.y] = TILE_TYPE.PLAYER_ON_POINT;
        }
        else
        {
            tileTable[nextPosition.x, nextPosition.y] = TILE_TYPE.PLAYER;
        }

        //現在の場所をGROUNDとする
        if (tileTable[currentPosition.x, currentPosition.y] == TILE_TYPE.PLAYER_ON_POINT)
        {
            tileTable[currentPosition.x, currentPosition.y] = TILE_TYPE.BLOCK_POINT;
        }
        else
        {
            tileTable[currentPosition.x, currentPosition.y] = TILE_TYPE.GROUND;
        }
    }

    //Blockの数とBLOCK_ON_POINTの数が一致するとクリア！
    public bool IsAllClear()
    {
        int clearCount = 0;
        for (int y = 0; y < tileTable.GetLength(1); y++)
        {
            for (int x = 0; x < tileTable.GetLength(0); x++)
            {
                if (tileTable[x, y] == TILE_TYPE.BLOCK_ON_POINT)
                {
                    clearCount++;
                }
            }
        }
        if (BlockCount == clearCount)
        {
            return true;
        }
        return false;
    }

    /*
    public bool IsBlockTypeOf(TILE_TYPE type, Vector2Int position)
    {
        if (tileTable[position.x, position.y].TILE_TYPE.tileType)
        {
            return true;
        }
        return false;
    }*/

}
