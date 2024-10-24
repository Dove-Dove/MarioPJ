using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어와 부딪히는 방향에따라 shellmove조정
//발판블럭 파괴시 바로 죽음
//발판블럭 흔들리면 등껌질상태
//shellmove에서 블럭파괴

public class Tuttle : Enemy
{
    protected float shellSpeed = 8.0f;

    protected float shellTimer = 8.0f;

    protected Animator Tuttleanim;

    protected bool reverse = false;
    protected GameObject player;

    void Start()
    {
        Tuttleanim = GetComponent<Animator>();
        player = GameObject.Find("Mario");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < range)
        {
            switch (currentState)
            {
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
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall") || collision.gameObject.CompareTag("Box")) //벽 충돌
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
            }
            else if (!hasWing && currentState == State.Shell)
            {
                if (player.GetComponentInChildren<Player_Move>().isKick == true)
                {
                    currentState = State.ShellMove;
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
        else if (collision.gameObject.CompareTag("Player")) //플레이어와 충돌
        {
            bool playerKick = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player_Move>().isKick;
            bool playerIsR = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player_Move>().isRight;

            if (!hasWing && currentState == State.Shell)
            {
                if (playerKick == true)
                {
                    movingLeft = !playerIsR;

                    currentState = State.ShellMove;
                }
            }
        }
        else if(collision.gameObject.CompareTag("Tail"))
        {
            if(currentState == State.Shell)
            {
                Vector3 theScale = transform.localScale;
                theScale.y *= -1;
                transform.localScale = theScale;
                reverse = true;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hasWing)
            {
                hasWing = false;
                currentState = State.Move;
            }
            else if (!hasWing && currentState == State.Move)
            {
                currentState = State.Shell;
            }
            else if (!hasWing && currentState == State.Shell)
            {
                currentState = State.ShellMove;
            }
            else if (currentState == State.ShellMove)
            {
                //플레이어 데미지
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if(currentState == State.ShellMove)
            {
            }
        }
        else if(collision.gameObject.CompareTag("MovingShell"))
        {
        }

    }


    protected float shellElapsedTime = 0;

    public void enemyShell()
    {
        Tuttleanim.SetBool("IsShell", true);
        Tuttleanim.SetBool("ShellMove", false);

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);

        gameObject.layer = 10;
        gameObject.tag = "Shell";

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
        transform.Translate(Vector2.left * shellSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
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

