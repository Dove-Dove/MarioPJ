using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinObj : MonoBehaviour
{
    public AudioSource getCoinSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            getCoinSound.Play();
            GameObject.Find("GameManager").GetComponent<GameManager>().GetCoin();
            gameObject.SetActive(false);
        }
    }

}
