using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject DoorOut;
    private GameObject cam;
    private GameObject Player;
    private bool PlayerDoor = false;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player" && PlayerDoor)
        {
            collision.gameObject.transform.position = DoorOut.transform.position;
            //cam.GetComponent<Transform>().transform.position.y = DoorOut.transform.position.y;
        }
    }

}
