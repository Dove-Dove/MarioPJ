using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tuttle : Enemy
{
    public float shellSpeed = 8.0f;

    private float shellTimer = 8.0f;

    Animator Tuttleanim;

    public GameObject player;

    void Start()
    {
        Tuttleanim = GetComponent<Animator>();
        player = GameObject.Find("Mario");
    }

    // Update is called once per frame
    void Update()
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall"))
        {
            Flip();
        }
        else if (collision.gameObject.tag.Contains("Attack"))
        {
            currentState = State.Dead;
        }
        else if (collision.gameObject.CompareTag("MovingShell"))
        {
            if (currentState == State.ShellMove)
            {
                Flip();
            }
            else
            {
                currentState = State.Dead;
            }
        }
        else if (collision.gameObject.CompareTag("Shell"))
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("Player"))
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
                if (player.GetComponentInChildren<Player_Move>().isKick == true)
                {
                    currentState = State.ShellMove;
                }
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


    private float shellElapsedTime = 0; // 클래스 필드로 선언

    void enemyShell()
    {
        Tuttleanim.SetBool("IsShell", true);
        gameObject.tag = "Shell";

        shellElapsedTime += Time.deltaTime; // timer를 누적
        if (shellElapsedTime >= shellTimer)
        {
            Tuttleanim.SetBool("IsShell", false);
            currentState = State.Move;
            gameObject.tag = "Enemy";
            shellElapsedTime = 0; // 타이머 초기화
        }
    }

    void enemyShellMove()
    {
        gameObject.tag = "MovingShell";
        gameObject.layer = 11;

        gameObject.GetComponent<BoxCollider2D>().excludeLayers = LayerMask.NameToLayer("EnemyWall");
        if (Tuttleanim.GetCurrentAnimatorStateInfo(0).IsName("ShellMove") == false)
        {
            Tuttleanim.SetTrigger("ShellMove");
        }
        transform.Translate(Vector2.left * shellSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
    }

    new public void enemyDead()
    {
        Tuttleanim.SetTrigger("IsDead");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        currentState = State.Dead;
        Invoke("destroy", 1.0f);
    }


}

