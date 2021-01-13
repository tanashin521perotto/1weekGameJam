using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : MonoBehaviour
{
    StageManager stageManager;
    public static int SetCount = 0;
    bool isSet;

    private void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isSet)
        {
            return;
        }
        if(collision.gameObject.tag == "Block")
        {
            SetCount++;
            isSet = true;
            SoundManager.instance.PlaySE(0);
            GameObject ground = Instantiate(stageManager.prefabs[(int)StageManager.TILE_TYPE.GROUND]);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            SetCount--;
            isSet = false;
            SoundManager.instance.PlaySE(1);
        }
    }
}
