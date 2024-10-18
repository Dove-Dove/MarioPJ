using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinObj : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().CoinGet();
            gameObject.SetActive(false);
        }
    }

}
