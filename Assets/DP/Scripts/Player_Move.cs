using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;


public enum MarioStatus
{
    None,
    NormalMario,
    SuperMario,
    FireMario,
    RaccoonMario,
    Death,
    Clear
}

public class Player_Move : MonoBehaviour
{
    //컴포넌트
    [Header("==Components")]
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer sprite;
    public PhysicsMaterial2D physicsMaterial;
    public GameObject tail;
    public GameObject marioFoot;

    //오브젝트 가져오기용
    private GameObject tuttleShell;
    [Header("==Mario Ability")]
    public float LMrio_Jump_pow = 9f;
    public float SMrio_Jump_pow = 10f;
    public float mario_AddedJumpPowLevel = 2.5f;
    //시작리셋용
    public bool firstStartMarioSetting=false;
    //==마리오 확인용
    [Header("==Mario Check")]
    public bool isSuperMario;
    public bool isLittleMario;
    [SerializeField]
    private MarioStatus marioStatus = MarioStatus.NormalMario;
    private MarioStatus curStatus;
    public MarioStatus StartMarioStatus;

    //마리오 HP
    [Header("==Mario HP")]
    [SerializeField]
    private int marioHp;
    //마리오 변신 확인용
    [Header("==Mario ChangeCheck")]
    public bool isFireMario = false;
    public bool isRaccoonMario = false;
    //무적상태(히트 후)
    [Header("==Hit Invincible")]
    public bool isInvincible = false;
    //별 무적
    [Header("==Star Invincible")]
    public bool isInvincibleStar = false;
    public bool isInvincibleStarStart = false;

    //히트
    [Header("==Mario Hit")]
    public bool ishit = false;
    //입력불가 상태
    [Header("==Input impossible status")]
    [SerializeField]
    private bool notInput = false;
    public bool NotInput
    {
    get { return notInput; } 
    set { notInput = value; }
    }
    public bool isClear= false;
    //마리오 방향
    [Header("==Mario Direction")]
    public bool isRight = false;


    //==이동
    [Header("==Move")]
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
    private float angle;
    private Vector2 perp;

    //littleMario
    [Header("==Little Mario")]
    public float Velocity = 20;
    public float LimitVelocity = 4;
    public float addedLimitVelocity;
    public float addLimitVelocity = 3;
    //FireMario
    [Header("==Fire Mario")]
    public GameObject FireBall;
    public float fireSpeed = 10;
    public UnityEngine.Transform firePoint;
    private GameObject[] FireBalls=new GameObject[2];
    private bool isFireBall = false;

    //==애니메이션
    public UnityEngine.KeyCode curKey=KeyCode.None;
    //좌우
    [Header("==Axis")]
    public float input_x;
    //점프
    [Header("==Jump")]
    public bool isJump=false;
    public bool onAir=false;
    //점프시간 유지용
    public bool onceInputJumpBoutton=false;
    private bool noDoubleJump = false;
    public float jumpInputTime = 0.5f;
    private float jumpPower;
    public bool isJumpInput=true;
    //언덕
    [Header("==Slope")]
    [SerializeField]
    private bool onHill=false;
    //미끄러지기
    public bool isSilding = false;
    public float slideAddForcd=5f;
    public const float friction = 0.4f;
    public const float hillFriction = 0.1f;

    //앉기
    [Header("==Sitting")]
    public bool isSit = false;
    [SerializeField]
    private float groundRayLen=0;
    [SerializeField]
    private float hillRayLen=0;
    public float LMarioGroundRayLen = 0.3f;
    public float LMarioHillRayLen = 0.6f;
    public float SMarioGroundRayLen = 0.3f;
    public float SMarioHillRayLen = 0.6f;
    //공격
    [Header("==Attack")]
    public bool isEnemy=false;
    public bool isAttack = false;
    //기능(Z)
    [Header("==Button'Z'")]
    public bool isLift;
    public bool isCrushShell=false;
    //상태 전달용
    [Header("==Status Transmission")]
    public bool isKick=true;
    public bool isPipe=false;
    public bool isShellKick=false;

    //죽음
    [Header("==Death")]
    private Vector2 clearVelocity=Vector2.zero;

    //중간중간 전체 애니메이션 멈춤제어하는 불형
    [SerializeField]
    private bool stopMoment;



    private Vector2 marioPos;
    [Header("==Sound")]
    //===사운드
    public AudioSource jumpSound;
    public AudioSource turnSound;
    public AudioSource powerUpSound;
    private bool isPowerUp=false;
    public AudioSource hitSound;
    public bool ishitSound = false;
    public AudioSource deadSound;
    private bool isDeadSound=false;
    public AudioSource runSound;
    public AudioSource kickSound;
    public bool iskcikSound = false;
    public AudioSource tailAttackSound;
    private bool isTailAttackSound=false;
    public AudioSource FireSound;
    private bool isFireSound = false;
    public AudioSource PipeSound;
    private bool isPipeSound = false;
    //===이펙트
    [Header("==Effect")]
    [SerializeField]
    private float invisibleTimeCount1 = 0;
    private float invisibleTimeCount2 = 0;
    [SerializeField]
    private int invisibleCount = 0;

    public Color originalColor;
    public Color changeColor;

    private float cutTimeCount = 0;
    bool effectOn = true;
    public Color Color1;
    public Color Color2;
    public Color Color3;

    //기타
    [Header("==ETC")]
    public bool timeStop=false;
    private Vector2 LMarioHitboxSize = new Vector2(0.9f, 0.9f);
    private Vector2 SMarioHitboxSize = new Vector2(0.9f, 1.7f);
    //콜라이더 확인용 
    public bool isGetShell=false;
    //문
    public bool inDoor=false;
    //마리오별 특별기능용
    public bool isGlideButton=false;
    public bool isChargedP = false;
    public bool isUseP=false;
    [SerializeField]
    private float PCheckTimeCount=0;
    [SerializeField]
    private float PLimitTimeCount=0;
    public bool isNoteblock = false;
    public bool isNoteblockJump = false;


