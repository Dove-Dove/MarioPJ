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

    public float LMrio_LowJump_pow = 8f;
    public float LMrio_TopJump_pow = 12.5f;
    public float BMrio_LowJump_pow = 10f;
    public float BMrio_TopJump_pow = 17.5f;
    public float mario_AddedJumpPowLevel = 2.5f;
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
    public float maxAnimSpeed = 10;
    public float curAnimSpeed;
    private bool isInputMove = false;
    private bool beginMove = false;
    public bool onGround;
    private float moveTimer = 0;
    private float jumpTimer = 0;
    public float animAccel = 0;

    //littleMario
    public float LMVelocity = 10;
    public float LMAccel = 8;
    //==애니메이션
    public UnityEngine.KeyCode curKey=KeyCode.None;


    //좌우
    public float input_x;
    //점프
    public bool isJump=false;
    public bool onAir=false;
    private bool onceInputJumpBoutton=false;
    public float jumpInputTime = 0.5f;
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

    //===사운드
    public AudioSource jumpSound;
    public AudioSource turnSound;
    public AudioSource growUpSound;
    public AudioSource hitUpSound;
    public AudioSource deadSound;


    private void Awake()
    {
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //회전 고정
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        isBigMario = false;
        isLittleMario = true;
        //TODO:이후 레이로 바닦확인
        onGround = true;

        //rigid.gravityScale = 2;

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

        marioPos =rigid.position;
        //플레이어 이동불가 조건
        if(ishit)
        {
            return;
        }
        else
        {
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
                    curAnimSpeed -= curAnimSpeed * 0.1f;
                    if (curAnimSpeed <= 0.1f)
                    {
                        curAnimSpeed = 0;
                        animator.SetBool("isRun", false);
                    }
                    animator.SetFloat("Speed", curAnimSpeed);
                }

                //타이머 초기화
                moveTimer = 0;
            }
            //우->좌 일때 방향전환 애니메이션
            if (curKey== KeyCode.RightArrow && Input.GetKey(KeyCode.LeftArrow))
                { 
                animator.SetBool("ChangeDirection", true);
                //속도가 3이하이고 점프중이 아닐 때
                if (rigid.velocity.x > 3 && onGround)
                    turnSound.Play();
                }

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
                    curAnimSpeed -= curAnimSpeed * 0.1f;
                    if (curAnimSpeed <= 0.1f)
                    {
                        curAnimSpeed = 0;
                        animator.SetBool("isRun", false);
                    }
                    animator.SetFloat("Speed", curAnimSpeed);
                }

                //타이머 초기화
                moveTimer = 0;
            }
            //좌->우 일때 방향전환 애니메이션
           if (curKey == KeyCode.LeftArrow && Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("ChangeDirection", true);
                if(rigid.velocity.x < -3 && onGround)
                turnSound.Play();
            }
            //바닦체크
            CheckOnGround();

           //==점프
           if (Input.GetKey(KeyCode.X))
            {
                Jump();
            }
           else if(Input.GetKeyUp(KeyCode.X))
            { 
                onceInputJumpBoutton = false;
                jumpTimer = 0;
            }

            Debug.Log(input_x);
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
            var direction =new Vector2(LMVelocity, 0);
            rigid.AddForce(direction);
            if(rigid.velocity.x> LMAccel)
                rigid.velocity = new Vector2(LMAccel, rigid.velocity.y);
        }
        else
        {
            var direction = new Vector2(-LMVelocity, 0);
            rigid.AddForce(direction);
            if (rigid.velocity.x < -LMAccel)
                rigid.velocity = new Vector2(-LMAccel, rigid.velocity.y);
        }
    }
    //누르는 시간에 따른 모션속도를 위한 함수
    //수정필요 물리addForce로 구현을 다시

    //키 입력시 위치 저장하고 그 위치에서의 거리에 따라 속도계산으로 움직임 표현
    void Destination()
    {
        //오른쪽 이동 시
        if(Input.GetKey(KeyCode.RightArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            moveTimer += Time.deltaTime;

            curAnimSpeed = moveTimer * animAccel;
            //최고속도 고정
            if (curAnimSpeed > maxAnimSpeed)
                curAnimSpeed = maxAnimSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            moveTimer += Time.deltaTime;

            curAnimSpeed = moveTimer * animAccel;
            //최고속도 고정
            if (curAnimSpeed > maxAnimSpeed)
                curAnimSpeed = maxAnimSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }
    }

    void Jump()
    {
        //TODO:다시 조정
        jumpTimer += Time.deltaTime;
        //버튼입력확인용.
        onceInputJumpBoutton = true;
        //점프 입력 시간 제한
        if (jumpTimer > jumpInputTime)
            onceInputJumpBoutton = false;
        //사운드 한번만 나오게
        if (Input.GetKeyDown(KeyCode.X))
            { 
            jumpSound.Play();
            animator.SetBool("IsJump", true);
            //+ 체공시간에따라 점프자세유지
        }

        float jumpPower = LMrio_LowJump_pow;
        Debug.Log(jumpPower);
        //addforce
        if (onceInputJumpBoutton)
        {        
            Debug.Log("Jump");
            var direction = new Vector2(0, jumpPower);
            rigid.AddForce(direction,ForceMode2D.Impulse);
            //힘 제한
            if (rigid.velocity.y > jumpPower)
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
    }

    void CheckOnGround()
    {
        //디버그용
        Debug.DrawRay(rigid.position, new Vector2(0,-0.6f), new Color(1,0,0));

        RaycastHit2D groundHit = Physics2D.Raycast(rigid.position, Vector2.down, 0.6f,LayerMask.GetMask("Ground"));
        if(groundHit.collider !=null)
        {
            Debug.Log("onGround");
            onAir = false;
            onGround = true;//뱡향전환효과 온오프용
            animator.SetBool("IsJump", false);
        }
        else
        {
            onGround = false;
            onAir = true;
            animator.SetBool("IsJump", true);
        }

        RaycastHit2D onDownhill = Physics2D.Raycast(rigid.position, Vector2.down, 1f, LayerMask.GetMask("DownHill"));
        if (onDownhill.collider != null)
        {
            Debug.Log(onDownhill.collider.name);
            onGround = true;
            animator.SetBool("IsJump", false);

            //언덕위에서 이동입력없으면 정지
            if (input_x == 0)
            { rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; }
            else
            { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; }

            

        }
    }


}
