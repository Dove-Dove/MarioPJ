using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //적 기본클래스
    // Move, Dead
    public enum State
    {
        Move, Dead, Shell, ShellMove
    }

    public State currentState = State.Move;

    
    public float moveSpeed = 3f;
    public float distance = 5f;

    public LayerMask groundLayer;
    //public LayerMask wallLayer;
    public float rayDistance = 1f;
    public Transform groundDetect1, groundDetect2;

    public bool hasWing;
    private bool isJumping;

    public float jumpForce = 5f;
    public float jumpInterveal = 2f;

    private Rigidbody2D rb;
    public float nextJumpTime;
    public bool movingLeft = true;

    Animator animator;
    public GameObject wings;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time + jumpInterveal;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Move:
                enemyMove();
                break;

            case State.Dead:
                enemyDead();
                break;
        }
        if(!hasWing)
        {
            wings.SetActive(false);
        }
    }

    public void enemyMove()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1));

        // 발판 확인
        RaycastHit2D groundInfo1 = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D groundInfo2 = Physics2D.Raycast(groundDetect2.position, Vector2.down, rayDistance, groundLayer);

        bool isGrounded = groundInfo1 || groundInfo2;

        if(!isGrounded)
        {
            if(hasWing && Time.time >= nextJumpTime && !isJumping)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
                isJumping = false;
            }
            else if(!hasWing)
            {
                Flip();
                nextJumpTime = Time.time + jumpInterveal;
            }
        }
        else
        {
            if(hasWing && Time.time >= nextJumpTime && !isJumping)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
                isJumping = false;

            }
        }
    }


    public void Flip()
    {
        movingLeft = !movingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Jump()
    {
        isJumping = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //else if(collision.gameObject.layer == LayerMask.NameToLayer("EnemyWall"))
        if (collision.gameObject.CompareTag("EnemyWall"))
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("MovingShell"))
        {
            currentState = State.Dead;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (hasWing)
            {
                Debug.Log(collision.gameObject.name);

                hasWing = false;
                currentState = State.Move;
            }
            else
            {
                Debug.Log(collision.gameObject.name);

                currentState = State.Dead;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hasWing)
            {
                Debug.Log(collision.tag);

                hasWing = false;
                currentState = State.Move;
            }
            else
            {
                Debug.Log(collision.tag);

                currentState = State.Dead;
            }
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
        }
        else if (collision.gameObject.tag.Contains("Attack"))
        {
            currentState = State.Dead;
        }
        else if(collision.gameObject.CompareTag("MovingShell"))
        {
            currentState = State.Dead;
        }


    }

    public void enemyDead()
    {
        animator.SetTrigger("IsDead");
        rb.gravityScale = 0;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        currentState = State.Dead;
        Invoke("destroy", 1.0f);
    }

    void destroy()
    {
        gameObject.SetActive(false);
    }

}

/*
점프할 때 방향전환시 플립제한
ShellMove상태일때 방향설정
*/

