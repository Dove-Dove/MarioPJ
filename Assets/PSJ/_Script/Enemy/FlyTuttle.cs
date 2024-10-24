using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTuttle : Tuttle
{
    protected bool movingDown = true;
    private Vector2 startPos;
    private float moveDistance = 5f;

    private void Start()
    {
        Tuttleanim = GetComponent<Animator>();

        startPos = transform.position;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Move:
                if (hasWing)
                    enemyFly();
                else
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
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

        }

    }

    new void enemyMove()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1));

        // 발판 확인
        RaycastHit2D groundInfo1 = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D groundInfo2 = Physics2D.Raycast(groundDetect2.position, Vector2.down, rayDistance, groundLayer);


        bool isGrounded = groundInfo1 || groundInfo2;

        if (!isGrounded)
        {
            if (hasWing && Time.time >= nextJumpTime)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
            }
            else if (!hasWing)
            {
                nextJumpTime = Time.time + jumpInterveal;
            }
        }
        else
        {
            if (hasWing && Time.time >= nextJumpTime)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
            }
        }
    }


    void enemyFly()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime * (movingDown ? 1 : -1));
        if(startPos.y - transform.position.y > moveDistance || transform.position.y - startPos.y > moveDistance)
        {
            movingDown = !movingDown;
        }
    }
    
}
