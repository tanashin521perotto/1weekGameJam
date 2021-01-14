using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public static SpriteRenderer playerSprite;
    public StageManager stage = default;
    
    public Sprite[] sprites;　//ゴールの画像
    SpriteRenderer sr;
    public static bool isClear = false;
    public static bool isFinish = false;
    void Start()
    {
        if (isClear)
        {
            //playerSprite.enabled = true;
            isClear = false;
        }
        playerSprite = GameObject.Find("Player_02(Clone)").GetComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        //Debug.Log(sr.sprite);
    }
    void Update()
    {
        if (isClear)
        {
         //   Debug.Log("クリア");
            sr.sprite = sprites[3];
        }
        else if (Set.SetCount == 0)
        {
//            Debug.Log("ゼロ");
            sr.sprite = sprites[0];
        }
        else if(Set.SetCount == 1)
        {
      //      Debug.Log("イチ");
            sr.sprite = sprites[1];
        }
        else if(Set.SetCount == 2)
        {
        //    Debug.Log("ニ");
            sr.sprite = sprites[2];
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("接触");
        Debug.Log("Set.SetCount:"+Set.SetCount);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject.tag);
            if (Set.SetCount == 2)
            {
                SoundManager.instance.PlaySE(2);
                isClear = true;
                playerSprite.enabled = false;
                if (GameManager.num < StageManager.maxStage - 1)
                {
                    Invoke("RestartGame", 2f);
                    GameManager.num++;

                    Debug.Log(GameManager.num);
                }
                else if (GameManager.num == StageManager.maxStage - 1)
                {
                    isFinish = true;
                    SceneManager.LoadScene("Clear");
                    Set.SetCount = 0;
                    GameManager.num = 0;
                }
            }
        }
    }
    // リスタート処理
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Set.SetCount = 0;
        //playerSprite.sprite = sprites[4];
        //
    }
}
