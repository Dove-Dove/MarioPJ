using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuttle : Enemy
{
    public float shellSpeed = 8.0f;

    private float shellTimer = 3.0f;

    Animator Tuttleanim;

    void Start()
    {
        Tuttleanim = GetComponent<Animator>();

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
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hasWing)
            {
                hasWing = false;
                currentState = State.Move;
            }
            else if(!hasWing && currentState == State.Move)
            {
                currentState = State.Shell;
            }
            else if(!hasWing && currentState == State.Shell)
            {
                currentState = State.ShellMove;
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if(currentState != State.ShellMove)
            {
                Flip();
            }
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            currentState = State.Dead;
        }


    }

    void enemyShell()
    {
        Tuttleanim.SetBool("IsShell", true);

        float timer = 0;
        timer += Time.deltaTime;
        if(timer >= shellTimer)
        {
            Tuttleanim.SetBool("IsShell", false);
            currentState = State.Move;
            gameObject.tag = "Enemy";
        }

    }

    void enemyShellMove()
    {
        gameObject.tag = "Attack";
        Tuttleanim.SetTrigger("ShellMove");
        transform.Translate(Vector2.left * shellSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
    }

}

