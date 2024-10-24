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
    }

    void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, player.position) < attackRange)
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
