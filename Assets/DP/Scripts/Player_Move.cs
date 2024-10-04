using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_Move : MonoBehaviour
{
    //컴포넌트
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;
    public Camera mainCam;

    public float MaxSpeed=10.0f;

    public float LMrio_Jump_pow = 10;
    public float BMrio_Jump_pow = 10;

    //==마리오 확인용
    public bool isBigMario;
    public bool isLittleMario;
    //마리오 변신 확인용
    public bool isFireMario = false;
    public bool isRaccoonMario = false;
    //별 무적
    public bool isInvincibleStar = false;
    //히트
    public bool ishit = false;
    //마리오 방향
    public bool isRight = false;
    //공중에 있을 때
    public bool inAir;

    //==이동
    private Vector2 destination;
    private Vector2 curPos = Vector2.zero;
    public float maxSpeed = 10;
    public float curAnimSpeed;
    private bool isInputMove = false;
    private bool beginMove = false;
    private float timer = 0;
    public float animAccel = 0;
    //==애니메이션
    public UnityEngine.KeyCode curKey=KeyCode.None;


    //좌우
    public float input_x;
    //점프

    //중간중간 전체 애니메이션 멈춤제어하는 불형
    [SerializeField]
    private bool stopMoment;
    //가속도 게이지
    [SerializeField]
    private float acceleGauge = 20;
    [SerializeField]
    private int marioHp;
    [SerializeField]
    private bool isLookRight;

    private Vector2 marioPos;

    private void Awake()
    {
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;

        isBigMario = false;
        isLittleMario = true;

        animAccel = 8;

    }

    // Start is called before the first frame update
    void Start()
    {
        //시작 시 오른쪽으로
        FlipPlayer(true);
    }

    // Update is called once per frame
    void Update()
    {
        marioPos=rigid.position;
        //플레이어 이동불가 조건
        if(ishit)
        {
            return;
        }
        else
        {
            //방향전환 애니메이션용
            //if(curKey== KeyCode.RightArrow && Input.GetKey(KeyCode.LeftArrow))
            //    animator.SetBool("ChangeDirection", true);
            //if (curKey == KeyCode.LeftArrow && Input.GetKey(KeyCode.RightArrow))
            //    animator.SetBool("ChangeDirection", true);

            input_x = Input.GetAxis("Horizontal");
            animator.SetBool("ChangeDirection", false);
            //오른쪽 이동
            if (input_x > 0 && Input.GetKey(KeyCode.RightArrow))
            {
                //입력키 저장
                curKey = KeyCode.RightArrow;
                //이동버튼 입력 중
                isInputMove = true;
                //오른쪽이동버튼
                isRight = true; FlipPlayer(isRight);
                playerMove();
  
            }
            else if (isRight)
            {
                //상수를 이후 변수로
                if (curAnimSpeed > 0)
                {
                    curAnimSpeed -= curAnimSpeed * 0.01f;
                    if (curAnimSpeed <= 0.1f)
                    {
                        curAnimSpeed = 0;
                        animator.SetBool("isRun", false);
                    }
                    animator.SetFloat("Speed", curAnimSpeed);
                }

                //타이머 초기화
                timer = 0;
                Debug.Log("Right key up");
            }
            //우->좌 일때 방향전환 애니메이션
            if (curKey== KeyCode.RightArrow && Input.GetKey(KeyCode.LeftArrow))
                animator.SetBool("ChangeDirection", true);

            //왼쪽 이동
            if (input_x < 0 && Input.GetKey(KeyCode.LeftArrow))
            {
                //입력키 저장
                curKey = KeyCode.LeftArrow;
                isInputMove = true;
                isRight = false; FlipPlayer(isRight);
                playerMove();
            }
            else if(!isRight)
            {
                //상수를 이후 변수로
                if (curAnimSpeed > 0)
                {
                    curAnimSpeed -= curAnimSpeed * 0.01f;
                    if (curAnimSpeed <= 0.1f)
                    {
                        curAnimSpeed = 0;
                        animator.SetBool("isRun", false);
                    }
                    animator.SetFloat("Speed", curAnimSpeed);
                }

                //타이머 초기화
                timer = 0;
                Debug.Log("Right key up");
            }
            //좌->우 일때 방향전환 애니메이션
           if (curKey == KeyCode.LeftArrow && Input.GetKey(KeyCode.RightArrow))
                animator.SetBool("ChangeDirection", true);

        }

    }
    //플레이어 방향 수정
    void FlipPlayer(bool isRight)
    {
        if (!isRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
            transform.eulerAngles = new Vector3(0, -180, 0);

    }

    void playerMove()
    {
        //destination 위치를 결정
        Destination();
        //addforce
        if (isRight)
        {
            Debug.Log("addForce");
            float curSpeed = 5;
            var direction =new Vector2(curSpeed, 0);
            rigid.AddForce(direction);
            if(rigid.velocity.x>5)
                rigid.velocity = new Vector2(5,rigid.velocity.y);
        }
        else
        {
            Debug.Log("addForce");
            float curSpeed = 5;
            var direction = new Vector2(-curSpeed, 0);
            rigid.AddForce(direction);
            if (rigid.velocity.x < -5)
                rigid.velocity = new Vector2(-5, rigid.velocity.y);
        }
    }
    //누르는 시간에 따른 모션속도를 위한 함수
    //수정필요 물리addForce로 구현을 다시

    //키 입력시 위치 저장하고 그 위치에서의 거리에 따라 속도계산으로 움직임 표현
    void Destination()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            timer += Time.deltaTime;

            curAnimSpeed = timer * animAccel;
            //Debug.Log(curSpeed);
            //최고속도 고정
            if (curAnimSpeed > maxSpeed)
                curAnimSpeed = maxSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            timer += Time.deltaTime;

            curAnimSpeed = timer * animAccel;
            //Debug.Log(curSpeed);
            //최고속도 고정
            if (curAnimSpeed > maxSpeed)
                curAnimSpeed = maxSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }
    }
}