    private void Awake()
    {                                  
        hitBox = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();

        //회전 고정
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        hitBox.offset = new Vector2(0, 0.9f);
        isSuperMario = false;
        isLittleMario = true;

        onGround = true;

        //rigid.gravityScale = 2;

        animAccel = 8;
        jumpPower=LMrio_Jump_pow;


        //마리오 상태변화 감지용
        curStatus = marioStatus;
    }

    void Start()
    {
        //시작 시 오른쪽으로
        FlipPlayer(true);

        //마리오 시작 Status
        //상태 및 hp적용
        if (!firstStartMarioSetting)
        {
            StartMarioStatusAnim(StartMarioStatus);
        }

        //애니메이션 최고속도 설정
        addedMaxAnimSpeed =maxAnimSpeed;
        //이동최고속도 설정
        addedLimitVelocity = LimitVelocity;
        //스프라이트 점멸용 기존마리오 스프라이트 컬러저장
        //변신 시 새로 저장
        originalColor = sprite.material.color;
        isLift = false;
        //기본마리오일때 바닥감지
        groundRayLen=LMarioGroundRayLen;
        hillRayLen=LMarioHillRayLen;

    }

    // Update is called once per frame
    void Update()
    {
        marioPos =rigid.position;

        //플레이어 이동불가 조건
        if (ishit && !isInvincible &&!isInvincibleStar)
        {
            //dead확인
            if(marioHp <= 1)
            { 
                //마리오 사망처리
                if(!isDeadSound)
                {
                    marioStatus = MarioStatus.Death;
                    setChangeStatus();
                }
                //씬전환 혹은 사망신호
                animator.SetBool("isDead", true);
            }
            else
            { 
                //아니면 파워다운
                marioHp--; 
                ishit = false;
                //1초 무적
                isInvincible = true;
                StartCoroutine(HitInvincible());
                if (marioHp==1)
                {
                    //기본 마리오
                    marioStatus=MarioStatus.NormalMario;
                    setChangeStatus();
                    
                }
                else if (marioHp==2)
                {
                    marioStatus = MarioStatus.SuperMario;
                    setChangeStatus();
                }
            }

            if(!ishitSound && marioHp > 0 && marioStatus != MarioStatus.Death)
            {
                ishitSound = true;
                hitSound.Play();
            }
            return;
        }
        //입력불가 상황
        else if(notInput&& !isInvincibleStar)
        {

            if (curStatus != marioStatus)
            {
                curStatus = marioStatus;
                setChangeStatus();
                ChangeSuperMario();
            }
            return;

        }
        //클리어
        else if(isClear)
        {
            //땅위라면 이동 애니메이션 출력 + 이동
            MarioClear();
            return;
        }

        //기본 입력가능상태
        else
        {
            //별 무적상태
            if (isInvincibleStar)
            {
                StarInvisibleEffect();
            }
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
            //머리체크
            CheckHeadCrush();

           //==점프
           if (Input.GetKey(KeyCode.X) || isAttack || isNoteblockJump)
            {
                if (isAttack)
                { marioFoot.SetActive(true); Debug.Log("발판"); }
                if(isJumpInput)
                    Jump();
                StartCoroutine(FalseMarioFoot());
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
            //P 판단
            TurnOnP();
            //너구리 활공
            RMarioSpecialActtion();

            //슬라이드 
            if (isSilding)
            {
                if (rigid.velocity.x == 0 || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.X))
                {
                    isSilding = false;
                    animator.SetBool("isSlide", false);
                    physicsMaterial.friction=friction;
                }   
            }
            //타임스케일 테스트용()
            TimePause();

            //테스트용 마리오 변신시 사용
            if (curStatus !=marioStatus && firstStartMarioSetting)
            {
                
                curStatus=marioStatus;
                setChangeStatus();
                ChangeSuperMario();
            }
            //마리오 초기 설정 끝
            firstStartMarioSetting = true;
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            isInvincibleStar = true;
        }

        Debug.Log(isGetShell);
    }

