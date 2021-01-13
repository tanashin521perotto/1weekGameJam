﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public StageManager stage = default;

    public Sprite[] sprites;
    SpriteRenderer sr;
    bool isClear = false;
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        Debug.Log(sr.sprite);
    }
    void Update()
    {
        if (isClear)
        {
            Debug.Log("クリア");
            sr.sprite = sprites[3];
        }
        else if (Set.SetCount == 0)
        {
            Debug.Log("ゼロ");
            sr.sprite = sprites[0];
        }
        else if(Set.SetCount == 1)
        {
            Debug.Log("イチ");
            sr.sprite = sprites[1];
        }
        else if(Set.SetCount == 2)
        {
            Debug.Log("ニ");
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
                isClear = true;
                Debug.Log("Clear!");
                Invoke("RestartGame", 2f);
            }
        }
    }
    // リスタート処理
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Set.SetCount = 0;
    }
}
