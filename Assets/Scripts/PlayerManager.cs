using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public StageManager stage = default;
    public GameManager gameManager = default;

    bool hasMochi;

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

    public void Move(Vector2 currentPosition, Vector2 position, DIRECTION direction)
    {
        //transform.position = position;
        Vector3 nowPos = new Vector3(currentPosition.x, currentPosition.y, 0);
        Vector3 targetPos = new Vector3(position.x, position.y, 0);
        transform.position = Vector3.Lerp(nowPos, targetPos, Time.deltaTime);
        Debug.Log(direction);
    }

    void MoveTo(DIRECTION direction)
    {
        Vector2Int currentPlayerPositionOnTile = stage.moveObjPositionOnTile[this.gameObject];               // 1.現在の位置を取得
        Vector2Int nextPlayerPositionOnTile = GetNextPositionOnTile(currentPlayerPositionOnTile, direction); // 2.次の位置を取得
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
        stage.UpdateTileTableForPlayer(currentPlayerPositionOnTile, nextPlayerPositionOnTile);
        this.Move(stage.GetScreenPositionFromTileTable(currentPlayerPositionOnTile), stage.GetScreenPositionFromTileTable(nextPlayerPositionOnTile), direction);              // 3.次の位置にプレイヤーを移動
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
}
