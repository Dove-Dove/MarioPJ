using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnemy2 : FlowerEnemy
{
    Animator animator2;

    void Start()
    {
        animator2 = GetComponent<Animator>();
        animator2.SetBool("IsHide", true);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inRange = false;
        originPos = transform.position;
        upPos = new Vector2(transform.position.x, transform.position.y + 2);
        IsClose = false;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, originPos) < 0.1f)
        {
            Vector2 pipeOrigin = new Vector2(transform.position.x - 0.5f, transform.position.y - 2f + boxSize.y / 2);

            if (player.position.x < transform.position.x)
                pipeOrigin.x -= 0.5f;
            else
                pipeOrigin.x += 1.5f;

            RaycastHit2D hit = Physics2D.BoxCast(pipeOrigin, boxSize, 0f, Vector2.zero, 0f, playerLayer);

            Vector2 pipeUp = new Vector2(transform.position.x, transform.position.y + 1f + boxSize2.y / 2);
            RaycastHit2D hit2 = Physics2D.BoxCast(pipeUp, boxSize2, 0f, Vector2.zero, 0f, playerLayer);


            if ((hit.collider != null && hit.collider.tag.Contains("Player"))
                || (hit2.collider != null && hit2.collider.tag.Contains("Player")))
                IsClose = true;
            else
                IsClose = false;

            //DrawBox(pipeOrigin, boxSize);
            //DrawBox(pipeUp, boxSize2);
        }

        if (Vector2.Distance(gameObject.transform.position, player.position) < attackRange && !IsClose)
        {
            animator2.SetBool("IsHide", false);
            gameObject.tag = "Enemy";
            inRange = true;
            animator2.SetBool("InRange", true);
        }
        else
        {
            animator2.SetBool("IsHide", false);
            gameObject.tag = "Untagged";
            inRange = false;
            animator2.SetBool("InRange", false);
        }

        if (animator2.GetCurrentAnimatorStateInfo(0).IsName("GFlowerMoveDown") == true)
        {
            MoveUp();
        }
        else if (animator2.GetCurrentAnimatorStateInfo(0).IsName("GFlowerMoveUp") == true)
        {
            Movedown();
        }
    }

 

}
