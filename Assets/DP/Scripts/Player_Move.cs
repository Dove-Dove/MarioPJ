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
    //������Ʈ
    [Header("==Components")]
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer sprite;
    public PhysicsMaterial2D physicsMaterial;
    public GameObject tail;
    public GameObject marioFoot;

    //������Ʈ ���������
    private GameObject tuttleShell;
    [Header("==Mario Ability")]
    public float LMrio_Jump_pow = 9f;
    public float SMrio_Jump_pow = 10f;
    public float mario_AddedJumpPowLevel = 2.5f;
    //���۸��¿�
    public bool firstStartMarioSetting=false;
    //==������ Ȯ�ο�
    [Header("==Mario Check")]
    public bool isSuperMario;
    public bool isLittleMario;
    [SerializeField]
    private MarioStatus marioStatus = MarioStatus.NormalMario;
    private MarioStatus curStatus;
    public MarioStatus StartMarioStatus;

    //������ HP
    [Header("==Mario HP")]
    [SerializeField]
    private int marioHp;
    //������ ���� Ȯ�ο�
    [Header("==Mario ChangeCheck")]
    public bool isFireMario = false;
    public bool isRaccoonMario = false;
    //��������(��Ʈ ��)
    [Header("==Hit Invincible")]
    public bool isInvincible = false;
    //�� ����
    [Header("==Star Invincible")]
    public bool isInvincibleStar = false;
    public bool isInvincibleStarStart = false;

    //��Ʈ
    [Header("==Mario Hit")]
    public bool ishit = false;
    //�ԷºҰ� ����
    [Header("==Input impossible status")]
    [SerializeField]
    private bool notInput = false;
    public bool NotInput
    {
    get { return notInput; } 
    set { notInput = value; }
    }
    public bool isClear= false;
    //������ ����
    [Header("==Mario Direction")]
    public bool isRight = false;


    //==�̵�
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

    //==�ִϸ��̼�
    public UnityEngine.KeyCode curKey=KeyCode.None;
    //�¿�
    [Header("==Axis")]
    public float input_x;
    //����
    [Header("==Jump")]
    public bool isJump=false;
    public bool onAir=false;
    //�����ð� ������
    public bool onceInputJumpBoutton=false;
    private bool noDoubleJump = false;
    public float jumpInputTime = 0.5f;
    private float jumpPower;
    public bool isJumpInput=true;
    //���
    [Header("==Slope")]
    [SerializeField]
    private bool onHill=false;
    //�̲�������
    public bool isSilding = false;
    public float slideAddForcd=5f;
    public const float friction = 0.4f;
    public const float hillFriction = 0.1f;

    //�ɱ�
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
    //����
    [Header("==Attack")]
    public bool isEnemy=false;
    public bool isAttack = false;
    //���(Z)
    [Header("==Button'Z'")]
    public bool isLift;
    public bool isCrushShell=false;
    //���� ���޿�
    [Header("==Status Transmission")]
    public bool isKick=true;
    public bool isPipe=false;
    public bool isShellKick=false;

    //����
    [Header("==Death")]
    private Vector2 clearVelocity=Vector2.zero;

    //�߰��߰� ��ü �ִϸ��̼� ���������ϴ� ����
    [SerializeField]
    private bool stopMoment;



    private Vector2 marioPos;
    [Header("==Sound")]
    //===����
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
    //===����Ʈ
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

    //��Ÿ
    [Header("==ETC")]
    public bool timeStop=false;
    private Vector2 LMarioHitboxSize = new Vector2(0.9f, 0.9f);
    private Vector2 SMarioHitboxSize = new Vector2(0.9f, 1.7f);
    //�ݶ��̴� Ȯ�ο� 
    public bool isGetShell=false;
    //��
    public bool inDoor=false;
    //�������� Ư����ɿ�
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

        //ȸ�� ����
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        hitBox.offset = new Vector2(0, 0.9f);
        isSuperMario = false;
        isLittleMario = true;

        onGround = true;

        //rigid.gravityScale = 2;

        animAccel = 8;
        jumpPower=LMrio_Jump_pow;


        //������ ���º�ȭ ������
        curStatus = marioStatus;
    }

    void Start()
    {
        //���� �� ����������
        FlipPlayer(true);

        //������ ���� Status
        //���� �� hp����
        if (!firstStartMarioSetting)
        {
            StartMarioStatusAnim(StartMarioStatus);
        }

        //�ִϸ��̼� �ְ�ӵ� ����
        addedMaxAnimSpeed =maxAnimSpeed;
        //�̵��ְ�ӵ� ����
        addedLimitVelocity = LimitVelocity;
        //��������Ʈ ����� ���������� ��������Ʈ �÷�����
        //���� �� ���� ����
        originalColor = sprite.material.color;
        isLift = false;
        //�⺻�������϶� �ٴڰ���
        groundRayLen=LMarioGroundRayLen;
        hillRayLen=LMarioHillRayLen;

    }

    // Update is called once per frame
    void Update()
    {
        marioPos =rigid.position;

        //�÷��̾� �̵��Ұ� ����
        if (ishit && !isInvincible &&!isInvincibleStar)
        {
            //deadȮ��
            if(marioHp <= 1)
            { 
                //������ ���ó��
                if(!isDeadSound)
                {
                    marioStatus = MarioStatus.Death;
                    setChangeStatus();
                }
                //����ȯ Ȥ�� �����ȣ
                animator.SetBool("isDead", true);
            }
            else
            { 
                //�ƴϸ� �Ŀ��ٿ�
                marioHp--; 
                ishit = false;
                //1�� ����
                isInvincible = true;
                StartCoroutine(HitInvincible());
                if (marioHp==1)
                {
                    //�⺻ ������
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
        //�ԷºҰ� ��Ȳ
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
        //Ŭ����
        else if(isClear)
        {
            //������� �̵� �ִϸ��̼� ��� + �̵�
            MarioClear();
            return;
        }

        //�⺻ �Է°��ɻ���
        else
        {
            //�� ��������
            if (isInvincibleStar)
            {
                StarInvisibleEffect();
            }
            input_x = Input.GetAxis("Horizontal");
            animator.SetBool("ChangeDirection", false);
            //������ �̵�
            if (input_x > 0 && Input.GetKey(KeyCode.RightArrow))
            {
                //�Է�Ű ����
                curKey = KeyCode.RightArrow;
                //�̵���ư �Է� ��
                isInputMove = true;
                //�������̵���ư
                isRight = true; FlipPlayer(isRight);
                playerMove();
  
            }
            else if (isRight)
            {
                //����� ���� ������
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

                //Ÿ�̸� �ʱ�ȭ
                moveTimer = 0;
            }
            //��->�� �϶� ������ȯ �ִϸ��̼�
            if (curKey== KeyCode.RightArrow && Input.GetKey(KeyCode.LeftArrow))
                { 
                animator.SetBool("ChangeDirection", true);
                //�ӵ��� 3�����̰� �������� �ƴ� ��
                if (rigid.velocity.x > 3 && onGround)
                    turnSound.Play();
                }

            //���� �̵�
            if (input_x < 0 && Input.GetKey(KeyCode.LeftArrow))
            {
                //�Է�Ű ����
                curKey = KeyCode.LeftArrow;
                isInputMove = true;
                isRight = false; FlipPlayer(isRight);
                playerMove();
            }
            else if(!isRight)
            {
                //����� ���� ������
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

                //Ÿ�̸� �ʱ�ȭ
                moveTimer = 0;
            }
            //��->�� �϶� ������ȯ �ִϸ��̼�
           if (curKey == KeyCode.LeftArrow && Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("ChangeDirection", true);
                if(rigid.velocity.x < -3 && onGround)
                turnSound.Play();
            }

            //�ٴ�üũ
            CheckOnGround();
            //�Ӹ�üũ
            CheckHeadCrush();

           //==����
           if (Input.GetKey(KeyCode.X) || isAttack || isNoteblockJump)
            {
                if (isAttack)
                { marioFoot.SetActive(true); Debug.Log("����"); }
                if(isJumpInput)
                    Jump();
                StartCoroutine(FalseMarioFoot());
            }
           else if(Input.GetKeyUp(KeyCode.X))
            { 
                onceInputJumpBoutton = false;
                //�������� ����
                noDoubleJump = true;
                jumpTimer = 0;

            }
            //=='Z'��ư
            InputActionButton();
            //P �Ǵ�
            TurnOnP();
            //�ʱ��� Ȱ��
            RMarioSpecialActtion();

            //�����̵� 
            if (isSilding)
            {
                if (rigid.velocity.x == 0 || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.X))
                {
                    isSilding = false;
                    animator.SetBool("isSlide", false);
                    physicsMaterial.friction=friction;
                }   
            }
            //Ÿ�ӽ����� �׽�Ʈ��()
            TimePause();

            //�׽�Ʈ�� ������ ���Ž� ���
            if (curStatus !=marioStatus && firstStartMarioSetting)
            {
                
                curStatus=marioStatus;
                setChangeStatus();
                ChangeSuperMario();
            }
            //������ �ʱ� ���� ��
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
    //====================�Լ�==================//
    //==========================================//

    //�÷��̾� ���� ����
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
        //destination ��ġ�� ����
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
    //������ �ð��� ���� ��Ǽӵ��� ���� �Լ�
    //�����ʿ� ����addForce�� ������ �ٽ�

    //Ű �Է½� ��ġ �����ϰ� �� ��ġ������ �Ÿ��� ���� �ӵ�������� ������ ǥ��
    void Destination()
    {
        //������ �̵� ��
        if(Input.GetKey(KeyCode.RightArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            moveTimer += Time.deltaTime;

            curAnimSpeed = moveTimer * animAccel;
            //�ְ�ӵ� ����
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
            //�ְ�ӵ� ����
            if (curAnimSpeed > addedMaxAnimSpeed)
                curAnimSpeed = addedMaxAnimSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }
    }

    void Jump()
    {
        //TODO:�ٽ� ����
        isJump = true;//�������� Ȯ�ο�
        jumpTimer += Time.deltaTime;
        //��ư�Է�Ȯ�ο�.
        onceInputJumpBoutton = true;
        //���� �Է� �ð� ����
        if (jumpTimer > jumpInputTime)
        {
            onceInputJumpBoutton = false;
        }   
        //���� �ѹ��� ������
        if (Input.GetKeyDown(KeyCode.X) && !onAir)
        { 
            jumpSound.Play();
            animator.SetBool("isJump", true);
        }
        //�ٴۿ� ���� �� �ִϸ��̼� �̵�
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
            //�� ����
            //rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
            //if (onHill)
            //    rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 2);
            if (rigid.velocity.y > jumpPower)
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
    }

    void CheckOnGround()
    {
        //����׿�
        //Debug.DrawRay(rigid.position, new Vector2(0,-groundRayLen), new Color(1,0,0));
        //Debug.DrawRay(rigid.position, new Vector2(0,-hillRayLen), new Color(0,1,0));

        RaycastHit2D groundHit = Physics2D.BoxCast(rigid.position, new Vector2(0.9f, 0.2f), 0, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (groundHit.collider != null)
        {
            //Debug.Log("onGround");
            onAir = false;
            onGround = true;//������ȯȿ�� �¿�����
            isJump = false;//���� ���� Ȯ�ο�
            noDoubleJump = false;//�����Է°���
            isJumpInput = true;
            animator.SetBool("isJump", false);
            animator.SetBool("onGround", true);
            rigid.gravityScale = 3;

            //�ɱ�
            if (Input.GetKey(KeyCode.DownArrow) && input_x == 0&& !onHill)
            {
                hitBox.size = LMarioHitboxSize;
                hitBox.offset = new Vector2(0, 0.5f);
                animator.SetBool("isSit", true);
                //������ ���º� �ִϸ��̼����� �ٷ� ��ȯ
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
                //��Ʈ�ڽ� ������ ����
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

        //====������� ���� �� 
        RaycastHit2D onDownhill = Physics2D.Raycast(transform.position + new Vector3(0, 1f,0), Vector2.down, hillRayLen+1f, LayerMask.GetMask("DownHill"));

        if (onDownhill.collider != null)
        {
            onGround = true;
            onAir = false;
            onHill = true;
            animator.SetBool("isJump", false);
            animator.SetBool("onGround", true);
            //���������� �߷°� ����
            if (!isSilding && onGround)
                rigid.gravityScale = 1;
            isJumpInput = true;

            //������ �׽�Ʈ
            //transform.position += Vector3.up * (0.1f - onDownhill.distance);

            //�̲�������
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

            //��������� �̵��Է¾����� ����

            if (input_x == 0 && !isSilding && !onAir)
            {

                //���߸� ���ǻ���(�۵�����) TODO:���� �����ʿ�.
                if(onHill)
                    marioFoot.SetActive(true);

                //�̲�������
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

        //====������ �ִϸ��̼� ����
        //�ʿ�� �Ӹ��� �����ؼ� �۵��� �� �ֵ���
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
        //�Ʒ� ������
        if (downPipe.collider !=null)
        {
            //�ɱ�
            if (Input.GetKey(KeyCode.DownArrow))
            {
                //timeScale�� ���������� ����
                //������ ���º� �ִϸ��̼����� �ٷ� ��ȯ
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
            //�ɱ�
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //timeScale�� ���������� ����
                //������ ���º� �ִϸ��̼����� �ٷ� ��ȯ
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
            //�ɱ�
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //timeScale�� ���������� ����
                //������ ���º� �ִϸ��̼����� �ٷ� ��ȯ
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
                //timeScale�� ���������� ����
                //������ ���º� �ִϸ��̼����� �ٷ� ��ȯ
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

        //��Ʈ�� ����
        RaycastHit2D onNoteBlock = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("NoteBlock"));
        if(onNoteBlock.collider != null)
        {
            if (Input.GetKey(KeyCode.X))
            {
                rigid.velocity =new Vector2(rigid.velocity.x, jumpPower * 2);
            }
        }

        //��ġ�Ƿ� �ϴ� ����
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
        //�׻� tag=player�� ����� ��
        //�����̵��� �����ÿ� �ױ� �ٲ��


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

        //����׿�
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

        //TODO: �ʿ��ϸ� �������ͽ��� �´� ȿ���ۼ�
    }

    //===ZŰ(�׼�Ű)==
    void InputActionButton()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //ű �ȵǵ���
            isKick = false;
            isLift = true;
            //������ ���
            if (isLift && isCrushShell)
            {
                if (GameObject.Find("Mario").GetComponent<MarioCollision>().shell != null)
                {
                    tuttleShell = GetComponentInChildren<MarioCollision>().shell;
                    //�������
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
            //�׼�Ű ������ �ִ�ӵ� addAnimSpeed ��ŭ �߰�
            //������ �ƴ� ��
            if (!onAir)
                addedMaxAnimSpeed = maxAnimSpeed + addAnimSpeed;
            //�̵� ��
            if (curAnimSpeed > maxAnimSpeed)
            {
                animator.SetBool("inputActionButton", true);
                //TODO:�ѹ������� �ѹ�����ϴ� ���·�
                runSound.Play();
            }

            //Debug.Log("Input 'Z'button");
            if(!isCrushShell)
            { switch (marioStatus)
                {
                    //���¿� ���� 
                    case MarioStatus.NormalMario:
                        break;
                    //���۸�����
                    case MarioStatus.SuperMario:
                        break;
                    //�Ҳɸ�����
                    case MarioStatus.FireMario:
                        //TODO:�ٽ� �Լ��� �����
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
                    //�ʱ���������
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
        else if(isCrushShell)//���󺹱�
        {
            //zŰ ������ ����Ʈ ���ɻ���
            addedLimitVelocity = LimitVelocity;
            addedMaxAnimSpeed = maxAnimSpeed;
            animator.SetBool("inputActionButton", false);
            animator.SetBool("isTailAttack", false);
            isTailAttackSound = false;
            runSound.Pause();
            isKick = true;
            //
            Debug.Log("���߻�");
            if (isLift)
            {
                //���̵���Ű��
                if(tuttleShell)
                    tuttleShell.GetComponent<Tuttle>().currentState = Enemy.State.ShellMove;
                if (tuttleShell != null)
                {
                    tuttleShell = null;
                    //ű ����
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
            //zŰ ������ ����Ʈ ���ɻ���
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
        //���� ��ġ ����
        //���� ���
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
    //������ ���Ż��� get set
    public void setMarioStatus(MarioStatus marioForm)
    {
        marioStatus = marioForm;
    }
    public MarioStatus getMarioStatus() { return marioStatus; }

    //��Ʈ�� ����
    IEnumerator HitInvincible()
    {
        //������& ����
        marioBlink();
        yield return new WaitForSeconds(1f);
        isInvincible = false;
        Debug.Log("������");
    }

    //������ ���� �ʱ�ȭ
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
    //���� ��Ʈ�ڽ��Ѱ� ����
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
        //��Ʈ�ڽ�����
        hitBox.enabled = false;
        //�������� ���� ������
        sprite.sortingOrder = 99;
        StartCoroutine(StartDeathAnim());

    }

    IEnumerator StartDeathAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // �ö� ��
        float h = 0;
        float addH = 0;
        float targetHeightUp = 5f;  // ��ǥ ����
        float moveSpeedUp = 10f;     // ������ �ӵ�

        while (addH < targetHeightUp)
        {
            h = moveSpeedUp * Time.unscaledDeltaTime;  // ������ �ӵ��� ���� ������
            transform.position = new Vector2(transform.position.x, transform.position.y + h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        // ������ ��
        h = 0;
        addH = 0;
        float targetHeightDown = 20f;  // ��ǥ �ϰ� �Ÿ�
        float moveSpeedDown = 15f;      // ������ �ӵ� (�ϰ� ��)

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // ������ �ӵ��� ���� �ϰ�
            transform.position = new Vector2(transform.position.x, transform.position.y - h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    //�������� �׼�
    //"Down" "Up" "Left" "Right"
    public void PipeAction(String dir)
    {
        if (!isPipe)
        {
            //Time.timeScale = 0;
            //��Ʈ�ڽ�����
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

        // ������ ��
        float h = 0;
        float addH = 0;
        float targetHeightDown = 2.5f;  // ��ǥ �ϰ� �Ÿ�
        float moveSpeedDown = 2f;      // ������ �ӵ� (�ϰ� ��)

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // ������ �ӵ��� ���� �ϰ�
            transform.position = new Vector2(transform.position.x, transform.position.y - h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator StartUpPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // ������ ��
        float h = 0;
        float addH = 0;
        float targetHeightDown = 2.5f;  // ��ǥ �ϰ� �Ÿ�
        float moveSpeedDown = 2f;      // ������ �ӵ� (�ϰ� ��)

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // ������ �ӵ��� ���� �ϰ�
            transform.position = new Vector2(transform.position.x, transform.position.y + h);
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator StartRightPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // ������ ��
        float h = 0;
        float addH = 0;
        float targetHeightDown = 0.5f;  // ��ǥ �ϰ� �Ÿ�
        float moveSpeedDown = 1f;      // ������ �ӵ� (�ϰ� ��)

        isRight = true;
        curAnimSpeed = addedMaxAnimSpeed;

        //Anim :run
        animator.SetBool("isRun", true);
        moveTimer += Time.unscaledDeltaTime;

        curAnimSpeed = moveTimer * animAccel;
        //�ְ�ӵ� ����
        if (curAnimSpeed > addedMaxAnimSpeed)
            curAnimSpeed = addedMaxAnimSpeed;

        animator.SetFloat("Speed", curAnimSpeed);

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // ������ �ӵ��� ���� �ϰ�
            transform.position = new Vector2(transform.position.x + h, transform.position.y );
            addH += h;
            yield return new WaitForSecondsRealtime(0.01f);
        }

    }

    IEnumerator StartLeftPipeAnim()
    {
        yield return new WaitForSecondsRealtime(1f);

        // ������ ��
        float h = 0;
        float addH = 0;
        float targetHeightDown = 2.5f;  // ��ǥ �ϰ� �Ÿ�
        float moveSpeedDown = 2f;      // ������ �ӵ� (�ϰ� ��)

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
        //�ְ�ӵ� ����
        if (curAnimSpeed > addedMaxAnimSpeed)
            curAnimSpeed = addedMaxAnimSpeed;

        animator.SetFloat("Speed", curAnimSpeed);

        while (addH < targetHeightDown)
        {
            h = moveSpeedDown * Time.unscaledDeltaTime;  // ������ �ӵ��� ���� �ϰ�
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
    //P ������ �Ǵ�
    void TurnOnP()
    {
        //p���� �ְ�ӵ����� �����ϰ�
        //TODO:�浹 �� P������ 
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

    //�ʱ��� ������ Ȱ��
    void RMarioSpecialActtion()
    {
        //�ʱ����������� �� �� ��밡��
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
    //�Ҳ� ������
    void ShootFire()
    {
        //�� �ΰ��� �߻����̸�
        MarioFire[] fires = FindObjectsOfType<MarioFire>();
        if(fires.Length > 1)
        { return; }
        
        Vector2 direction;
        // �߻�ü ���� �� �÷��̾ ���� �߻�
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

    //Ŭ����
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
            //�ְ�ӵ� ����
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
            //�������� Phsics������
            if (Time.timeScale == 0)
            {
                ApplyCustomGravity();
            }
        }
    }

    //timeScale=0�� �� ���
    void ApplyCustomGravity()
    {
        // �߷� ���͸� Rigidbody2D�� �ӵ��� ����
        clearVelocity += Vector2.down * 9.8f * Time.unscaledDeltaTime;
        Vector2 newPosition = rigid.position + clearVelocity * Time.unscaledDeltaTime;
        rigid.transform.position = newPosition;
    }

    //���̵� ��Ȱ��ȭ
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

    //����ȿ��
    public void StarInvisibleEffect()
    {
        if (invisibleTimeCount1 > 7)
        {
            Debug.Log("������");
            StopCoroutine("Blink");
            GetComponent<SpriteRenderer>().material.color = originalColor;
            invisibleTimeCount1 = 0;
            isInvincibleStar = false;
            //�±׿��󺹱�
            gameObject.tag = "Player";
            //�ִϸ����� ����
            animator.SetBool("isInvincibleStar", false);
            notInput = false;
            invisibleCount = 0;
        }
        else
        {
            invisibleTimeCount1 += Time.deltaTime;
            //�±׺�ȭ
            gameObject.tag = "StarInvincible";
            //�ִϸ����� �ѱ�
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
    //���̾� ������ ����Ʈ
    //TODO
    public void ChangeFireMario()
    {
        if (invisibleTimeCount2 > 7)
        {
            StopCoroutine("Blink");
            StopCoroutine("ChangeFireMarioEffect");  // �ڷ�ƾ ����
            GetComponent<SpriteRenderer>().material.color = originalColor; // ���� ������ ����
            invisibleTimeCount2 = 0;
            effectOn = false;
            invisibleCount = 0;
        }
        else
        {
            invisibleTimeCount2 += Time.unscaledDeltaTime;

            // �ڷ�ƾ�� �� ���� ����ǵ��� ����
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
        while (true)  // �ڷ�ƾ�� �ݺ������� ����ǵ��� ����
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
    //������ ���� �ִϸ��̼� ������ �Լ�
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

    // �ڽ�ĳ��Ʈ ����׿�
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

