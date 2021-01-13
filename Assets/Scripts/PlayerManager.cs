using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public StageManager stage = default;
    public GameManager gameManager = default;
    public Restart rs;
    public Vector2Int defaultPosition; //現在の位置情報

    bool hasMochi;

    public int mochiCount;

    public static Vector2Int currentPlayerPositionOnTile;               // 1.現在の位置を取得
    public static Vector2Int nextPlayerPositionOnTile; // 2.次の位置を取得


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stage = GameObject.Find("StageManager").GetComponent<StageManager>();
        rs = GameObject.Find("Goal(Clone)").GetComponent<Restart>();
        hasMochi = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTo(DIRECTION.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTo(DIRECTION.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTo(DIRECTION.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTo(DIRECTION.RIGHT);
        }

        gameManager.CheckAllClear();
        if (Input.GetKeyDown(KeyCode.R))
        {
            rs.RestartGame();
        }
        
    }

   

    public void Move(Vector2 position, DIRECTION direction)
    {
        transform.position = position;
    }

    void MoveTo(DIRECTION direction)
    {
        currentPlayerPositionOnTile = stage.moveObjPositionOnTile[this.gameObject];               // 1.現在の位置を取得
        nextPlayerPositionOnTile = GetNextPositionOnTile(currentPlayerPositionOnTile, direction); // 2.次の位置を取得
        //クリアした時
        if (Restart.isClear)
        {
            return;
        }
        //Playerの移動先がDOORのとき
        if (stage.IsDoor(nextPlayerPositionOnTile))
        {
            return;//処理をここで終了させる
        }
        //Playerの移動先がREVOLVING_DOORのとき
        if (stage.IsRevolvingDoor(nextPlayerPositionOnTile))
        {
            Vector2Int nextDoorPositionOnTile = GetNextPositionDoor(nextPlayerPositionOnTile, direction, currentPlayerPositionOnTile);
            //doorの移動先がWALLもしくはBLOCKのとき
            if (stage.IsWall(nextDoorPositionOnTile) || stage.IsBlock(nextDoorPositionOnTile) || stage.IsDoor(nextDoorPositionOnTile))
            {
                return;
            }
            stage.UpdateRevolvingDoorPosition(nextPlayerPositionOnTile, nextDoorPositionOnTile);

        }
        //Playerの移動先がWALLのとき
        if (stage.IsWall(nextPlayerPositionOnTile))
        {
            return;//処理をここで終了させる(下の処理を行わない)壁があるならここで終了
        }
        //Playerの移動先がBLOCKのとき
        if (stage.IsBlock(nextPlayerPositionOnTile))
        {
            Vector2Int nextBlockPositionOnTile = GetNextPositionOnTile(nextPlayerPositionOnTile, direction);
            //Blockの移動先がWALLもしくはBLOCKのとき
            if (stage.IsWall(nextBlockPositionOnTile) || stage.IsBlock(nextBlockPositionOnTile) || stage.IsRevolvingDoor(nextBlockPositionOnTile) || stage.IsDoor(nextBlockPositionOnTile))
            {
                return;
            }
            stage.UpdateBlockPosition(nextPlayerPositionOnTile, nextBlockPositionOnTile);
        }
        

        stage.UpdateTileTableForPlayer(currentPlayerPositionOnTile, nextPlayerPositionOnTile);
        this.Move(stage.GetScreenPositionFromTileTable(nextPlayerPositionOnTile), direction);              // 3.次の位置にプレイヤーを移動
        stage.moveObjPositionOnTile[this.gameObject] = nextPlayerPositionOnTile;                           // 4.タイル情報も更新
    }

    Vector2Int GetNextPositionOnTile(Vector2Int currentPosition, DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                return currentPosition + Vector2Int.down;
            case DIRECTION.DOWN:
                return currentPosition + Vector2Int.up;
            case DIRECTION.LEFT:
                return currentPosition + Vector2Int.left;
            case DIRECTION.RIGHT:
                return currentPosition + Vector2Int.right;
        }
        return currentPosition;
    }
    Vector2Int GetNextPositionDoor(Vector2Int currentPosition, DIRECTION direction, Vector2Int player)
    {
        Debug.Log("GetNextPositionDoor:" + direction);
        Vector2Int center = stage.GetDoorCenter(currentPosition);
        float angle = Vector2.SignedAngle(currentPosition - center, player - center);
        if (angle > 0)
        {
            switch (direction)
            {
                case DIRECTION.UP:
                    return currentPosition + new Vector2Int(-1, -1);
                case DIRECTION.DOWN:
                    return currentPosition + new Vector2Int(1, 1);
                case DIRECTION.LEFT:
                    return currentPosition + new Vector2Int(-1, 1);
                case DIRECTION.RIGHT:
                    return currentPosition + new Vector2Int(1, -1);
            }
        }
        else if (angle < 0)
        {
            switch (direction)
            {
                case DIRECTION.UP:
                    return currentPosition + new Vector2Int(1, -1);
                case DIRECTION.DOWN:
                    return currentPosition + new Vector2Int(-1, 1);
                case DIRECTION.LEFT:
                    return currentPosition + new Vector2Int(-1, -1);
                case DIRECTION.RIGHT:
                    return currentPosition + new Vector2Int(1, 1);
            }
        }
        else
        {
            return center;
            //Debug.Log("なんとかする");
        }
        return currentPosition;
    }
}
