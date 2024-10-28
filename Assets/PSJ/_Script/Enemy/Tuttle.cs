using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어와 부딪히는 방향에따라 shellmove조정
//발판블럭 파괴시 바로 죽음
//발판블럭 흔들리면 등껌질상태
//shellmove에서 블럭파괴

public class Tuttle : Enemy
{
    protected float shellSpeed = 6.0f;

    protected float shellTimer = 8.0f;

    protected Animator Tuttleanim;

    protected bool reverse = false;
    protected bool playerIsR;
    public LayerMask playerLayer;

    void Start()
    {
        Tuttleanim = GetComponent<Animator>();
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
            case State.Shell:
                enemyShell();
                break;
            case State.ShellMove:
                enemyShellMove();
                break;
        }
        if (!hasWing)
        {
            wings.SetActive(false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall")) //벽 충돌
        {
            Flip();
        }
        else if(collision.gameObject.name.Contains("Cliff") && currentState == State.Move)
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("Tail")) //공격 충돌
        {
            if (hasWing)
            {
                DeadSound.Play();
                hasWing = false;
                currentState = State.Move;
            }
            else if (!hasWing && currentState == State.Move)
            {
                DeadSound.Play();
                currentState = State.Shell;
                if (collision.gameObject.CompareTag("Tail"))
                {
                    Jump();
                    Vector3 theScale = transform.localScale;
                    theScale.y *= -1;
                    transform.localScale = theScale;
                    reverse = true;
                }

            }
            else if (!hasWing && currentState == State.Shell)
            {
                if (player.GetComponentInChildren<Player_Move>().isKick == true)
                {
                    currentState = State.ShellMove;
                }
                if(collision.gameObject.CompareTag("Tail"))
                {
                    Jump();
                    if(!reverse)
                    {
                        Vector3 theScale = transform.localScale;
                        theScale.y *= -1;
                        transform.localScale = theScale;
                        reverse = true;
                    }
                }
            }
            else if(!hasWing && currentState == State.ShellMove)
            {
                currentState = State.Shell;
            }

        }
        else if (collision.gameObject.CompareTag("PlayerFire"))
        {
            DeadSound.Play();
            currentState = State.Dead;
        }
        else if (collision.gameObject.CompareTag("MovingShell")) //움직이는 껍질과충돌
        {
            if (currentState == State.ShellMove)
            {
                Flip();
            }
            else if (currentState != State.Shell)
            {
                hasWing = false;
                DeadSound.Play();
                currentState = State.Dead;
            }
        }
        else if (collision.gameObject.CompareTag("Shell")) //껍질과 충돌
        {
            Flip();
        }
        else if(collision.gameObject.CompareTag("StarInvincible"))
        {
            DeadSound.Play();
            Jump();
            Tuttleanim.SetTrigger("IsDead2");
            currentState = State.Dead;

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //플레이어와 충돌
        {
            bool playerKick = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player_Move>().isKick;

            if (!hasWing && currentState == State.Shell)
            {
                if (playerIsR)
                    movingLeft = true;
                else
                    movingLeft = false;
                if (playerKick == true)
                    currentState = State.ShellMove;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
    }


    protected float shellElapsedTime = 0;

    public void enemyShell()
    {
        Tuttleanim.SetBool("IsShell", true);
        Tuttleanim.SetBool("ShellMove", false);

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);

        gameObject.layer = 10;
        gameObject.tag = "Shell";

        //플레이어 방향 체크
        RaycastHit2D playerCheck1;
        RaycastHit2D playerCheck2;

        playerCheck1 = Physics2D.Raycast(gameObject.transform.position, Vector2.left, 0.7f, playerLayer);
        Debug.DrawLine(gameObject.transform.position, new Vector2(gameObject.transform.position.x - 0.7f, gameObject.transform.position.y), Color.blue);
        playerCheck2 = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 0.7f, playerLayer);
        Debug.DrawLine(gameObject.transform.position, new Vector2(gameObject.transform.position.x + 0.7f, gameObject.transform.position.y), Color.blue);

        if (playerCheck1)
        {
            playerIsR = false;
        }
        if(playerCheck2)
        {
            playerIsR = true;
        }


        shellElapsedTime += Time.deltaTime; // timer를 누적
        if (shellElapsedTime >= shellTimer)
        {
            if(reverse)
            {
                Vector3 theScale = transform.localScale;
                theScale.y *= -1;
                transform.localScale = theScale;
            }
            Tuttleanim.SetBool("IsShell", false);
            currentState = State.Move;
            gameObject.tag = "Enemy";
            shellElapsedTime = 0; // 타이머 초기화
        }
    }

    public void enemyShellMove()
    {
        gameObject.tag = "MovingShell";
        gameObject.layer = 11;

        gameObject.GetComponent<BoxCollider2D>().excludeLayers = LayerMask.NameToLayer("Default");
        if (Tuttleanim.GetCurrentAnimatorStateInfo(0).IsName("ShellMove") == false)
        {
            Tuttleanim.SetBool("ShellMove", true);
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        transform.Translate(Vector2.left * shellSpeed * Time.deltaTime * (movingLeft ? 1 : -1));

        //블록체크
        RaycastHit2D blockCheck;
        RaycastHit2D noteCheck;

        if (movingLeft)
        {
            blockCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.left, 0.2f, groundLayer);
            noteCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.left, 0.2f, noteLayer);
            Debug.DrawLine(gameObject.transform.position, new Vector2(groundDetect1.position.x - 0.2f, gameObject.transform.position.y), Color.blue);
        }
        else
        {
            blockCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 0.2f, groundLayer);
            noteCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 0.2f, noteLayer);
            Debug.DrawLine(gameObject.transform.position, new Vector2(gameObject.transform.position.x + 0.2f, gameObject.transform.position.y), Color.blue);

        }

        if (blockCheck && blockCheck.collider.CompareTag("Box"))
        {
            Debug.Log("BoxFlip");
            Flip();
        }

        if (noteCheck)
        {
            Debug.Log("NoteFlip");
            Flip();
        }

    }

    new public void enemyDead()
    {
        Tuttleanim.SetTrigger("IsDead");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);


        currentState = State.Dead;
        Invoke("destroy", 1.0f);
    }

}

