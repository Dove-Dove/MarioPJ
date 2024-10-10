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
        // �÷��̾� ����
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

            SetAnimationState(true, false);  // �ִϸ��̼� ���� ��ȯ
            Invoke("ReturnToMoveState", attackCooldown);  // ��Ÿ�� �� MoveState�� ��ȯ
        }
        else
        {
            ReturnToMoveState();  // ���� ������ ����� �ٷ� MoveState��
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
        //�ִϸ��̼� ���
        //�ı�
        gameObject.SetActive(false);
    }

    void ShootProjectile()
    {
        // �߻�ü ���� �� �÷��̾ ���� �߻�
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
                Invoke("hide", 3.0f);  // �ߺ� ȣ�� ����
            }
        }
        else
        {
            if (!IsInvoking("attack"))
            {
                Invoke("attack", 2.0f);  // �ߺ� ȣ�� ����
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
