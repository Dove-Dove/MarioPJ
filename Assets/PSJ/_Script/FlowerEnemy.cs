using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnemy : MonoBehaviour
{
    public float attackRange = 10f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public LayerMask playerLayer;
    public bool isMoveUp;

    public enum EnemyState { Hide, Attack, Dead, Move}
    public EnemyState currentState;

    private Transform player;
    private float attackCooldown = 3f;
    private float nextAttackTime;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("IsHide", true);
        currentState = EnemyState.Hide;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Hide:
                HideState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Move:
                MoveState();
                break;
            case EnemyState.Dead:
                DeadState();
                break;
        }
    }

    void HideState()
    {
        // 플레이어 감지
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            animator.SetBool("IsHide", false);
            currentState = EnemyState.Move;
        }
    }

    void AttackState()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            SetDirectionAndAnimation();

            SetAnimationState(true, false);  // 애니메이션 상태 전환
            Invoke("ReturnToMoveState", attackCooldown);  // 쿨타임 후 MoveState로 전환
        }
        else
        {
            ReturnToMoveState();  // 범위 밖으로 벗어나면 바로 MoveState로
        }
    }


    void SetAnimationState(bool isHide, bool isAttack)
    {
        animator.SetBool("IsHide", isHide);
        animator.SetBool("IsAttack", isAttack);
    }

    void ReturnToMoveState()
    {
        currentState = EnemyState.Move;
    }

    void DeadState()
    {
        //애니메이션 재생
        //파괴
        gameObject.SetActive(false);
    }

    void ShootProjectile()
    {
        // 발사체 생성 및 플레이어를 향해 발사
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }

    void MoveState()
    {
        if (animator.GetBool("IsHide") && !animator.GetBool("IsAttack"))
        {
            if (!IsInvoking("hide"))
            {
                Invoke("hide", 3.0f);  // 중복 호출 방지
            }
        }
        else
        {
            if (!IsInvoking("attack"))
            {
                Invoke("attack", 2.0f);  // 중복 호출 방지
            }
        }
    }


    void SetDirectionAndAnimation()
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

    void attack()
    {
        currentState = EnemyState.Attack;
    }
    void hide()
    {
        currentState = EnemyState.Hide;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  
        {
            currentState = EnemyState.Dead;
        }
        else if (collision.gameObject.CompareTag("Attack"))
        {
            currentState = EnemyState.Dead;
        }
    }
}
