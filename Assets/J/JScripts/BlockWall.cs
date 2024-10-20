using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWall : MonoBehaviour
{
    public bool WallOpen = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingShell")
            WallOpen = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        WallOpen = false;
    }
}
