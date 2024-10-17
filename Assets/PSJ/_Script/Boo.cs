using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour
{
    /*
     * 0. �¿�� ���� ���� �̵�
     * 1. ������ �׻� �÷��̾ �ٶ�
     * 2. ��Ÿ� �ȿ� �÷��̾ ������ ����
     * 3. 2�ʸ��� �θ޶��� ����
    */

    public float moveSpeed = 2f;
    public float moveDistance = 1f; // �¿�� �̵��ϴ� �ִ� �Ÿ�
    private float startXPosition;

    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private float attackRange = 10f;
    public bool inRange;

    private Transform player;
    private float attackCooldown = 1f; // �θ޶��� ������ ��Ÿ��
    public float nextAttackTime;

    public float projectileSpeed = 12f; // �߻�ü �ӵ�
    public GameObject projectilePrefab;
    public Transform throwPoint;

    public int maxBoomerangs = 4; //�θ޶� �ִ� ����
    private int currentBoomerangs = 0; //�θ޶� ���� ����

    public Transform ThrowDistance;

    Animator animator;
    public AudioSource DeadSound;
    public AudioSource ThrowSound;

    private bool movingLeft = true;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startXPosition = transform.position.x; // ���� ��ġ ����
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

        // ���� ��Ÿ���� ������ ���� ����
        if (Time.time >= nextAttackTime && inRange)
        {
            //�θ޶� 3������ ����
            if (currentBoomerangs < maxBoomerangs)
            {
                ShootProjectile();
                nextAttackTime = Time.time + attackCooldown; // ���� ���� �ð� ����
            }
        }

        // ���� ����
        SetDirectionAndAnimation();
    }

    // ���� �¿�� ���� ������ �ݺ��ؼ� �̵�
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

    // �÷��̾ ���� �߻�ü�� �߻�
    void ShootProjectile()
    {
        // �߻�ü ���� �� �÷��̾ ���� �߻�
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

    // ���� �׻� �÷��̾ �ٶ󺸵��� ������ ����
    void SetDirectionAndAnimation()
    {
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1); // �÷��̾ ���ʿ� ���� ��
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1); // �÷��̾ �����ʿ� ���� ��
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
