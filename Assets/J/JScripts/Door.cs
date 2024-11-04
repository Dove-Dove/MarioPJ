using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject DoorOut;
    public float moveCam = 0;
    private GameObject cam;
    private GameObject Player;
    private bool PlayerDoor = false;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        Player = GameObject.Find("Mario");
    }

    private void Update()
    {
        PlayerDoor = Player.GetComponent<Player_Move>().inDoor;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player" && PlayerDoor)
        {
            collision.gameObject.transform.position = DoorOut.transform.position;
            cam.GetComponent<CameraController>().inPipe();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player" && PlayerDoor)
        {
            collision.gameObject.transform.position = DoorOut.transform.position;
            cam.GetComponent<CameraController>().inPipe();
        }
    }


}
