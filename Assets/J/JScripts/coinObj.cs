using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinObj : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 20)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GetCoin();
        }
    }

}
