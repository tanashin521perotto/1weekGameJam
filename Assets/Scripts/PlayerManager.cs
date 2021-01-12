using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public StageManager stage = default;
    public GameManager gameManager = default;

    bool hasMochi;
    public static bool atDoor = false;
    public int mochiCount;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stage = GameObject.Find("StageManager").GetComponent<StageManager>();

        hasMochi = false;
    }

    private void Update()
    {
        if (InputDirection() == true)
        {
            if (hasMochi == false)
            {
                Debug.Log("もちを取得");
                mochiCount++;
                if (mochiCount > 3)
                {
                    hasMochi = true;
                }
            }
            else if (hasMochi == true)
            {
                Debug.Log("ゴール");
                gameManager.CheckAllClear();
            }

        }

        gameManager.CheckAllClear();

    }

    bool InputDirection()
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
        return false;

    }

    public void Move(Vector2 position, DIRECTION direction)
    {
        transform.position = position;
    }

    void MoveTo(DIRECTION direction)
    {
        Vector2Int currentPlayerPositionOnTile = stage.moveObjPositionOnTile[this.gameObject];               // 1.現在の位置を取得
        Vector2Int nextPlayerPositionOnTile = GetNextPositionOnTile(currentPlayerPositionOnTile, direction); // 2.次の位置を取得
        /*
        //Playerの移動先がDOORのとき
        if (stage.IsDoor(nextPlayerPositionOnTile))
        {
            return;//処理をここで終了させる
        }
        //Playerの移動先がREVOLVING_DOORのとき
        if (stage.IsRevolvingDoor(nextPlayerPositionOnTile))
        {
            Vector2Int nextDoorPositionOnTile = GetNextPositionDoor(nextPlayerPositionOnTile, direction);
            //doorの移動先がWALLもしくはBLOCKのとき
            if (stage.IsWall(nextDoorPositionOnTile) || stage.IsBlock(nextDoorPositionOnTile))
            {
                return;
            }
            stage.UpdateDoorPosition(nextPlayerPositionOnTile, nextDoorPositionOnTile);

        }
        */
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
            if (stage.IsWall(nextBlockPositionOnTile) || stage.IsBlock(nextBlockPositionOnTile))
            {
                return;
            }
            stage.UpdateBlockPosition(nextPlayerPositionOnTile, nextBlockPositionOnTile);
        }
        /*
        //蝶番と接触時
        if (DoorManager.atHinge)
        {
            return;
        }
        */
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
    /*
    Vector2Int GetNextPositionDoor(Vector2Int currentPosition, DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                return currentPosition + new Vector2Int(-1,-1);
            case DIRECTION.DOWN:
                return currentPosition + new Vector2Int(-1, 1);
        }
        return currentPosition;
    }
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            atDoor = true;
        }
    }
}
