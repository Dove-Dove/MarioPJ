using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum MarioStatus
{
    None,
    NormalMario,
    SuperMario,
    FireMario,
    RaccoonMario,
    InvincibleMario
}

public class Player_Move : MonoBehaviour
{
    //컴포넌트
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer sprite;

    public float LMrio_LowJump_pow = 9f;
    public float LMrio_TopJump_pow = 12.5f;
    public float BMrio_LowJump_pow = 10f;
    public float BMrio_TopJump_pow = 15f;
    public float mario_AddedJumpPowLevel = 2.5f;
    //==마리오 확인용
    public bool isSuperMario;
    public bool isLittleMario;
    [SerializeField]
    private MarioStatus marioStatus = MarioStatus.NormalMario;
    //마리오 HP
    [SerializeField]
    private uint hp;
    //마리오 변신 확인용
    public bool isFireMario = false;
    public bool isRaccoonMario = false;
    //별 무적
    public bool isInvincibleStar = false;
    //히트
    public bool ishit = false;
    //입력불가 상태
    [SerializeField]
    private bool notInput = false;
    public bool NotInput
    {
    get { return notInput; } 
    set { notInput = value; }
    }
    [SerializeField]
    private bool isClear= false;
    //마리오 방향
    public bool isRight = false;
    //공중에 있을 때
    public bool inAir;

    //==이동
    private Vector2 destination;
    private Vector2 curPos = Vector2.zero;
    public float maxAnimSpeed = 10;
    public float addedMaxAnimSpeed=0;
    public float curAnimSpeed=0;
    public float addAnimSpeed;
    private bool isInputMove = false;
    private bool beginMove = false;
    public bool onGround;
    private float moveTimer = 0;
    private float jumpTimer = 0;
    public float animAccel = 0;

    //littleMario
    public float LMVelocity = 20;
    public float LMLimitVelocity = 4;
    public float addedLimitVelocity;
    public float addLimitVelocity = 3;
    //==애니메이션
    public UnityEngine.KeyCode curKey=KeyCode.None;

    //좌우
    public float input_x;
    //점프
    public bool isJump=false;
    public bool onAir=false;
    private bool onceInputJumpBoutton=false;
    private bool noDoubleJump = false;
    public float jumpInputTime = 0.5f;
    //미끄러지기
    public bool isSilding = false;
    //기능(Z)
    private bool isLift
    { get { return isLift; } set { isLift = value; }}

    //중간중간 전체 애니메이션 멈춤제어하는 불형
    [SerializeField]
    private bool stopMoment;
    //가속도 게이지
    [SerializeField]
    private float acceleGauge = 0;    
    [SerializeField]
    private float maxAcceleGauge = 20;
    [SerializeField]
    private int marioHp;
    [SerializeField]
    private bool isLookRight;

    private Vector2 marioPos;

    //===사운드
    public AudioSource jumpSound;
    public AudioSource turnSound;
    public AudioSource powerUpSound;
    private bool isPowerUp=false;
    public AudioSource hitSound;
    private bool ishitSound = false;
    public AudioSource deadSound;
    private bool isDeadSound=false;
    public AudioSource runSound;
    //===이펙트
    private Color originalColor;

    //기타
    public bool timeStop=false;
    private void Awake()
    {
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();

        //회전 고정
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        isSuperMario = false;
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
        //상태 및 hp적용
        UpdateMarioStatusAndHP(MarioStatus.NormalMario);
        //애니메이션 최고속도 설정
        addedMaxAnimSpeed=maxAnimSpeed;
        //이동최고속도 설정
        addedLimitVelocity = LMLimitVelocity;
        //스프라이트 점멸용 기존마리오 스프라이트 컬러저장
        //변신 시 새로 저장
        originalColor = sprite.material.color;
        isLift = false;
    }

