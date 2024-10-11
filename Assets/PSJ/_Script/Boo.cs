using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour
{
    /*
     * 0.좌우로 이동
     * 1.방향은 항상 플레이어
     * 2.사거리 안에 플레이어가 들어오면
     * 3.throw 2초마다 반복
    */



    public float moveSpeed = 3f;
    public float distance = 5f;

    public LayerMask groundLayer;

    private Rigidbody2D rb;

    public float attackRange = 10f;
    public bool inRange;

    private Transform player;
    private float attackCooldown = 2f;
    private float nextAttackTime;

    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
        
        SetDirectionAndAnimation();

    }

    void ShootProjectile()
    {
        // 발사체 생성 및 플레이어를 향해 발사
        if(inRange)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - firePoint.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
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
    }

}

