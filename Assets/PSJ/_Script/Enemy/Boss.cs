using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum State { Idle, Move, Attack, Hit, Dead }
    public State currentState = State.Idle;

    public float moveSpeed = 3f;
    public LayerMask groundLayer;
    public float range = 10f;
    private Rigidbody2D rb;
    private Transform player;

    public float attackCooldown = 4f;
    public float nextAttackTime;

    public float jumpForce = 10f;
    public float hitCoolTime = 2f;
    public int bossHp = 3;

    private bool isJumping;
    private bool isHit = false; // 피격 상태 확인 플래그

    Animator animator;
    public AudioSource HitSound;
    public GameObject score;
    public GameObject effect;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        nextAttackTime = Time.time + attackCooldown;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isJumping = false;
    }

    void Update()
    {
        if (bossHp == 1)
        {
            moveSpeed = 4f;
        }

        switch (currentState)
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
        if (Vector2.Distance(transform.position, player.position) < range)
        {
            currentState = State.Attack;
        }
    }

    void bossAttack()
    {
        animator.SetBool("IsAttack", true);
        gameObject.tag = "BossAttack";
        Invoke("Move", 1.0f);
    }

    void bossMove()
    {
        isJumping = false;
        isHit = false;  // 이동 시 피격 상태 해제
        gameObject.tag = "Enemy";

        Vector2 direction = (transform.position.x >= player.position.x) ? Vector2.left : Vector2.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);

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
        if (collision.gameObject.CompareTag("PlayerAttack") && currentState == State.Move && !isHit)
        {
            HitSound.Play();
            Enemy.Score(gameObject, score);

            if (bossHp > 1)
            {
                bossHp--;
                rb.velocity = new Vector2(0, rb.velocity.y);

                animator.SetBool("IsHit", true);
                currentState = State.Hit;
                isHit = true;  // 피격 중으로 설정
            }
            else
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, rb.velocity.y);

                StartCoroutine(WaitAndEffect());

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

    private IEnumerator WaitAndEffect()
    {
        yield return new WaitForSeconds(2.8f);

        GameObject projectile = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(projectile, 3.0f);
    }
}
