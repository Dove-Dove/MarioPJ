using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //적 기본클래스
    // Move, Dead
    public enum State
    {
        Move, Dead, Shell, ShellMove, Idle
    }

    public State currentState = State.Idle;

    protected float range = 15f;
    
    public float moveSpeed = 3f;
    protected float distance = 5f;

    public LayerMask groundLayer;
    public LayerMask slopeLayer;
    public LayerMask noteLayer;
    protected float rayDistance = 1f;
    public Transform groundDetect1, groundDetect2;

    public bool hasWing;
    private bool isJumping;

    protected float jumpForce = 5f;
    protected float jumpInterveal = 2f;

    private Rigidbody2D rb;
    protected float nextJumpTime;
    public bool movingLeft = true;

    Animator animator;
    public GameObject wings;

    public AudioSource DeadSound;

    protected RaycastHit2D BlockCheck;

    protected float angle;
    protected Vector2 perp;
    protected bool isSlope;

    protected bool attackedbyTail;
    protected GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time + jumpInterveal;
        player = GameObject.Find("Mario");
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                enemyIdle();
                break;

            case State.Move:
                enemyMove();
                break;

            case State.Dead:
                enemyDead();
                break;
        }
        if (!hasWing)
        {
            wings.SetActive(false);
        }

    }

    public void enemyIdle()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            currentState = State.Move;
        }
    }

        public void enemyMove()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;

        // 발판 확인
        RaycastHit2D groundleft = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D groundright = Physics2D.Raycast(groundDetect2.position, Vector2.down, rayDistance, groundLayer);

        bool isGrounded = groundleft || groundright;

        //경사확인
        RaycastHit2D hit = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D fronthit;

        if (movingLeft)
        {
            fronthit = Physics2D.Raycast(groundDetect1.position, Vector2.left, 0.1f, slopeLayer);
            //Debug.DrawLine(fronthit.point ,Vector2.left, Color.magenta);
        }
        else
        {
            fronthit = Physics2D.Raycast(groundDetect1.position, Vector2.right, 0.1f, slopeLayer);
            //Debug.DrawLine(fronthit.point, Vector2.right, Color.magenta);
        }

        //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
        //Debug.DrawLine(hit.point, hit.point + perp, Color.red);

        if (hit || fronthit)
        {
            if (fronthit)
            {
                //Debug.Log("angle = " + angle);
                slopeChk(fronthit);
            }
            else
                slopeChk(hit);
        }

        //경사면에 따른 속도 조정
        if(!isSlope)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
        }
        else if(isSlope)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.Translate(new Vector2(perp.x * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1), perp.y * moveSpeed * Time.deltaTime *(movingLeft ? 1 : -1)));
        }

        //점프
        if (!isGrounded)
        {
            if(hasWing && Time.time >= nextJumpTime && !isJumping)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
                isJumping = false;
            }
            else if(!hasWing)
            {
                //Flip();
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

        //블록체크
        RaycastHit2D blockCheck;
        RaycastHit2D noteCheck;

        if(movingLeft)
        {
            blockCheck = Physics2D.Raycast(groundDetect1.position, Vector2.left, 0.3f, groundLayer);
            noteCheck = Physics2D.Raycast(groundDetect1.position, Vector2.left, 0.3f, noteLayer);
        }
        else
        {
            blockCheck = Physics2D.Raycast(groundDetect1.position, Vector2.right, 0.3f, groundLayer);
            noteCheck = Physics2D.Raycast(groundDetect1.position, Vector2.right, 0.3f, noteLayer);
        }

        if (blockCheck && blockCheck.collider.CompareTag("Box"))
        {
            Flip();
        }

        if (noteCheck)
            Flip();
    }

    public void slopeChk(RaycastHit2D hit)
    {
        perp = Vector2.Perpendicular(hit.normal).normalized;
        angle = Vector2.Angle(hit.normal, Vector2.up);

        if (angle != 0)
        {
            isSlope = true;
        }
        else
        {
            isSlope = false;
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
        gameObject.GetComponent<Rigidbody2D>().velocity 
            = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall")) //벽 충돌
        {
            Flip();
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            Flip();
        }
        else if (collision.gameObject.name.Contains("Cliff") && currentState == State.Move)
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("MovingShell"))
        {
            DeadSound.Play();
            hasWing = false;
            currentState = State.Dead;
        }
        else if (collision.gameObject.CompareTag("Shell"))
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("Tail"))
        {
            DeadSound.Play();

            if (hasWing)
            {
                hasWing = false;
                currentState = State.Move;
            }
            else
            {
                if(collision.gameObject.CompareTag("PlayerAttack"))
                {
                    animator.SetTrigger("IsDead");
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.zero;
                }
                else if (collision.gameObject.CompareTag("Tail"))
                {
                    Jump();
                    animator.SetTrigger("IsDead2");
                }
                currentState = State.Dead;
            }
        }
        else if(collision.gameObject.CompareTag("StarInvincible"))
        {
            DeadSound.Play();
            Jump();
            animator.SetTrigger("IsDead2");
            currentState = State.Dead;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


    }

    public void enemyDead()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("destroy", 1.0f);
    }

    void destroy()
    {
        gameObject.SetActive(false);
    }

}

/*
    경사면 처리
*/

