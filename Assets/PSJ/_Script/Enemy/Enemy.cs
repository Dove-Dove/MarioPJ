using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : MonoBehaviour
{
    //�� �⺻Ŭ����
    // Move, Dead
    public enum State
    {
        Move, Dead, Shell, ShellMove, Idle
    }

    //��� ����
    public State currentState = State.Idle;

    //���¸� ��ȭ�ϱ� �����ϴ� �÷��̾���� �ּҰŸ�
    protected float range = 15f;
    
    //�⺻ �ӵ�
    public float moveSpeed = 3f;

    //�����ɽ�Ʈ
    //���̾�
    public LayerMask groundLayer;
    public LayerMask slopeLayer;
    public LayerMask noteLayer;
    //���̰Ÿ�
    protected float rayDistance = 1f;
    //�ٴ� ������ ���� ������ġ
    public Transform groundDetect1, groundDetect2;

    //����
    public bool hasWing;
    //��������Ȯ�ο� ����
    private bool isJumping;
    
    //�������� ����
    protected float jumpForce = 5f;
    protected float jumpInterveal = 2f;
    protected float nextJumpTime;

    //������ٵ�
    private Rigidbody2D rb;

    //�¿���⼳��<-�⺻�� ����
    public bool movingLeft = true;

    //�ִϸ�����
    Animator animator;
    //����������Ʈ
    public GameObject wings;
    //������ҽ�
    public AudioSource DeadSound;
    //������Ȯ�ο� �����ɽ�Ʈ
    protected RaycastHit2D BlockCheck;

    //���� Ȯ���� ���� ����
    protected float angle;
    protected Vector2 perp;
    protected bool isSlope;

    //������ ������ ���õ� ����
    protected bool attackedbyTail;

    //�÷��̾� ������Ʈ
    protected GameObject player;

    //���ھ� ������Ʈ
    public GameObject score;
    //protected GameManager gameManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time + jumpInterveal;
        player = GameObject.Find("Mario");
        currentState = State.Idle;
        //gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                enemyIdle();
                break;

            case State.Move:
                enemyMove();
                break;

            case State.Dead:
                enemyDead();
                break;
        }
        if (!hasWing)
        {
            wings.SetActive(false);
        }

    }

    //�÷��̾ ��Ÿ��� ������ �ʾ��� �� ������
    public void enemyIdle()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            currentState = State.Move;
        }
    }

    //�⺻���� ������ �Լ�
    public void enemyMove()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;

        // ���� Ȯ��
        RaycastHit2D groundleft = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D groundright = Physics2D.Raycast(groundDetect2.position, Vector2.down, rayDistance, groundLayer);

        bool isGrounded = groundleft || groundright;

        //���Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D fronthit;

        //�¿쿡 ���� ���鷹���ɽ�Ʈ
        if (movingLeft)
        {
            fronthit = Physics2D.Raycast(groundDetect1.position, Vector2.left, 0.1f, slopeLayer);
            //Debug.DrawLine(fronthit.point ,Vector2.left, Color.magenta);
        }
        else
        {
            fronthit = Physics2D.Raycast(groundDetect2.position, Vector2.right, 0.1f, slopeLayer);
            //Debug.DrawLine(fronthit.point, Vector2.right, Color.magenta);
        }

        //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
        //Debug.DrawLine(hit.point, hit.point + perp, Color.red);

        //���ĸ� ��� Ȯ�ο� ���ǹ�
        if (hit || fronthit)
        {
            if (fronthit)
            {
                //Debug.Log("angle = " + angle);
                slopeChk(fronthit);
            }
            else
                slopeChk(hit);
        }

        //���鿡 ���� �ӵ� ����
        if(!isSlope)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
        }
        else if(isSlope)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.Translate(new Vector2(perp.x * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1), perp.y * moveSpeed * Time.deltaTime *(movingLeft ? 1 : -1)));
        }

        //����(������ �������� ����)
        if (!isGrounded)
        {
            if(hasWing && Time.time >= nextJumpTime && !isJumping)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
                isJumping = false;
            }
            else if(!hasWing)
            {
                //Flip();
                nextJumpTime = Time.time + jumpInterveal;
            }
        }
        else
        {
            if(hasWing && Time.time >= nextJumpTime && !isJumping)
            {
                Jump();
                nextJumpTime = Time.time + jumpInterveal;
                isJumping = false;
            }
        }

        //���üũ
        RaycastHit2D blockCheck;
        RaycastHit2D noteCheck;

        if(movingLeft)
        {
            blockCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.left, 0.8f, groundLayer);
            noteCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.left, 0.8f, noteLayer);
        }
        else
        {
            blockCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 0.8f, groundLayer);
            noteCheck = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 0.8f, noteLayer);
        }

        if (blockCheck)
        {
            if(blockCheck.collider.CompareTag("Box"))
            {
                Flip();
            }
            else if(blockCheck.collider.name.Contains("Spawn"))
            {

                Flip();
            }
        }

        if (noteCheck)
            Flip();
    }

    //���� üũ �Լ�
    public void slopeChk(RaycastHit2D hit)
    {
        perp = Vector2.Perpendicular(hit.normal).normalized;
        angle = Vector2.Angle(hit.normal, Vector2.up);

        if (angle != 0)
        {
            isSlope = true;
        }
        else
        {
            isSlope = false;
        }

    }

    //������ȯ�� �Լ�
    public void Flip()
    {
        movingLeft = !movingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //���� �Լ�
    public void Jump()
    {
        isJumping = true;
        gameObject.GetComponent<Rigidbody2D>().velocity 
            = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, jumpForce);
    }

    //�浹 ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall")) //�� �浹
        {
            Flip();
        }
        else if(collision.gameObject.CompareTag("Enemy")) //�� �±�
        {
            Flip();
        }
        else if (collision.gameObject.name.Contains("Cliff") && currentState == State.Move) //�������� ���������ʵ��� ����
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("MovingShell")) //�����̴� ����
        {
            DeadSound.Play();
            hasWing = false;
            Score(gameObject,score);
            currentState = State.Dead;
        }
        else if (collision.gameObject.CompareTag("Shell")) //����
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("Tail")) //�÷��̾� ����
        {
            DeadSound.Play();

            if (hasWing)
            {
                hasWing = false;
                currentState = State.Move;
            }
            else //�÷��̾� ���������� ���� ó��
            {
                if(collision.gameObject.CompareTag("PlayerAttack"))
                {
                    animator.SetTrigger("IsDead");
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.zero;
                    Score(gameObject, score);

                }
                else if (collision.gameObject.CompareTag("Tail"))
                {
                    Jump();
                    animator.SetTrigger("IsDead2");
                    Score(gameObject, score);

                }
                currentState = State.Dead;
            }
        }
        else if(collision.gameObject.CompareTag("StarInvincible")) //�÷��̾� ��������
        {
            DeadSound.Play();
            Jump();
            animator.SetTrigger("IsDead2");
            currentState = State.Dead;

        }
    }

    //���� ����
    public void enemyDead()
    {
        Invoke("offCollider", 0.3f);
        //Invoke("destroy", 1.0f);
        Destroy(gameObject, 1.0f);
    }

    //�ı�
    void destroy()
    {
        gameObject.SetActive(false);
    }

    //�浹���� ��Ȱ��ȭ
    void offCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    //���ھ� �ð�ȭ �� ó��
    public static void Score(GameObject gameObject,GameObject Score)
    {
        Vector3 scorePos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.8f, gameObject.transform.position.z);

        GameObject projectile = Instantiate(Score, scorePos, Quaternion.identity);
    }
}
