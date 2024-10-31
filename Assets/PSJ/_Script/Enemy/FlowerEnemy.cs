using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerEnemy : MonoBehaviour
{
    /*
    0.hide 
    1.플레이어가 사거리안에 들어옴
    2.moveup
    3.플레이어위치 파악
    4.attack
    5.movedown
    6.그대로 사거리안이면->2//밖이면 0
    */

    public float attackRange = 10f;
    public float projectileSpeed = 7f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    protected Vector2 originPos;
    protected Vector2 upPos;

    public int speed = 2;

    public bool inRange;
    protected Transform player;
    protected float attackCooldown = 3f;
    protected float nextAttackTime;

    Animator animator;
    public LayerMask playerLayer;

    protected Vector2 boxSize = new Vector2(1f, 3f);
    protected Vector2 boxSize2 = new Vector2(2f, 1f);
    public bool IsClose;

    protected bool playerIsRight;
    protected bool playerIsUp;

    public Transform Dir1;
    public Transform Dir2;
    public Transform Dir3;
    public Transform Dir4;

    public GameObject score;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsHide", true);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inRange = false;
        originPos = transform.position;
        upPos = new Vector2(transform.position.x, transform.position.y + 2);
        IsClose = false;
    }

    // Update is called once per frame
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


            if((hit.collider != null && hit.collider.gameObject.layer == playerLayer)
                || (hit2.collider != null && hit2.collider.gameObject.layer == playerLayer))
                IsClose = true;
            else
                IsClose = false;

            //DrawBox(pipeOrigin, boxSize);
            //DrawBox(pipeUp, boxSize2);
        }

        if (Vector2.Distance(gameObject.transform.position, player.position) < attackRange)
            inRange = true;
        else
            inRange = false;
        
        if (inRange && !IsClose)
        {
            animator.SetBool("IsHide", false);
            //gameObject.tag = "Enemy";
            animator.SetBool("InRange", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            animator.SetBool("IsHide", false);
            //gameObject.tag = "Untagged";
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            animator.SetBool("InRange", false);

        }

        SetDirectionAndAnimation();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RFlowerMoveDown") == true)
        {
            MoveUp();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("RFlowerMoveUp") == true)
        {
            Movedown();
        }


    }



    void ShootProjectile()
    {
        // 발사체 생성 및 플레이어를 향해 발사
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        //Vector2 direction = (player.position - firePoint.position).normalized;
        Vector2 direction;

        if(playerIsRight && playerIsUp)
        {
            direction = (Dir3.position - firePoint.position).normalized;
        }
        else if(playerIsRight && !playerIsUp)
        {
            direction = (Dir4.position - firePoint.position).normalized;
        }
        else if(!playerIsRight && playerIsUp)
        {
            direction = (Dir3.position - firePoint.position).normalized;
        }
        else
        {
            direction = (Dir4.position - firePoint.position).normalized;
        }

        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }



    public void SetDirectionAndAnimation()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            playerIsRight = false;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            playerIsRight = true;
        }

        if(player.position.y >= transform.position.y)
        {
            animator.SetBool("AttackUp", true);
            playerIsUp = true;
        }
        else
        {
            animator.SetBool("AttackUp", false);
            playerIsUp = false;
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
        else if (collision.gameObject.CompareTag("PlayerFire"))
        {
            if (!animator.GetBool("IsHide"))
            {
                Enemy.Score(gameObject, score);
                animator.SetTrigger("IsDead");

                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == ("Tail"))
        {
            Debug.Log("enter");
            if (!animator.GetBool("IsHide"))
            {
                Enemy.Score(gameObject, score);
                animator.SetTrigger("IsDead");

                gameObject.SetActive(false);
            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == ("Tail"))
        {
            Debug.Log("stay");

            if (!animator.GetBool("IsHide"))
            {

                Enemy.Score(gameObject, score);
                animator.SetTrigger("IsDead");

                gameObject.SetActive(false);
            }

        }
    }

    public void Movedown()
    {
        transform.position = Vector3.MoveTowards(transform.position, upPos, speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, originPos, speed * Time.deltaTime);
    }

    public static void DrawBox(Vector2 origin, Vector2 size)
    {
        Vector2 halfSize = size / 2;
        Vector2 bottomLeft = origin - halfSize;
        Vector2 bottomRight = origin + new Vector2(halfSize.x, -halfSize.y);
        Vector2 topLeft = origin + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = origin + halfSize;

        Debug.DrawLine(bottomLeft, bottomRight, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);
        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(bottomRight, topRight, Color.red);
    }
}