    private void FixedUpdate()
    {
    }
    //====================함수==================//
    //==========================================//

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
            var direction =new Vector2(Velocity, 0);
            rigid.AddForce(direction);
            if(rigid.velocity.x> addedLimitVelocity)
                rigid.velocity = new Vector2(addedLimitVelocity, rigid.velocity.y);
        }
        else
        {
            var direction = new Vector2(-Velocity, 0);
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
        isJump = true;//점프시작 확인용
        jumpTimer += Time.deltaTime;
        //버튼입력확인용.
        onceInputJumpBoutton = true;
        //점프 입력 시간 제한
        if (jumpTimer > jumpInputTime)
        {
            onceInputJumpBoutton = false;
        }   
        //사운드 한번만 나오게
        if (Input.GetKeyDown(KeyCode.X) && !onAir)
        { 
            jumpSound.Play();
            animator.SetBool("isJump", true);
        }
        //바닦에 있을 때 애니메이션 이동
        if (onGround)
        {
            animator.SetBool("isJump", false);
        }

        jumpPower = LMrio_Jump_pow;
        //addforce
        if (onceInputJumpBoutton &&!noDoubleJump)
        {
            //Debug.Log("Jump");
            var direction = new Vector2(0, jumpPower);
            rigid.AddForce(direction, ForceMode2D.Impulse);
            //힘 제한
            //rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            //if (onHill)
            //    rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 2);
            if (rigid.velocity.y > jumpPower)
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
    }

    void CheckOnGround()
    {
        //디버그용
        //Debug.DrawRay(rigid.position, new Vector2(0,-groundRayLen), new Color(1,0,0));
        //Debug.DrawRay(rigid.position, new Vector2(0,-hillRayLen), new Color(0,1,0));

        RaycastHit2D groundHit = Physics2D.BoxCast(rigid.position, new Vector2(0.9f, 0.2f), 0, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (groundHit.collider != null)
        {
            //Debug.Log("onGround");
            onAir = false;
            onGround = true;//뱡향전환효과 온오프용
            isJump = false;//점프 시작 확인용
            noDoubleJump = false;//점프입력가능
            isJumpInput = true;
            animator.SetBool("isJump", false);
            animator.SetBool("onGround", true);
            rigid.gravityScale = 3;

            //앉기
            if (Input.GetKey(KeyCode.DownArrow) && input_x == 0&& !onHill)
            {
                hitBox.size = LMarioHitboxSize;
                hitBox.offset = new Vector2(0, 0.5f);
                animator.SetBool("isSit", true);
                //마리오 상태별 애니메이션으로 바로 전환
                switch (marioStatus)
                {
                    case MarioStatus.NormalMario:
                        break;
                    case MarioStatus.SuperMario:
                        animator.Play("SMario_sit");
                        break;
                    case MarioStatus.FireMario:
                        animator.Play("FMario_sit"); break;
                    case MarioStatus.RaccoonMario:
                        animator.Play("RMario_sit"); break;

                }
            }
            else
            {
                animator.SetBool("isSit", false);
                //히트박스 사이즈 조정
                if (marioStatus == MarioStatus.NormalMario)
                { hitBox.size = LMarioHitboxSize; hitBox.offset = new Vector2(0, 0.5f); }
                else
                { hitBox.size = SMarioHitboxSize; hitBox.offset = new Vector2(0, 0.9f); }
            }
        }
        else
        {
            onGround = false;
            onAir = true;
            animator.SetBool("isJump", true);
            animator.SetBool("onGround", false);
            rigid.gravityScale = 3;
        }

        //====언덕위에 있을 때 
        RaycastHit2D onDownhill = Physics2D.Raycast(transform.position + new Vector3(0, 1f,0), Vector2.down, hillRayLen+1f, LayerMask.GetMask("DownHill"));

        if (onDownhill.collider != null)
        {
            onGround = true;
            onAir = false;
            onHill = true;
            animator.SetBool("isJump", false);
            animator.SetBool("onGround", true);
            //오르막에서 중력값 조정
            if (!isSilding && onGround)
                rigid.gravityScale = 1;
            isJumpInput = true;

            //오를때 테스트
            //transform.position += Vector3.up * (0.1f - onDownhill.distance);

            //미끄러지기
            if (Input.GetKey(KeyCode.DownArrow) && !isSilding)
            {
                animator.SetBool("isSlide", true);
                isSilding = true;
                gameObject.tag = "PlayerAttack";
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                rigid.gravityScale = 3;
                if (isRight)
                {
                    rigid.velocity = new Vector2(slideAddForcd, rigid.velocity.y);
                    physicsMaterial.friction = hillFriction;
                }
                else
                {
                    rigid.velocity = new Vector2(-slideAddForcd, rigid.velocity.y);
                    physicsMaterial.friction = hillFriction;
                }
            }

            //언덕위에서 이동입력없으면 정지

            if (input_x == 0 && !isSilding && !onAir)
            {

                //멈추면 발판생성(작동안함) TODO:보류 수정필요.
                if(onHill)
                    marioFoot.SetActive(true);

                //미끄러지기
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    gameObject.tag = "PlayerAttack";
                    animator.SetBool("isSlide", true);
                    isSilding = true;
                    rigid.gravityScale = 3;

                    rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (isRight)
                    {
                        rigid.velocity = new Vector2(slideAddForcd, rigid.velocity.y);
                        physicsMaterial.friction = hillFriction;
                    }
                    else
                    {
                        rigid.velocity = new Vector2(-slideAddForcd, rigid.velocity.y);
                        physicsMaterial.friction = hillFriction;
                    }
                }
                else
                {
                    rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                }
            }
            else
            { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; marioFoot.SetActive(false); }
        }
        else
        { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; onHill = false; marioFoot.SetActive(false); }

        //====파이프 애니메이션 동작
        //필요시 머리도 감지해서 작동할 수 있도록
        RaycastHit2D downPipe   = Physics2D.Raycast(rigid.position, Vector2.down, 0.2f, LayerMask.GetMask("Pipe"));
        RaycastHit2D upPipe;
        if (marioStatus == MarioStatus.NormalMario)
        {
            Debug.DrawRay(rigid.position + new Vector2(0, 1), new Vector2(0, 0.2f), new Color(0, 0, 1));
            upPipe = Physics2D.Raycast(rigid.position + new Vector2(0, 1), Vector2.up, 0.2f, LayerMask.GetMask("Pipe"));
        }
        else
        {
            Debug.DrawRay(rigid.position + new Vector2(0, 1.6f), new Vector2(0, 0.2f), new Color(0, 0, 1));
            upPipe = Physics2D.Raycast(rigid.position + new Vector2(0, 1.6f), Vector2.up, 0.2f, LayerMask.GetMask("Pipe"));
        }
        RaycastHit2D leftPipe   = Physics2D.Raycast(rigid.position + new Vector2(-1, 0.5f), Vector2.left, 0.2f, LayerMask.GetMask("Pipe"));
        RaycastHit2D rightPipe  = Physics2D.Raycast(rigid.position + new Vector2(1, 0.5f), Vector2.right, 0.2f, LayerMask.GetMask("Pipe"));

        Debug.DrawRay(rigid.position, new Vector2(0, -0.2f), new Color(0, 0, 1));
        Debug.DrawRay(rigid.position + new Vector2(-1, 0.5f), new Vector2(0.2f / 2, 0), new Color(0, 0, 1));
        Debug.DrawRay(rigid.position + new Vector2(1, 0.5f), new Vector2(-0.2f / 2, 0), new Color(0, 0, 1));
        //아래 파이프
        if (downPipe.collider !=null)
        {
            //앉기
            if (Input.GetKey(KeyCode.DownArrow))
            {
                //timeScale을 조정할지는 차후
                //마리오 상태별 애니메이션으로 바로 전환
                switch (marioStatus)
                {
                    case MarioStatus.NormalMario:
                        animator.Play("Mario_inpipe");
                        PipeAction("Down");
                        isPipe =true;
                        break;
                    case MarioStatus.SuperMario:
                        animator.Play("SMario_inpipe");
                        PipeAction("Down");
                        isPipe = true;
                        break;
                    case MarioStatus.FireMario:
                        animator.Play("FMario_inpipe");
                        PipeAction("Down");
                        isPipe = true;
                        break;
                    case MarioStatus.RaccoonMario:
                        animator.Play("RMario_inpipe");
                        PipeAction("Down");
                        isPipe = true;
                        break;

                }
            }
            else
            {
                isPipe = false;
            }
        }
        //Up Pipe
        if (upPipe.collider != null)
        {
            //앉기
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //timeScale을 조정할지는 차후
                //마리오 상태별 애니메이션으로 바로 전환
                switch (marioStatus)
                {
                    case MarioStatus.NormalMario:
                        animator.Play("Mario_inpipe");
                        PipeAction("Up");
                        isPipe = true;
                        break;
                    case MarioStatus.SuperMario:
                        animator.Play("SMario_inpipe");
                        PipeAction("Up");
                        isPipe = true;
                        break;
                    case MarioStatus.FireMario:
                        animator.Play("FMario_inpipe");
                        PipeAction("Up");
                        isPipe = true;
                        break;
                    case MarioStatus.RaccoonMario:
                        animator.Play("RMario_inpipe");
                        PipeAction("Up");
                        isPipe = true;
                        break;

                }
            }
            else
            {
                isPipe = false;
            }
        }

        //left Pipe
        if (rightPipe.collider != null)
        {
            //앉기
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //timeScale을 조정할지는 차후
                //마리오 상태별 애니메이션으로 바로 전환
                switch (marioStatus)
                {
                    case MarioStatus.NormalMario:
                        animator.Play("LMario_run");
                        PipeAction("Right");
                        isPipe = true;
                        break;
                    case MarioStatus.SuperMario:
                        animator.Play("SMario_run");
                        PipeAction("Right");
                        isPipe = true;
                        break;
                    case MarioStatus.FireMario:
                        animator.Play("FMario_run");
                        PipeAction("Right");
                        isPipe = true;
                        break;
                    case MarioStatus.RaccoonMario:
                        animator.Play("RMario_run");
                        PipeAction("Right");
                        isPipe = true;
                        break;

                }
            }
            else
            {
                isPipe = false;
            }
        }

        //left Pipe
        if (leftPipe.collider != null)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //timeScale을 조정할지는 차후
                //마리오 상태별 애니메이션으로 바로 전환
                switch (marioStatus)
                {
                    case MarioStatus.NormalMario:
                        animator.Play("LMario_run");
                        PipeAction("Left");
                        isPipe = true;
                        break;
                    case MarioStatus.SuperMario:
                        animator.Play("SMario_run");
                        PipeAction("Left");
                        isPipe = true;
                        break;
                    case MarioStatus.FireMario:
                        animator.Play("FMario_run");
                        PipeAction("Left");
                        isPipe = true;
                        break;
                    case MarioStatus.RaccoonMario:
                        animator.Play("RMario_run");
                        PipeAction("Left");
                        isPipe = true;
                        break;

                }
            }
            else
            {
                isPipe = false;
            }
        }

        //노트블럭 감지
        RaycastHit2D onNoteBlock = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("NoteBlock"));
        if(onNoteBlock.collider != null)
        {
            if (Input.GetKey(KeyCode.X))
            {
                rigid.velocity =new Vector2(rigid.velocity.x, jumpPower * 2);
            }
        }

        //겹치므로 일단 보류
        //RaycastHit2D marioAttackHit = Physics2D.Raycast(rigid.position, Vector2.down, 0.8f, LayerMask.GetMask("Enemy"));
        //if(marioAttackHit.collider != null)
        //{
        //    isEnemy = true;
        //    gameObject.tag = "PlayerAttack";
        //}
        //else { isEnemy = false; gameObject.tag = "Player"; isAttack = false; }


        RaycastHit2D marioAttackHit2 = Physics2D.BoxCast(rigid.position,new Vector2(0.8f,0.2f), 0, Vector2.down, 0.3f, LayerMask.GetMask("Enemy"));
        RaycastHit2D marioAttackHit3 = Physics2D.BoxCast(rigid.position, new Vector2(0.8f, 0.2f), 0, Vector2.down, 0.3f, LayerMask.GetMask("MovingShell"));
        if (marioAttackHit2.collider != null || marioAttackHit3.collider != null)
        {
            isEnemy = true;
            if(!isInvincibleStar)
                gameObject.tag = "PlayerAttack";
        }
        else
        {
            isEnemy = false;
            isAttack = false;

            if (!isSilding && !isInvincibleStar)
                { gameObject.tag = "Player"; }
        }
        //항상 tag=player로 만드는 곳
        //슬라이딩과 무적시엔 테그 바뀌도록


        DrawBoxCast(rigid.position, new Vector2(0.8f, 0.2f), 0, Vector2.down, 0.3f, marioAttackHit2);


        RaycastHit2D marioNoteBlockJumpHit = Physics2D.BoxCast(rigid.position, new Vector2(0.8f, 0.2f), 0, Vector2.down, 0.3f, LayerMask.GetMask("NoteBlock"));
        if (marioNoteBlockJumpHit.collider != null)
        {
            isEnemy = true;
        }
    }

    void CheckHeadCrush()
    {
        float len;
        if (marioStatus == MarioStatus.NormalMario)
        { len = 0.5f; }
        else
        { len = 0.9f; }


        RaycastHit2D marioHeadBoxHit = Physics2D.Raycast(rigid.position + new Vector2(-0.4f, 1), Vector2.up, len, LayerMask.GetMask("Box"));
        RaycastHit2D marioHeadBoxHit2 = Physics2D.Raycast(rigid.position + new Vector2(0.4f, 1), Vector2.up, len, LayerMask.GetMask("Box"));


        if (marioHeadBoxHit.collider != null || marioHeadBoxHit2.collider != null )
        {
            isJumpInput=false;
        }

        //디버그용
        Debug.DrawRay(rigid.position + new Vector2(-0.4f, 1), new Vector2(0, len), new Color(1, 0, 0));
        Debug.DrawRay(rigid.position + new Vector2(0.4f, 1), new Vector2(0, len), new Color(1, 0, 0));

    }

    public void UpdateMarioStatusAndHP(MarioStatus status)
    {
        marioStatus = status;
        switch(marioStatus)
        {
            case MarioStatus.None:
                break;
            case MarioStatus.NormalMario:
                marioHp = 1; break;
            case MarioStatus.SuperMario:
                isSuperMario = true;
                marioHp = 2; break;
            case MarioStatus.FireMario:
                marioHp = 3; break;
            case MarioStatus.RaccoonMario:
                marioHp = 3; break;
        }

        //TODO: 필요하면 스테이터스에 맞는 효과작성
    }

    //===Z키(액션키)==
    void InputActionButton()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //킥 안되도록
            isKick = false;
            isLift = true;
            //아이템 들기
            if (isLift && isCrushShell)
            {
                if (GameObject.Find("Mario").GetComponent<MarioCollision>().shell != null)
                {
                    tuttleShell = GetComponentInChildren<MarioCollision>().shell;
                    //방향결정
                    if (isRight)
                    {
                        if(tuttleShell.GetComponent<Tuttle>().currentState == Enemy.State.Shell)
                            tuttleShell.transform.position = marioPos + new Vector2(0.8f, 0.5f);

                        tuttleShell.GetComponent<Tuttle>().movingLeft = false;
                    }
                    else
                    {
                        if (tuttleShell.GetComponent<Tuttle>().currentState == Enemy.State.Shell)
                            tuttleShell.transform.position = marioPos + new Vector2(-0.8f, 0.5f);

                        tuttleShell.GetComponent<Tuttle>().movingLeft = true;
                    }
                    animator.SetBool("isLift", true);
                }
            }

            if (onGround)
                addedLimitVelocity = LimitVelocity + addLimitVelocity;
            //액션키 누르면 최대속도 addAnimSpeed 만큼 추가
            //공중이 아닐 때
            if (!onAir)
                addedMaxAnimSpeed = maxAnimSpeed + addAnimSpeed;
            //이동 시
            if (curAnimSpeed > maxAnimSpeed)
            {
                animator.SetBool("inputActionButton", true);
                //TODO:한번끝나고 한번재생하는 형태로
                runSound.Play();
            }

            //Debug.Log("Input 'Z'button");
            if(!isCrushShell)
            { switch (marioStatus)
                {
                    //상태에 따른 
                    case MarioStatus.NormalMario:
                        break;
                    //슈퍼마리오
                    case MarioStatus.SuperMario:
                        break;
                    //불꽃마리오
                    case MarioStatus.FireMario:
                        //TODO:다시 함수로 만들기
                        if (Input.GetKeyDown(KeyCode.Z))
                        {
                            ShootFire();
                        }
                        else
                        {
                            isFireBall = false;
                            animator.SetBool("isInputX", false);
                        }
                        break;
                    //너구리마리오
                    case MarioStatus.RaccoonMario:
                        animator.SetBool("isTailAttack", true);
                        if (Input.GetKeyDown(KeyCode.Z))
                            animator.Play("RMario_tailattack");
                        if (!isTailAttackSound)
                        {
                            isTailAttackSound = true;
                            tailAttackSound.Play();
                        }
                        break;
                }
            }
        }
        else if(isCrushShell)//원상복귀
        {
            //z키 누르면 리프트 가능상태
            addedLimitVelocity = LimitVelocity;
            addedMaxAnimSpeed = maxAnimSpeed;
            animator.SetBool("inputActionButton", false);
            animator.SetBool("isTailAttack", false);
            isTailAttackSound = false;
            runSound.Pause();
            isKick = true;
            //
            Debug.Log("쉘발사");
            if (isLift)
            {
                //쉘이동시키기
                if(tuttleShell)
                    tuttleShell.GetComponent<Tuttle>().currentState = Enemy.State.ShellMove;
                if (tuttleShell != null)
                {
                    tuttleShell = null;
                    //킥 사운드
                    kickSound.Play();
                }
                //isKick = true;
                isLift = false;
                animator.SetBool("isLift", false);
                isCrushShell = false;
            }
        }
        else
        {
            //z키 누르면 리프트 가능상태
            addedLimitVelocity = LimitVelocity;
            addedMaxAnimSpeed = maxAnimSpeed;
            animator.SetBool("inputActionButton", false);
            animator.SetBool("isTailAttack", false);
            isTailAttackSound = false;
        }
        if(!isLift)
        {
            tuttleShell = null;
        }

    }

    void ChangeSuperMario()
    {
        //각종 수치 변경
        //사운드 출력
        if (!isPowerUp)
        {
            if(marioStatus==MarioStatus.Clear)
                { return; }
            isPowerUp = true;
        }
        Time.timeScale = 0;
        StartCoroutine(MarioChangeTimeStart());
    }
    IEnumerator MarioChangeTimeStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        notInput = false;
        isPowerUp = false;
    }
    //마리오 변신상태 get set
    public void setMarioStatus(MarioStatus marioForm)
    {
        marioStatus = marioForm;
    }
    public MarioStatus getMarioStatus() { return marioStatus; }

    //히트후 무적
    IEnumerator HitInvincible()
    {
        //깜박임& 무적
        marioBlink();
        yield return new WaitForSeconds(1f);
        isInvincible = false;
        Debug.Log("무적끝");
    }

    //마리오 변신 초기화
    public void setChangeStatus()
    {
        switch (marioStatus)
        {
            case MarioStatus.NormalMario:
                animator.SetBool("ChangeSuperMario", false);
                animator.Play("SuperMario_2_LMario");
                animator.SetBool("ChangeRaccoonMario", false);
                animator.SetBool("ChangeFireMario", false);
                UpdateMarioStatusAndHP(marioStatus);
                SetLMario();
                break;
            case MarioStatus.SuperMario:
                SetSMario();
                animator.SetBool("ChangeSuperMario", true);
                animator.Play("LMario_2_SuperMario");
                animator.SetBool("ChangeRaccoonMario", false);
                animator.SetBool("ChangeFireMario", false);
                UpdateMarioStatusAndHP(marioStatus);
                break;
            case MarioStatus.RaccoonMario:
                animator.SetBool("ChangeSuperMario", false);
                animator.Play("SmokeEffect");
                animator.SetBool("ChangeRaccoonMario", true);
                animator.SetBool("ChangeFireMario", false);
                UpdateMarioStatusAndHP(marioStatus);
                SetSMario();
                break;
            case MarioStatus.FireMario:
                animator.SetBool("ChangeSuperMario", false);
                animator.Play("FMario_idle");
                animator.SetBool("ChangeRaccoonMario", false);
                animator.SetBool("ChangeFireMario", true);
                ChangeFireMario();
                UpdateMarioStatusAndHP(marioStatus);
                SetSMario();
                break;

            case MarioStatus.Death:
                animator.Play("Mario_Dead");
                animator.SetBool("ChangeSuperMario", false);
                animator.SetBool("ChangeRaccoonMario", false);
                animator.SetBool("ChangeFireMario", false);
                MarioDeath();
                break;
            case MarioStatus.Clear:
                isClear = true; break;
        }
    }
     void SetLMario()
    {
        jumpInputTime = 0.4f;
        jumpPower = LMrio_Jump_pow;
        hitBox.size= LMarioHitboxSize;
        hitBox.offset = new Vector2(0, 0.9f);
        groundRayLen = LMarioGroundRayLen;
        hillRayLen = LMarioHillRayLen;
    }

    void SetSMario()
    {
        jumpInputTime = 0.5f;
        jumpPower = SMrio_Jump_pow;
        hitBox.size = SMarioHitboxSize;
        hitBox.offset = new Vector2(0, 0.9f);
        groundRayLen = SMarioGroundRayLen;
        hillRayLen = SMarioHillRayLen;
    }
    //꼬리 히트박스켜고 끄기
    public void onTailHitbox()
    {
        tail.SetActive(true);  
    }

    public void offTailHitbox()
    {
        tail.SetActive(false);
    }

    void TimePause()
    {
        if (Input.GetKeyDown(KeyCode.P) && !timeStop)
        {
            Time.timeScale = 0;
            timeStop = true;
            //Debug.Log("Time Stop");
        }
        else if (Input.GetKeyDown(KeyCode.P) && timeStop)
        {
            Time.timeScale = 1;
            timeStop = false;
            //Debug.Log("Time Start");
        }
    }

    void MarioDeath()
    {
        isDeadSound = true;
        Time.timeScale = 0;
        deadSound.Play();
        //히트박스끄기
        hitBox.enabled = false;
        //마리오가 제일 앞으로
        sprite.sortingOrder = 99;
        StartCoroutine(StartDeathAnim());

    }

    IEnumerator StartDeathAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // 올라갈 때
        float h = 0;
        float addH = 0;
        float targetHeightUp = 5f;  // 목표 높이
        float moveSpeedUp = 10f;     // 일정한 속도

        while (addH < targetHeightUp)
        {
            h = moveSpeedUp * Time.unscaledDeltaTime;  // 고정된 속도에 따른 움직임
            transform.position = new Vector2(transform.position.x, transform.position.y + h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        // 내려갈 때
        h = 0;
        addH = 0;
        float targetHeightDown = 20f;  // 목표 하강 거리
        float moveSpeedDown = 15f;      // 일정한 속도 (하강 시)

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // 고정된 속도에 따른 하강
            transform.position = new Vector2(transform.position.x, transform.position.y - h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    //파이프용 액션
    //"Down" "Up" "Left" "Right"
    public void PipeAction(String dir)
    {
        if (!isPipe)
        {
            //Time.timeScale = 0;
            //히트박스끄기
            //hitBox.enabled = false;
            if (dir == "Down")
                StartCoroutine(StartDownPipeAnim());
            else if (dir == "Up")
                StartCoroutine(StartUpPipeAnim());
            else if (dir == "Right")
                StartCoroutine(StartRightPipeAnim());
            else if (dir == "Left")
                StartCoroutine(StartLeftPipeAnim());

            isPipe = true;
            StartCoroutine(FalseInPipe());
        }

    }

    IEnumerator StartDownPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // 내려갈 때
        float h = 0;
        float addH = 0;
        float targetHeightDown = 2.5f;  // 목표 하강 거리
        float moveSpeedDown = 2f;      // 일정한 속도 (하강 시)

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // 고정된 속도에 따른 하강
            transform.position = new Vector2(transform.position.x, transform.position.y - h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator StartUpPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // 내려갈 때
        float h = 0;
        float addH = 0;
        float targetHeightDown = 2.5f;  // 목표 하강 거리
        float moveSpeedDown = 2f;      // 일정한 속도 (하강 시)

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // 고정된 속도에 따른 하강
            transform.position = new Vector2(transform.position.x, transform.position.y + h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator StartRightPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // 내려갈 때
        float h = 0;
        float addH = 0;
        float targetHeightDown = 0.5f;  // 목표 하강 거리
        float moveSpeedDown = 1f;      // 일정한 속도 (하강 시)

        isRight = true;
        curAnimSpeed = addedMaxAnimSpeed;

        //Anim :run
        animator.SetBool("isRun", true);
        moveTimer += Time.unscaledDeltaTime;

        curAnimSpeed = moveTimer * animAccel;
        //최고속도 고정
        if (curAnimSpeed > addedMaxAnimSpeed)
            curAnimSpeed = addedMaxAnimSpeed;

        animator.SetFloat("Speed", curAnimSpeed);

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // 고정된 속도에 따른 하강
            transform.position = new Vector2(transform.position.x + h, transform.position.y );
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }

    }

    IEnumerator StartLeftPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // 내려갈 때
        float h = 0;
        float addH = 0;
        float targetHeightDown = 2.5f;  // 목표 하강 거리
        float moveSpeedDown = 2f;      // 일정한 속도 (하강 시)

        isRight = true;
        curAnimSpeed = addedMaxAnimSpeed;

        //Anim :run
        animator.SetBool("isRun", true);
        animator.SetFloat("Speed", 5f);
        switch (marioStatus)
        {
            case MarioStatus.NormalMario:
                animator.Play("LMario_move");
                break;
            case MarioStatus.SuperMario:
                animator.Play("SMario_move");
                break;
            case MarioStatus.FireMario:
                animator.Play("FMario_move");
                break;
            case MarioStatus.RaccoonMario:
                animator.Play("RMario_move");
                break;
        }

        moveTimer += Time.unscaledDeltaTime;

        curAnimSpeed = moveTimer * animAccel;
        //최고속도 고정
        if (curAnimSpeed > addedMaxAnimSpeed)
            curAnimSpeed = addedMaxAnimSpeed;

        animator.SetFloat("Speed", curAnimSpeed);

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // 고정된 속도에 따른 하강
            transform.position = new Vector2(transform.position.x - h, transform.position.y);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }

    }
    IEnumerator FalseInPipe()
    {
        yield return new WaitForSeconds(0.5f);
        isPipe = false;
    }
    //P 차지및 판단
    void TurnOnP()
    {
        //p값이 최고속도값이 도달하고
        //TODO:충돌 시 P꺼지게 
        var limitP = LimitVelocity + addLimitVelocity-0.5f;
        if (limitP < Math.Abs(rigid.velocity.x))
        {
            PCheckTimeCount += Time.deltaTime;
            if(PCheckTimeCount>0.5f)
                isChargedP = true;

            if(isChargedP && Input.GetKeyDown(KeyCode.X) && onGround)
            { 
                isUseP = true;
                animator.SetBool("isUseP",true);
            }
        }
        else
        {
            PCheckTimeCount = 0;
            isChargedP = false;
        }
        if(isUseP)
        {
            PLimitTimeCount += Time.deltaTime;
        }
        if (isUseP && PLimitTimeCount > 4)
        {
            isUseP = false;
            PLimitTimeCount = 0;
            animator.SetBool("isUseP", false);
        }
    }

    //너구리 마리오 활공
    void RMarioSpecialActtion()
    {
        //너구리마리오일 때 만 사용가능
        if(marioStatus!=MarioStatus.RaccoonMario)
            return;

        if(Input.GetKey(KeyCode.X) &&onAir)
        {
            if(!isUseP)
            {
                if (!isGlideButton && rigid.velocity.y < 0)
                {
                    animator.SetBool("isInputX", true);

                    var direction = new Vector2(0, 290);
                    isGlideButton = true;
                    rigid.AddForce(direction, ForceMode2D.Force);
                    StartCoroutine(ButtonAvailable());
                }
            }
            else if(isUseP)
            {
                if (!isGlideButton )
                {
                    animator.SetBool("isInputX", true);

                    Debug.Log("P");
                    var direction = new Vector2(0, 11);
                    isGlideButton = true;
                    rigid.AddForce(direction, ForceMode2D.Impulse);
                    StartCoroutine(ButtonAvailable2());
                }
            }   
        }
        else if(Input.GetKeyUp(KeyCode.X))
        {

        }
    }

    IEnumerator ButtonAvailable()
    {
        yield return new WaitForSeconds(0.13f);
        isGlideButton = false;
        animator.SetBool("isInputX", false);
    }
    IEnumerator ButtonAvailable2()
    {
        yield return new WaitForSeconds(0.15f);
        isGlideButton = false;
        animator.SetBool("isInputX", false);
    }
    //불꽃 마리오
    void ShootFire()
    {
        //불 두개가 발사중이면
        MarioFire[] fires = FindObjectsOfType<MarioFire>();
        if(fires.Length > 1)
        { return; }
        
        Vector2 direction;
        // 발사체 생성 및 플레이어를 향해 발사
        GameObject projectile = Instantiate(FireBall, firePoint.position, Quaternion.identity);
        if(isRight)
            direction = new Vector2(1,-1);
        else
            direction = new Vector2(-1, -1);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * fireSpeed;

        if (!isFireBall)
        {
            isFireBall = true;
            animator.SetBool("isInputX", true);
        }
    }

    //클리어
    void MarioClear()
    {
        gameObject.tag = "Untagged";
        CheckOnGround();
        Time.timeScale = 0;
        if (onGround)
        {
            isRight = true;
            curAnimSpeed = addedMaxAnimSpeed;

            //Anim :run
            animator.SetBool("isRun", true);
            moveTimer += Time.unscaledDeltaTime;

            curAnimSpeed = moveTimer * animAccel;
            //최고속도 고정
            if (curAnimSpeed > addedMaxAnimSpeed)
                curAnimSpeed = addedMaxAnimSpeed;

            animator.SetFloat("Speed", curAnimSpeed);

            var direction = new Vector2(10, 0);
            rigid.AddForce(direction);
            rigid.velocity = direction;


            Vector3 move = new Vector3(1, 0, 0) * 3 * Time.unscaledDeltaTime;

            transform.position += move;
        }
        else 
        {
            //수동으로 Phsics돌리기
            if (Time.timeScale == 0)
            {
                ApplyCustomGravity();
            }
        }
    }

    //timeScale=0일 때 사용
    void ApplyCustomGravity()
    {
        // 중력 벡터를 Rigidbody2D의 속도에 적용
        clearVelocity += Vector2.down * 9.8f * Time.unscaledDeltaTime;
        Vector2 newPosition = rigid.position + clearVelocity * Time.unscaledDeltaTime;
        rigid.transform.position = newPosition;
    }

    //문이동 비활성화
    public IEnumerator FlaseInDoor()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        inDoor = false;
        Debug.Log(inDoor);
    }

    //
    IEnumerator FalseMarioFoot()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        marioFoot.SetActive(false);
    }

    //===Effect
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

        if(count<5)
        {
            StartCoroutine (Blink(count+1, !makeBlink));
        }
        else
        {
            ishit = false;
            ishitSound = false;
        }
    }

    //무적효과
    public void StarInvisibleEffect()
    {
        if (invisibleTimeCount1 > 7)
        {
            Debug.Log("무적끝");
            StopCoroutine("Blink");
            GetComponent<SpriteRenderer>().material.color = originalColor;
            invisibleTimeCount1 = 0;
            isInvincibleStar = false;
            //태그원상복귀
            gameObject.tag = "Player";
            //애니메이터 끄기
            animator.SetBool("isInvincibleStar", false);
            notInput = false;
            invisibleCount = 0;
        }
        else
        {
            invisibleTimeCount1 += Time.deltaTime;
            //태그변화
            gameObject.tag = "StarInvincible";
            //애니메이터 켜기
            animator.SetBool("isInvincibleStar", true);
            StartCoroutine(StarEffect());
            invisibleCount++;
        }
    }

    private IEnumerator StarEffect()
    {
        if (0 == invisibleCount % 3)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(originalColor.r, originalColor.g, 0.5f - originalColor.b, 255);
        }
        else if (1 == invisibleCount % 3)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(102, originalColor.g, 102, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }

        yield return new WaitForSecondsRealtime(0.1f);
    }
    //파이어 마리오 이펙트
    //TODO
    public void ChangeFireMario()
    {
        if (invisibleTimeCount2 > 7)
        {
            StopCoroutine("Blink");
            StopCoroutine("ChangeFireMarioEffect");  // 코루틴 정지
            GetComponent<SpriteRenderer>().material.color = originalColor; // 원래 색으로 변경
            invisibleTimeCount2 = 0;
            effectOn = false;
            invisibleCount = 0;
        }
        else
        {
            invisibleTimeCount2 += Time.unscaledDeltaTime;

            // 코루틴이 한 번만 실행되도록 설정
            if (!effectOn)
            {
                StartCoroutine(ChangeFireMarioEffect());
                effectOn = true;
            }

            invisibleCount++;
        }
    }

    private IEnumerator ChangeFireMarioEffect()
    {
        while (true)  // 코루틴이 반복적으로 실행되도록 설정
        {
            if (0 == invisibleCount % 3)
            {
                GetComponent<SpriteRenderer>().material.color = new Color(originalColor.r, 0.5f - originalColor.g, 0.5f - originalColor.b, originalColor.a);
            }
            else if (1 == invisibleCount % 3)
            {
                GetComponent<SpriteRenderer>().material.color = new Color(102 / 255f, originalColor.g, 102 / 255f, originalColor.a);
            }
            else
            {
                GetComponent<SpriteRenderer>().material.color = originalColor;
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    //마리오 시작 애니메이션 결정용 함수
    public void StartMarioStatusAnim(MarioStatus status=MarioStatus.NormalMario)
    {
        switch(status)
        {
            case MarioStatus.NormalMario:
                animator.Play("LMario_idle");
                UpdateMarioStatusAndHP(MarioStatus.NormalMario);
                break;
            case MarioStatus.SuperMario:
                animator.Play("SMario_idle");
                UpdateMarioStatusAndHP(MarioStatus.SuperMario);
                break;
            case MarioStatus.FireMario:
                animator.Play("FMario_idle");
                UpdateMarioStatusAndHP(MarioStatus.FireMario);
                break;
            case MarioStatus.RaccoonMario:
                animator.Play("RMario_idle");
                UpdateMarioStatusAndHP(MarioStatus.RaccoonMario);
                break;
        }
        curStatus = marioStatus;
    }

    // 박스캐스트 디버그용
    void DrawBoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, RaycastHit2D hit)
    {
        // Calculate the four corners of the box at the origin
        Vector2 halfSize = size / 2;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Get the corner points (top-left, top-right, bottom-left, bottom-right)
        Vector2 topLeft = (Vector2)(rotation * new Vector3(-halfSize.x, halfSize.y, 0)) + origin;
        Vector2 topRight = (Vector2)(rotation * new Vector3(halfSize.x, halfSize.y, 0)) + origin;
        Vector2 bottomLeft = (Vector2)(rotation * new Vector3(-halfSize.x, -halfSize.y, 0)) + origin;
        Vector2 bottomRight = (Vector2)(rotation * new Vector3(halfSize.x, -halfSize.y, 0)) + origin;

        // Move the box along the direction of the cast
        Vector2 castOffset = direction.normalized * distance;

        // Draw the box at the starting position (before cast)
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);

        // Draw the box at the end position (after cast)
        Debug.DrawLine(topLeft + castOffset, topRight + castOffset, Color.red);
        Debug.DrawLine(topRight + castOffset, bottomRight + castOffset, Color.red);
        Debug.DrawLine(bottomRight + castOffset, bottomLeft + castOffset, Color.red);
        Debug.DrawLine(bottomLeft + castOffset, topLeft + castOffset, Color.red);

        // If the box hits something, mark the hit point
        if (hit.collider != null)
        {
            Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.blue);  // Draw the hit point and normal
        }
    }

}

