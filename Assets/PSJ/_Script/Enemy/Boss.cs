using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    /* 보스 클래스
     * 0.플레이어 방향으로 이동
     * 1.공격상태로 시작
     * 2.공격상태 이후 약간점프
     * 3.쿨타임마다 공격상태
     * 밟히면 애니메이션후 공격상태,쿨타임 초기화
     * 2번 밟히면 이동속도 상승
     */

    public enum State
    {
        Idle, Move, Attack, Hit, Dead
    }

    public State currentState = State.Idle;

    public float moveSpeed = 3f;
    public LayerMask groundLayer;
    public float range = 10f;

    private Rigidbody2D rb;

    private Transform player;
    private float attackCooldown = 4f;
    public float nextAttackTime;

    public float jumpForce = 10f;

    public float hitCoolTime = 2f;

    public int bossHp = 3;
    private bool isJumping;


    Animator animator;

    public AudioSource HitSound;
    public GameObject score;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        nextAttackTime = Time.time + attackCooldown;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(bossHp == 1)
        {
            moveSpeed = 4f;
        }
        
        switch(currentState)
        {
            case State.Idle:
                bossIdle();
                break;
            case State.Attack:
                bossAttack();
                break;
            case State.Move:
                bossMove();
                break;
            case State.Hit:
                bossHit();
                break;
            case State.Dead:
                bossDead();
                break;
        }
    }

    public void bossIdle()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            currentState = State.Attack;
        }
    }


    void bossAttack()
    {
        animator.SetBool("IsAttack", true);
        gameObject.tag = "E_Attack";

        Invoke("Move", 1.0f);
    }

    void bossMove()
    {
        isJumping = false;
        gameObject.tag = "Enemy";
        if (transform.position.x >= player.position.x)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        if (Time.time >= nextAttackTime)
        {
            currentState = State.Attack;
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void bossHit()
    {
        animator.SetBool("IsAttack", false);
        Invoke("Attack", hitCoolTime);
        nextAttackTime = Time.time + attackCooldown;

    }

    void bossDead()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 3.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerAttack") &&
            currentState == State.Move)
        {
            HitSound.Play();
            Enemy.Score(gameObject, score);


            if (bossHp > 1)
            {
                bossHp--;
                rb.velocity = Vector2.zero;

                animator.SetBool("IsHit", true);
                currentState = State.Hit;
            }
            else
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;

                animator.SetTrigger("IsDead");
                currentState = State.Dead;
            }
            
        }

    }

    public void Jump()
    {
        isJumping = true;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x, jumpForce);
    }

    void Attack()
    {
        currentState = State.Attack;
        animator.SetBool("IsHit", false);
    }

    void Move()
    {
        currentState = State.Move;
        animator.SetBool("IsAttack", false);
    }

    void destroy()
    {
        gameObject.SetActive(false);
    }
}
