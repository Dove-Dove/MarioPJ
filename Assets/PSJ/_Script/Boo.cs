using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour
{
    /*
     * 0. 좌우로 일정 범위 이동
     * 1. 방향은 항상 플레이어를 바라봄
     * 2. 사거리 안에 플레이어가 들어오면 공격
     * 3. 2초마다 부메랑을 던짐
    */

    public float moveSpeed = 2f;
    public float moveDistance = 1f; // 좌우로 이동하는 최대 거리
    private float startXPosition;

    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private float attackRange = 10f;
    public bool inRange;

    private Transform player;
    private float attackCooldown = 1f; // 부메랑을 던지는 쿨타임
    public float nextAttackTime;

    public float projectileSpeed = 12f; // 발사체 속도
    public GameObject projectilePrefab;
    public Transform throwPoint;

    public int maxBoomerangs = 4; //부메랑 최대 개수
    private int currentBoomerangs = 0; //부메랑 현재 개수

    public Transform ThrowDistance;

    Animator animator;
    public AudioSource DeadSound;
    public AudioSource ThrowSound;

    private bool movingLeft = true;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startXPosition = transform.position.x; // 시작 위치 저장
        inRange = false;
        nextAttackTime = Time.time;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Vector2.Distance(transform.position,player.position)< attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        // 공격 쿨타임이 지나면 공격 실행
        if (Time.time >= nextAttackTime && inRange)
        {
            //부메랑 3개이하 유지
            if (currentBoomerangs < maxBoomerangs)
            {
                ShootProjectile();
                nextAttackTime = Time.time + attackCooldown; // 다음 공격 시간 갱신
            }
        }

        // 방향 설정
        SetDirectionAndAnimation();
    }

    // 적이 좌우로 일정 범위를 반복해서 이동
    void Move()
    {
        if (movingLeft)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= startXPosition - moveDistance)
            {
                movingLeft = false;
            }
        }
        else
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= startXPosition + moveDistance)
            {
                movingLeft = true;
            }
        }
    }

    // 플레이어를 향해 발사체를 발사
    void ShootProjectile()
    {
        // 발사체 생성 및 플레이어를 향해 발사
        ThrowSound.Play();
        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
        Vector2 direction = (player.position - throwPoint.position).normalized;
        Boomerang boomerang = projectile.GetComponent<Boomerang>();
        boomerang.SetShooter(this);
        currentBoomerangs++;
    }

    public void DecreaseBoomerangCount()
    {
        currentBoomerangs--;
    }

    // 적이 항상 플레이어를 바라보도록 방향을 설정
    void SetDirectionAndAnimation()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); // 플레이어가 왼쪽에 있을 때
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // 플레이어가 오른쪽에 있을 때
        }
    }

    void booDead()
    {
        DeadSound.Play();
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        animator.SetTrigger("IsDead");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("destroy", 1.0f);
    }

    void destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MovingShell"))
        {
            booDead();
        }
        else if(collision.gameObject.CompareTag("PlayerAttack"))
        {
            booDead();
        }
        
    }
}
