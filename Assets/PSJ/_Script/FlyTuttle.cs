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

    void enemyFly()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime * (movingDown ? 1 : -1));
        if(startPos.y - transform.position.y > moveDistance || transform.position.y - startPos.y > moveDistance)
        {
            movingDown = !movingDown;
        }
    }
    
}
