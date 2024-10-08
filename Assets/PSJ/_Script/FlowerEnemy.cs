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
        animator.SetBool("IsAttack", true);

        // �÷��̾���� �Ÿ��� ��� üũ�Ͽ� ���Ÿ� ���� ����
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if(player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                if (player.position.y < transform.position.y)
                {
                    animator.SetBool("AttackUp", false);
                }
                else
                {
                    animator.SetBool("AttackUp", true);
                }
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                if (player.position.y < transform.position.y)
                {
                    animator.SetBool("AttackUp", false);
                }
                else
                {
                    animator.SetBool("AttackUp", true);
                }

            }


            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;  // ���� �� ��Ÿ�� ����
                ShootProjectile();
                isMoveUp = false;
                animator.SetBool("IsAttack", false);
                animator.SetBool("IsHide", true);
                currentState = EnemyState.Move;
            }
        }
        else
        {
            isMoveUp = false;
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsHide", true);
            currentState = EnemyState.Move;
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
        if(animator.GetBool("IsHide") && !animator.GetBool("IsAttack"))
        {
            Invoke("hide", 3.0f);
        }
        else
        {
            Invoke("attack", 2.0f);
        }
  
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
        currentState = EnemyState.Dead;
    }
}
