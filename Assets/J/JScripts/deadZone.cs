using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class deadZone : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.Find("Mario").GetComponent<Player_Move>().setMarioStatus(MarioStatus.Death);
        }
        else
            collision.gameObject.SetActive(false);
    }
}
