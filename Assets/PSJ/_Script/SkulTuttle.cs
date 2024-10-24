using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulTuttle : Tuttle
{
    protected Animator Skulanim;



    public float reviveTimer = 5.0f;
    public float reviveTime = 0f;

    void Start()
    {
        Skulanim = GetComponent<Animator>();
        player = GameObject.Find("Mario");

    }


    void Update()
    {
        if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < range)
        {
            switch (currentState)
            {
                case State.Move:
                    gameObject.tag = "Enemy";
                    enemyMove();
                    break;
                case State.Dead:
                    enemyHit();
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall")) //º® Ãæµ¹
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("Tail"))
        {
            currentState = State.Dead;
        }
    }



    void enemyHit()
    {
        Skulanim.SetBool("IsHit", true);
        gameObject.tag = "Untagged";

        reviveTime += Time.deltaTime;
        if(reviveTime >= reviveTimer)
        {
            currentState = State.Move;
            Skulanim.SetBool("IsHit", false);
            gameObject.tag = "Enemy";
            reviveTime = 0;
        }
        
    }
}
