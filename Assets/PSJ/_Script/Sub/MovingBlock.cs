using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    protected Vector2 originPos; 
    protected Vector2 downPos;

    private float moveCooldown = 3f;
    private float stayDuration = 1f;
    private float speed = 8f;

    private bool isMovingDown = true;
    private bool isMovingUp = false;

    private Rigidbody2D rb;

    private GameObject player;

    void Start()
    {
        originPos = transform.position;
        downPos = new Vector2(transform.position.x, transform.position.y - 8);
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.Find("Mario");
        rb.isKinematic = true;
    }

    void Update()
    {
        if(player !=null && player.transform.position.y < -15f)
        {
            if (isMovingDown)
            {
                Invoke("Movedown", 1.5f);
                //Movedown();
            }
            else if (isMovingUp)
            {
                MoveUp();
            }
        }
    }

    public void Movedown()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, downPos, speed * Time.deltaTime));

        if (Vector2.Distance(transform.position, downPos) < 0.1f)
        {
            isMovingDown = false;
            StartCoroutine(WaitAndMoveUp());
        }
    }

    public void MoveUp()
    {
        rb.MovePosition(Vector2.MoveTowards(transform.position, originPos, speed * Time.deltaTime));

        if (Vector2.Distance(transform.position, originPos) < 0.1f)
        {
            isMovingUp = false;
            StartCoroutine(WaitAndMoveDown());
        }
    }

    private IEnumerator WaitAndMoveUp()
    {
        yield return new WaitForSeconds(stayDuration);
        isMovingUp = true;
    }

    private IEnumerator WaitAndMoveDown()
    {
        yield return new WaitForSeconds(moveCooldown);
        isMovingDown = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall"))
        {
            isMovingDown = false;
            isMovingUp = true;
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Test1");
        }
    }
}
