using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class underCol : MonoBehaviour
{
    public bool WallUnderOpen = false;
    public bool big = false;
    float time = 0.5f;
    float nowTime = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" 
            || collision.gameObject.tag == "StarInvincible")
        {
            if (GameObject.Find("Mario").GetComponent<Player_Move>().isJump == true||
                GameObject.Find("Mario").GetComponent<Player_Move>().onAir == true)
            {
                WallUnderOpen = true;
                big = GameObject.Find("GameManager").GetComponent<GameManager>().breakBlock;
            }

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        WallUnderOpen = false;
        big = false;
    }
}

