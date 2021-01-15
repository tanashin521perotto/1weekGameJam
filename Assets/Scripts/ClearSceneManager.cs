using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearSceneManager : MonoBehaviour
{
    
    void Start()
    {
        GameObject player = GameObject.Find("Player_02");
        GameObject kagamimoti = GameObject.Find("kagamimoti");
        Text tx = GameObject.Find("Text").GetComponent<Text>();
        if (Restart.isFinish)
        {
            tx.text = "ありがとう!!!";
            player.SetActive(false);
            kagamimoti.SetActive(true);
            SoundManager.instance.PlaySE(3);
        }
        else
        {
            tx.text = "かがみもちをつくって！";
            player.SetActive(true);
            kagamimoti.SetActive(false);
            SoundManager.instance.PlaySE(4);
        }
        
    }

    
    void Update()
    {
        if (Input.anyKey)
        {
            if (Restart.isFinish)
            {
                //Restart.isFinish = false;
                //SceneManager.LoadScene("Clear");
                return;
            }
            else
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}
