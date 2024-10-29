using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailCollision : MonoBehaviour
{
    public bool tailHIt = false;
    public Vector2 hitPos;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Enemy" || collision.gameObject.tag == "Shell")
        {
            hitPos = collision.transform.position;
            tailHIt = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Shell")
        {
            hitPos = collision.transform.position;
            tailHIt = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        tailHIt = false;
    }
}
