using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    protected Vector2 originPos;
    protected Vector2 downPos;

    public float moveCooldown = 3f;
    public float nextMoveTime;

    public int speed = 2;
    bool inRange;

    void Start()
    {
        inRange = false;
        originPos = transform.position;
        downPos = new Vector2(transform.position.x, transform.position.y - 8);

        
    }

    void Update()
    {
        if(Time.time >= nextMoveTime)
        {
            Movedown();
            nextMoveTime = Time.time + moveCooldown;
            //Invoke("MoveUp", 3.0f);
        }
    }

    public void Movedown()
    {
        //Debug.Log("Test");
        transform.position = Vector3.MoveTowards(transform.position, downPos, speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        //Debug.Log("Test2");
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        transform.position = Vector3.MoveTowards(transform.position, originPos, 5.0f * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyWall"))
        {
            MoveUp();
        }
    }

}
