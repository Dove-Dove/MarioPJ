using System.Collections;
using System.Collections.Generic;
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
    public float projectileSpeed = 10f;
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsHide", true);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inRange = false;
        originPos = transform.position;
        upPos = new Vector2(transform.position.x, transform.position.y + 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, player.position) < attackRange)
        {
            animator.SetBool("IsHide", false);
            gameObject.tag = "Enemy";
            inRange = true;
            animator.SetBool("InRange",true);
        }
        else
        {
            animator.SetBool("IsHide", false);
            gameObject.tag = "Untagged";
            inRange = false;
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
        Vector2 direction = (player.position - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }



    public void SetDirectionAndAnimation()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetBool("AttackUp", player.position.y >= transform.position.y);
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
}


