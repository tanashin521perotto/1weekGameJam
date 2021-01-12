using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set : MonoBehaviour
{
    public static int SetCount = 0;
    bool isSet;
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
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            SetCount--;
            isSet = false;
        }
    }
}
