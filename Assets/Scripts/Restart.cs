using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public StageManager stage = default;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("接触");
        Debug.Log("Set.SetCount:"+Set.SetCount);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject.tag);
            if (Set.SetCount == 2)
            {
                Debug.Log("Clear!");
                Set.SetCount = 0;
                Invoke("RestartGame", 1f);
            }
        }
    }
    // リスタート処理
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
