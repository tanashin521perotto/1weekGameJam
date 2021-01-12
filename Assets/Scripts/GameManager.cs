using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DIRECTION
{
    UP,
    DOWN,
    LEFT,
    RIGHT,

}

public class GameManager : MonoBehaviour
{
    
    public StageManager stage = default;
    bool isClear;

    void Start()
    {
        stage.LoadTileData();
        stage.CreateStage();
    }

    // ゲームクリア処理
    public void CheckAllClear()
    {
        if (isClear)
        {
            return;
        }
        if (stage.IsAllClear())
        {
            //ゲーム終了　⇒リスタート
            isClear = true;
            //Invoke("Restart", 1f);
        }
    }

    // リスタート処理
    void Restart()
    {
        isClear = false;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

}
