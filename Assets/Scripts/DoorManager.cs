using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    GameObject player;
    GameObject parent;
    public bool isRight = true;
    public bool isHinge = false;
    public static bool isBlockAtDoor = false;
    public static bool atHinge = false;
    private void Start()
    {
        parent = transform.parent.gameObject;
        player = GameObject.Find("Player_02(Clone)");
        Debug.Log(player);
    }
    private void Update()
    {
        //playerを取得
        if(player == null)
        {
            player = GameObject.Find("Player_02(Clone)");
            Debug.Log(player);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");

        //壁やブロックと接触
        if (collision.gameObject.tag == "Block")
        {
            //isBlockAtDoor = true;
            return;
        }
        //Playerと接触したら
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("接触！");
            StartCoroutine(revolvingDoor());
        }
        if (isHinge)
        {
            atHinge = true;
            return;
        }
    }
    //回転ドアが９０度回転
    IEnumerator revolvingDoor()
    {
        
            Debug.Log("コルーチン！");
            int x = 1;
            if (!isRight)
            {
                x *= -1;
            }
            
            parent.transform.Rotate(0, 0, 90*x);
            yield return new WaitForSeconds(1f);
            Debug.Log("回転１");
            //isTurn = true;
            PlayerManager.atDoor = false;
                //float angle = Mathf.LerpAngle(0.0f, 90.0f * x, 1.0f);
                //parent.transform.eulerAngles = new Vector3(0, 0, angle);

                //Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
                //rb.angularVelocity = 90f * x;
        
    }
}
