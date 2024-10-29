using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class deadZone : MonoBehaviour
{
    private GameObject cam;
    public bool playerRender = false;
    private void Start()
    {
        cam = GameObject.Find("MainCamera");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cam.GetComponent<CameraController>().deadCam();
            if(!playerRender)
                GameObject.Find("Mario").GetComponent<SpriteRenderer>().enabled = false;
            else
                GameObject.Find("Mario").GetComponent<SpriteRenderer>().enabled = true;

            GameObject.Find("Mario").GetComponent<Player_Move>().setMarioStatus(MarioStatus.Death);
        }
        else
            Destroy(collision.gameObject);
    }
}
