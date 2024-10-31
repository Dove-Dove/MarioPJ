using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamStop : MonoBehaviour
{
    private GameObject Cam;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        Cam = GameObject.Find("Main Camera");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Cam.GetComponent<CameraController>().deadCam();
            gameManager.BossMusicStart();
            Destroy(gameObject);
        }
    }
}
