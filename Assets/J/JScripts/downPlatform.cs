using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downPlatform : MonoBehaviour
{

    private Vector2 StartPos;
    private Vector2 endPos;
    private float playerDis = 0.0f;
    private GameObject Player;
    
    //X 혹은 Y의 이동 거리

    public float PlatformSpeed = 1.5f;

    private bool back = false;

    private void Start()
    {
        StartPos = transform.position;
        Player = GameObject.Find("Mario");
    }

    private void Update()
    {
        playerDis = Player.GetComponent<Transform>().transform.position.x;
        
        if((StartPos.x - playerDis) <= 12.0f)
            transform.Translate(Vector2.left * Time.deltaTime * PlatformSpeed);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            PlatformSpeed = 0;
        }
    }
}
