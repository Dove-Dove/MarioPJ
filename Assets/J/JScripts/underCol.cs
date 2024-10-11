using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class underCol : MonoBehaviour
{
    public bool WallUnderOpen = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GameObject.Find("Mario").GetComponent<Player_Move>().onAir == true)
            {
                WallUnderOpen = true;
            }

                
        }


    }
}

