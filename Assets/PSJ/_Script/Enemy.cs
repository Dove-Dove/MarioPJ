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
    public float rayDistance = 1f;
    public Transform groundDetect1, groundDetect2;
    public Transform jumpDetect1, jumpDetect2;

    public bool hasWing;

    public float jumpForce = 5f;
    public float jumpInterveal = 2f;

    private Rigidbody2D rb;
    private float nextJumpTime;
    public bool movingLeft = true;

    Animator animator;

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
    }

    public void enemyMove()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
        //발판 확인
        RaycastHit2D groundInfo1 = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D groundInfo2 = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D jumpInfo1 = Physics2D.Raycast(jumpDetect1.position, Vector2.down, 7.0f, groundLayer);
        RaycastHit2D jumpInfo2 = Physics2D.Raycast(jumpDetect2.position, Vector2.down, 7.0f, groundLayer);

        //발판이 없을 때
        if (groundInfo1.collider == false || groundDetect2 == false)
        {
            if (hasWing)
            {
                if (jumpInfo1.collider == false || jumpInfo2.collider == false)
                {
                    Flip();
                }
            }
            else
            {
                Flip();
            }
        }
        if (hasWing)
        {
            if (Time.time >= nextJumpTime)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
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
        if(movingLeft)
        {
            rb.velocity = new Vector2(-moveSpeed * Time.deltaTime, jumpForce);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * Time.deltaTime, jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(hasWing)
            {
                hasWing = false;
                currentState = State.Move;
            }
            else
            {
                currentState = State.Dead;
            }
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Flip();
        }
        else if(collision.gameObject.CompareTag("Attack"))
        {
            currentState = State.Dead;
        }
    }

    public void enemyDead()
    {
        animator.SetTrigger("IsDead");
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

