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
    private float hideTimer = 2f;
    private float attackCooldown = 2f;
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
            case EnemyState.Dead:
                DeadState();
                break;
            case EnemyState.Move:
                MoveState();
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
        animator.SetBool("IsAttack", true);

        // �÷��̾���� �Ÿ��� ��� üũ�Ͽ� ���Ÿ� ���� ����
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if(player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (player.position.y < transform.position.y)
                {
                    animator.SetBool("AttackUp", true);
                }
                else
                {
                    animator.SetBool("AttackUp", false);
                }
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                if (player.position.y < transform.position.y)
                {
                    animator.SetBool("AttackUp", true);
                }
                else
                {
                    animator.SetBool("AttackUp", false);
                }

            }

            if (Time.time >= nextAttackTime)
            {
                ShootProjectile();
                nextAttackTime = Time.time + attackCooldown;  // ���� �� ��Ÿ�� ����
                isMoveUp = false;
                currentState = EnemyState.Hide;
            }
        }
        else
        {
            isMoveUp = false;
            currentState = EnemyState.Hide;
        }
    }

    void DeadState()
    {
        //�ִϸ��̼� ���
        //�ı�
        Destroy(gameObject);
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
        //�ִϸ��̼�
        currentState = EnemyState.Attack;

  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState = EnemyState.Dead;
    }
}
