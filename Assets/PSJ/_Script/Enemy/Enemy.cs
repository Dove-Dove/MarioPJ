using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : MonoBehaviour
{
    //적 기본클래스
    // Move, Dead
    public enum State
    {
        Move, Dead, Shell, ShellMove, Idle
    }

    //대기 상태
    public State currentState = State.Idle;

    //상태를 변화하기 시작하는 플레이어와의 최소거리
    protected float range = 15f;
    
    //기본 속도
    public float moveSpeed = 3f;

    //레이케스트
    //레이어
    public LayerMask groundLayer;
    public LayerMask slopeLayer;
    public LayerMask noteLayer;
    //레이거리
    protected float rayDistance = 1f;
    //바닥 판정을 위한 전후위치
    public Transform groundDetect1, groundDetect2;

    //날개
    public bool hasWing;
    //점프상태확인용 변수
    private bool isJumping;
    
    //점프관련 변수
    protected float jumpForce = 5f;
    protected float jumpInterveal = 2f;
    protected float nextJumpTime;

    //리지드바디
    private Rigidbody2D rb;

    //좌우방향설정<-기본은 좌측
    public bool movingLeft = true;

    //애니메이터
    Animator animator;
    //날개오브젝트
    public GameObject wings;
    //오디오소스
    public AudioSource DeadSound;
    //정면블록확인용 레이케스트
    protected RaycastHit2D BlockCheck;

    //경사면 확인을 위한 변수
    protected float angle;
    protected Vector2 perp;
    protected bool isSlope;

    //마리오 꼬리와 관련딘 변수
    protected bool attackedbyTail;

    //플레이어 오브젝트
    protected GameObject player;

    //스코어 오브젝트
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

    //플레이어가 사거리에 들어오지 않았을 때 대기상태
    public void enemyIdle()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            currentState = State.Move;
        }
    }

    //기본적인 움직임 함수
    public void enemyMove()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;

        // 발판 확인
        RaycastHit2D groundleft = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D groundright = Physics2D.Raycast(groundDetect2.position, Vector2.down, rayDistance, groundLayer);

        bool isGrounded = groundleft || groundright;

        //경사확인
        RaycastHit2D hit = Physics2D.Raycast(groundDetect1.position, Vector2.down, rayDistance, groundLayer);
        RaycastHit2D fronthit;

        //좌우에 따른 정면레이케스트
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

        //전후면 경사 확인용 조건문
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

        //경사면에 따른 속도 조정
        if(!isSlope)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1));
        }
        else if(isSlope)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.Translate(new Vector2(perp.x * moveSpeed * Time.deltaTime * (movingLeft ? 1 : -1), perp.y * moveSpeed * Time.deltaTime *(movingLeft ? 1 : -1)));
        }

        //점프(날개가 있을때만 실행)
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

        //블록체크
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

    //경사면 체크 함수
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

    //방향전환용 함수
    public void Flip()
    {
        movingLeft = !movingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //점프 함수
    public void Jump()
    {
        isJumping = true;
        gameObject.GetComponent<Rigidbody2D>().velocity 
            = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, jumpForce);
    }

    //충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyWall")) //벽 충돌
        {
            Flip();
        }
        else if(collision.gameObject.CompareTag("Enemy")) //적 태그
        {
            Flip();
        }
        else if (collision.gameObject.name.Contains("Cliff") && currentState == State.Move) //절벽에서 떨어지지않도록 제한
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("MovingShell")) //움직이는 껍질
        {
            DeadSound.Play();
            hasWing = false;
            Score(gameObject,score);
            currentState = State.Dead;
        }
        else if (collision.gameObject.CompareTag("Shell")) //껍질
        {
            Flip();
        }
        else if (collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("Tail")) //플레이어 공격
        {
            DeadSound.Play();

            if (hasWing)
            {
                hasWing = false;
                currentState = State.Move;
            }
            else //플레이어 공격종류에 따른 처리
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
        else if(collision.gameObject.CompareTag("StarInvincible")) //플레이어 무적상태
        {
            DeadSound.Play();
            Jump();
            animator.SetTrigger("IsDead2");
            currentState = State.Dead;

        }
    }

    //죽음 상태
    public void enemyDead()
    {
        Invoke("offCollider", 0.3f);
        //Invoke("destroy", 1.0f);
        Destroy(gameObject, 1.0f);
    }

    //파괴
    void destroy()
    {
        gameObject.SetActive(false);
    }

    //충돌판정 비활성화
    void offCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    //스코어 시각화 및 처리
    public static void Score(GameObject gameObject,GameObject Score)
    {
        Vector3 scorePos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.8f, gameObject.transform.position.z);

        GameObject projectile = Instantiate(Score, scorePos, Quaternion.identity);
    }
}