    // Update is called once per frame
    void Update()
    {
        marioPos =rigid.position;
        //플레이어 이동불가 조건
        if(ishit)
        {
            //dead확인
            Debug.Log("HP :"+marioHp);
            if(marioHp<=0)
            { 
                //마리오 사망처리
                if(!isDeadSound)
                {
                    isDeadSound = true;
                    deadSound.Play();
                }
                //씬전환 혹은 사망신호
                animator.SetBool("isDead", true);
            }
            else
            { 
                marioHp--; 
                ishit = false;
            }


            if(!ishitSound)
            {
                ishitSound = true;
                hitSound.Play();
            }
            marioBlink();
            
            return;
        }
        //입력불가 상황
        else if(notInput)
        {
            Debug.Log("notInput");
            ChangeSuperMario();
            return;
        }
        //클리어
        else if(isClear)
        { return; }
        //기본 입력가능상태
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
                    curAnimSpeed -= curAnimSpeed * 1f;
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
                    curAnimSpeed -= curAnimSpeed * 1f;
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
                //더블점프 방지
                noDoubleJump = true;
                jumpTimer = 0;
            }
            //=='Z'버튼
            InputActionButton();
            //슬라이드 
            if (isSilding)
            {
                if (rigid.velocity.x == 0 || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.X))
                {
                    isSilding = false;
                    animator.SetBool("isSit", false);
                }   
            }
            //타임스케일 테스트용()
            if(Input.GetKeyDown(KeyCode.P) && !timeStop)
            { 
                Time.timeScale = 0;
                timeStop = true;
                Debug.Log("Time Stop");
            }
            else if (Input.GetKeyDown(KeyCode.P) && timeStop)
            {
                Time.timeScale = 1;
                timeStop = false;
                Debug.Log("Time Start");
            }


            Debug.Log(marioStatus);
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
            if(rigid.velocity.x> addedLimitVelocity)
                rigid.velocity = new Vector2(addedLimitVelocity, rigid.velocity.y);
        }
        else
        {
            var direction = new Vector2(-LMVelocity, 0);
            rigid.AddForce(direction);
            if (rigid.velocity.x < -addedLimitVelocity)
                rigid.velocity = new Vector2(-addedLimitVelocity, rigid.velocity.y);
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
            if (curAnimSpeed > addedMaxAnimSpeed)
                curAnimSpeed = addedMaxAnimSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            moveTimer += Time.deltaTime;

            curAnimSpeed = moveTimer * animAccel;
            //최고속도 고정
            if (curAnimSpeed > addedMaxAnimSpeed)
                curAnimSpeed = addedMaxAnimSpeed;

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
        if (Input.GetKeyDown(KeyCode.X) && !onAir)
            { 
            jumpSound.Play();
            animator.SetBool("isJump", true);
            //+ 체공시간에따라 점프자세유지
        }

            float jumpPower = LMrio_LowJump_pow;
        Debug.Log(jumpPower);
        //addforce
        if (onceInputJumpBoutton &&!noDoubleJump)
        {
            Debug.Log("Jump");
            var direction = new Vector2(0, jumpPower);
            rigid.AddForce(direction, ForceMode2D.Impulse);
            //힘 제한
            if (rigid.velocity.y > jumpPower)
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
    }

    void CheckOnGround()
    {
        //디버그용
        Debug.DrawRay(rigid.position, new Vector2(0,-0.6f), new Color(1,0,0));
        Debug.DrawRay(rigid.position, new Vector2(0,-1.2f), new Color(0,1,0));

        RaycastHit2D groundHit = Physics2D.Raycast(rigid.position, Vector2.down, 0.6f,LayerMask.GetMask("Ground"));
        if(groundHit.collider !=null)
        {
            Debug.Log("onGround");
            onAir = false;
            onGround = true;//뱡향전환효과 온오프용
            noDoubleJump = false;//점프입력가능
            animator.SetBool("isJump", false);
        }
        else
        {
            onGround = false;
            onAir = true;
            animator.SetBool("isJump", true);
        }
        //언덕위에 있을 때 
        RaycastHit2D onDownhill = Physics2D.Raycast(rigid.position, Vector2.down, 1.1f, LayerMask.GetMask("DownHill"));
        if (onDownhill.collider != null)
        {
            Debug.Log(onDownhill.collider.name);
            onGround = true;
            onAir = false;
            animator.SetBool("isJump", false);

            //미끄러지기
            if (Input.GetKey(KeyCode.DownArrow) && !isSilding)
            {
                animator.SetBool("isSit", true);
                isSilding = true;
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation; 
                if(isRight)
                    rigid.velocity = new Vector2(0.1f, rigid.velocity.y);
                else
                    rigid.velocity = new Vector2(-0.1f, rigid.velocity.y);
            }

            //언덕위에서 이동입력없으면 정지
            //TODO:슬라이드 작동되도록
            if (input_x == 0 &&!isSilding &&!onAir)
            { 
                //미끄러지기
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    animator.SetBool("isSit", true);
                    isSilding = true;
                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (isRight)
                        rigid.velocity = new Vector2(0.1f, rigid.velocity.y);
                    else
                        rigid.velocity = new Vector2(-0.1f, rigid.velocity.y);
                }
                else
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; }

        }
    }

    public void UpdateMarioStatusAndHP(MarioStatus status)
    {
        marioStatus = status;
        switch(marioStatus)
        {
            case MarioStatus.None:
                break;
            case MarioStatus.NormalMario:
                originalColor = sprite.material.color;
                marioHp = 1; break;
            case MarioStatus.SuperMario:
                originalColor = sprite.material.color;
                isSuperMario = true;
                marioHp = 2; break;
            case MarioStatus.FireMario:
                marioHp = 2; break;
            case MarioStatus.RaccoonMario:
                marioHp = 2; break;
            case MarioStatus.InvincibleMario:
                isInvincibleStar=true; break;
        }

        //TODO: 필요하면 스테이터스에 맞는 효과작성
    }

    //===Z키(액션키)==
    void InputActionButton()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Input 'Z'button");
            switch (marioStatus)
            {
                //상태에 따른 최대속도변경
                case MarioStatus.NormalMario:
                    addedLimitVelocity = LMLimitVelocity + addLimitVelocity;
                    //액션키 누르면 최대속도 addAnimSpeed 만큼 추가
                    addedMaxAnimSpeed = maxAnimSpeed + addAnimSpeed;
                    //이동 시
                    if(curAnimSpeed > maxAnimSpeed)
                    {
                        animator.SetBool("inputActionButton", true);
                        //TODO:한번끝나고 한번제생하는 형태로
                        runSound.Play();
                    }
                    //아이템 들기
                    break;
                    //슈퍼마리오
                case MarioStatus.SuperMario:
                    break;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Z))//원상복귀
        {
            addedLimitVelocity = LMLimitVelocity;
            addedMaxAnimSpeed = maxAnimSpeed;
            animator.SetBool("inputActionButton", false);
            runSound.Pause();
        }
    }

    void ChangeSuperMario()
    {
        if (isSuperMario)
        {
            //각종 수치 변경
            Debug.Log("SuperMario!!");
            //사운드 출력
            if (!isPowerUp)
            {
                powerUpSound.Play();
                isPowerUp = true;
            }
            Time.timeScale = 0;
            StartCoroutine(MarioChangeTimeStart());
        }
    }
    IEnumerator MarioChangeTimeStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        notInput = false;
        isPowerUp = false;
        Debug.Log("EndChange MarioForm");
    }
    //마리오 변신상태 get set
    public void setMarioTransform(MarioStatus marioForm)
    {
        marioStatus = marioForm;
    }
    MarioStatus getMarioStatus() { return marioStatus; }
    //Effect
    public void marioBlink()
    {
        StartCoroutine(Blink(0, true));
    }

    private IEnumerator Blink(int count, bool makeBlink)
    {       
        if (makeBlink)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }
        
        yield return new WaitForSecondsRealtime(0.2f);

        if(count<3)
        {
            StartCoroutine (Blink(count+1, !makeBlink));
        }
        else
        {
            ishit = false;
            ishitSound = false;
        }
    }
}

