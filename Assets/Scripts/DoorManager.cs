using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    GameObject player;
    GameObject parent;
    public bool isRight = true;
    private void Start()
    {
        parent = transform.parent.gameObject;
        player = GameObject.Find("Player(Clone)");
        Debug.Log(player);
    }
    private void Update()
    {
        //playerを取得
        if(player == null)
        {
            player = GameObject.Find("Player(Clone)");
            Debug.Log(player);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision!");

        //Playerと接触したら
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("接触！");
            StartCoroutine("revolvingDoor");
        }
    }
    IEnumerable revolvingDoor()
    {
        Debug.Log("コルーチン！");
        int x = 1;
        if (!isRight)
        {
            x *= -1;
        }
        parent.transform.Rotate(0, 0, 20*x);
        yield return new WaitForSeconds(2f);
        parent.transform.Rotate(0, 0, 20*x);
        yield return new WaitForSeconds(2f);
        parent.transform.Rotate(0, 0, 20*x);
    }
}
