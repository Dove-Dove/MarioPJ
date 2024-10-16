using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    //������Ʈ
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer sprite;
    public PhysicsMaterial2D physicsMaterial;
    public GameObject tail;

    //������Ʈ ���������
    private GameObject tuttleShell;

    public float LMrio_Jump_pow = 9f;
    public float SMrio_Jump_pow = 10f;
    public float mario_AddedJumpPowLevel = 2.5f;
    //==������ Ȯ�ο�
    public bool isSuperMario;
    public bool isLittleMario;
    [SerializeField]
    private MarioStatus marioStatus = MarioStatus.NormalMario;
    private MarioStatus curStatus;
    //������ HP
    public uint hp;
    //������ ���� Ȯ�ο�
    public bool isFireMario = false;
    public bool isRaccoonMario = false;
    //��������(��Ʈ ��)
    public bool isInvincible = false;
    //�� ����
    public bool isInvincibleStar = false;
    //��Ʈ
    public bool ishit = false;
    //�ԷºҰ� ����
    [SerializeField]
    private bool notInput = false;
    public bool NotInput
    {
    get { return notInput; } 
    set { notInput = value; }
    }
    [SerializeField]
    private bool isClear= false;
    //������ ����
    public bool isRight = false;
    //���߿� ���� ��
    public bool inAir;

    //==�̵�
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
    public float Velocity = 20;
    public float LimitVelocity = 4;
    public float addedLimitVelocity;
    public float addLimitVelocity = 3;
    //==�ִϸ��̼�
    public UnityEngine.KeyCode curKey=KeyCode.None;

    //�¿�
    public float input_x;
    //����
    public bool isJump=false;
    public bool onAir=false;
    private bool onceInputJumpBoutton=false;
    private bool noDoubleJump = false;
    public float jumpInputTime = 0.5f;
    private float jumpPower;
    //�̲�������
    public bool isSilding = false;
    public float slideAddForcd=5f;
    public const float friction = 1;
    public const float hillFriction = 0.1f;
    
    //�ɱ�
    public bool isSit = false;
    [SerializeField]
    private float groundRayLen=0;
    [SerializeField]
    private float hillRayLen=0;
    public float LMarioGroundRayLen = 0.6f;
    public float LMarioHillRayLen = 1.3f;
    public float SMarioGroundRayLen = 1.1f;
    public float SMarioHillRayLen = 1.8f;
    //����
    public bool isEnemy=false;
    public bool isAttack = false;
    //���(Z)
    public bool isLift;
    //���� ���޿�
    public bool isKick=true;

    //����

    //�߰��߰� ��ü �ִϸ��̼� ���������ϴ� ����
    [SerializeField]
    private bool stopMoment;
    //���ӵ� ������
    [SerializeField]
    private float acceleGauge = 0;    
    [SerializeField]
    private float maxAcceleGauge = 20;
    [SerializeField]
    private int marioHp;
    [SerializeField]
    private bool isLookRight;

    private Vector2 marioPos;

    //===����
    public AudioSource jumpSound;
    public AudioSource turnSound;
    public AudioSource powerUpSound;
    private bool isPowerUp=false;
    public AudioSource hitSound;
    private bool ishitSound = false;
    public AudioSource deadSound;
    private bool isDeadSound=false;
    public AudioSource runSound;
    public AudioSource kickSound;
    private bool iskcikSound = false;
    public AudioSource tailAttackSound;
    private bool isTailAttackSound=false;
    //===����Ʈ
    private Color originalColor;

    //��Ÿ
    public bool timeStop=false;
    private Vector2 LMarioHitboxSize = new Vector2(0.9f, 0.9f);
    private Vector2 SMarioHitboxSize = new Vector2(0.9f, 1.7f);

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
        //TODO:���� ���̷� �ٴ�Ȯ��
        onGround = true;

        //rigid.gravityScale = 2;

        animAccel = 8;
        jumpPower=LMrio_Jump_pow;

    }

    // Start is called before the first frame update
    void Start()
    {
        //���� �� ����������
        FlipPlayer(true);
        //���� �� hp����
        marioStatus= MarioStatus.NormalMario;
        UpdateMarioStatusAndHP(MarioStatus.NormalMario);
        //������ ���º�ȭ ������
        curStatus = marioStatus;
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
        originalColor = sprite.material.color;
        marioPos =rigid.position;
        //�÷��̾� �̵��Ұ� ����
        if(ishit && !isInvincible)
        {
            //deadȮ��
            if(marioHp <= 1)
            { 
                //������ ���ó��
                if(!isDeadSound)
                {
                    MarioDeath();
                }
                //����ȯ Ȥ�� �����ȣ
                animator.SetBool("isDead", true);
            }
            else
            { 
                marioHp--; 
                ishit = false;
                isInvincible = true;
                //1�� ����
                StartCoroutine(HitInvincible());
                if (marioHp==1)
                {
                    //�⺻ ������
                    marioStatus=MarioStatus.NormalMario;
                    setChangeStatus();
                    
                }
                else if (marioHp==2)
                {
                    //Debug.Log("Set SuperMario");
                    marioStatus = MarioStatus.SuperMario;
                    setChangeStatus();
                }
            }


            if(!ishitSound)
            {
                ishitSound = true;
                hitSound.Play();
            }
            //TODO:�÷������ΰ� ������ �ϴ� ����
            //marioBlink();
            
            return;
        }
        //�ԷºҰ� ��Ȳ
        else if(notInput)
        {
            //Debug.Log("notInput");
            ChangeSuperMario();
            setChangeStatus();
            return;
        }
        //Ŭ����
        else if(isClear)
        {
            //������� �̵� �ִϸ��̼� ��� + �̵�
            return;
        }
        //�⺻ �Է°��ɻ���
        else
        {
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

           //==����
           if (Input.GetKey(KeyCode.X) || isAttack)
            {
                Jump();
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
            if (curStatus !=marioStatus)
            {
                curStatus=marioStatus;
                setChangeStatus();
            }
        }
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
        jumpTimer += Time.deltaTime;
        //��ư�Է�Ȯ�ο�.
        onceInputJumpBoutton = true;
        //���� �Է� �ð� ����
        if (jumpTimer > jumpInputTime) 
            onceInputJumpBoutton = false;
            
        //���� �ѹ��� ������
        if (Input.GetKeyDown(KeyCode.X) && !onAir)
            { 
            jumpSound.Play();
            animator.SetBool("isJump", true);
            }

        jumpPower = LMrio_Jump_pow;
        //Debug.Log(jumpPower);
        //addforce
        if (onceInputJumpBoutton &&!noDoubleJump)
        {
            //Debug.Log("Jump");
            var direction = new Vector2(0, jumpPower);
            rigid.AddForce(direction, ForceMode2D.Impulse);
            //�� ����
            if (rigid.velocity.y > jumpPower)
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
    }

    void CheckOnGround()
    {
        //����׿�
        Debug.DrawRay(rigid.position, new Vector2(0,-groundRayLen), new Color(1,0,0));
        Debug.DrawRay(rigid.position, new Vector2(0,-hillRayLen), new Color(0,1,0));

        RaycastHit2D groundHit = Physics2D.Raycast(rigid.position, Vector2.down, groundRayLen,LayerMask.GetMask("Ground"));
        if (groundHit.collider != null)
        {
            //Debug.Log("onGround");
            onAir = false;
            onGround = true;//������ȯȿ�� �¿�����
            noDoubleJump = false;//�����Է°���
            animator.SetBool("isJump", false);

            //�ɱ�
            if (Input.GetKey(KeyCode.DownArrow) && input_x == 0)
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
                    case MarioStatus.InvincibleMario:
                        break;
                }
            }
            else
            {
                animator.SetBool("isSit", false);
                //��Ʈ�ڽ� ������ ����
                if (marioStatus == MarioStatus.NormalMario)
                    { hitBox.size = LMarioHitboxSize; hitBox.offset = new Vector2(0, 0.5f); }
                else
                    { hitBox.size = SMarioHitboxSize; hitBox.offset =new Vector2(0,0.9f); }
            }
        }
        else
        {
            onGround = false;
            onAir = true;
            animator.SetBool("isJump", true);
        }

            //������� ���� �� 
            RaycastHit2D onDownhill = Physics2D.Raycast(rigid.position, Vector2.down, hillRayLen, LayerMask.GetMask("DownHill"));
        if (onDownhill.collider != null)
        {
            //Debug.Log(onDownhill.collider.name);
            onGround = true;
            onAir = false;
            animator.SetBool("isJump", false);

            //�̲�������
            if (Input.GetKey(KeyCode.DownArrow) && !isSilding)
            {
                animator.SetBool("isSlide", true);
                isSilding = true;
                gameObject.tag = "PlayerAttack";
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

            //��������� �̵��Է¾����� ����
            //TODO:�����̵� �۵��ǵ���
            if (input_x == 0 &&!isSilding &&!onAir)
            {
                //�̲�������
                gameObject.tag = "PlayerAttack";
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    animator.SetBool("isSlide", true);
                    isSilding = true;
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
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; gameObject.tag = "Player"; }

        }

        RaycastHit2D marioAttackHit = Physics2D.Raycast(rigid.position, Vector2.down, groundRayLen, LayerMask.GetMask("Enemy"));
        if(marioAttackHit.collider != null)
        {
            isEnemy = true;
            gameObject.tag = "PlayerAttack";
        }
        else { isEnemy = false; gameObject.tag = "Player"; isAttack = false; }
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
            case MarioStatus.InvincibleMario:
                isInvincibleStar=true; break;
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

            addedLimitVelocity = LimitVelocity + addLimitVelocity;
            //�׼�Ű ������ �ִ�ӵ� addAnimSpeed ��ŭ �߰�
            addedMaxAnimSpeed = maxAnimSpeed + addAnimSpeed;
            //�̵� ��
            if (curAnimSpeed > maxAnimSpeed)
            {
                animator.SetBool("inputActionButton", true);
                //TODO:�ѹ������� �ѹ�����ϴ� ���·�
                runSound.Play();
            }
            //������ ���
            if (isLift)
            {
                if (GameObject.Find("Mario").GetComponent<MarioCollision>().shell != null)
                {
                    tuttleShell = GameObject.Find("Mario").GetComponent<MarioCollision>().shell;
                    if (isRight)
                        tuttleShell.transform.position = marioPos + new Vector2(0.8f, 0.5f);
                    else
                        tuttleShell.transform.position = marioPos + new Vector2(-0.8f, 0.5f);
                    animator.SetBool("isLift", true);
                }
            }
            else
            {
                tuttleShell = null;
            }

            //Debug.Log("Input 'Z'button");
            switch (marioStatus)
            {
                //���¿� ���� �ִ�ӵ�����
                case MarioStatus.NormalMario:
                    break;
                //���۸�����
                case MarioStatus.SuperMario:
                    break;
                //�Ҳɸ�����
                case MarioStatus.FireMario:
                    break;
                //�ʱ���������
                case MarioStatus.RaccoonMario:
                    animator.SetBool("isTailAttack",true);
                    if(Input.GetKeyDown(KeyCode.Z))
                        animator.Play("RMario_tailattack");
                    if (!isTailAttackSound)
                    {
                        isTailAttackSound = true;
                        tailAttackSound.Play();
                    }
                    break;
            }
        }
        else if(Input.GetKeyUp(KeyCode.Z))//���󺹱�
        {
            addedLimitVelocity = LimitVelocity;
            addedMaxAnimSpeed = maxAnimSpeed;
            animator.SetBool("inputActionButton", false);
            animator.SetBool("isTailAttack", false);
            isTailAttackSound = false;
            runSound.Pause();
            if(isLift)
            {
                if (tuttleShell != null)
                { 
                    tuttleShell = null;
                    //ű ����
                    kickSound.Play();
                }
                isKick = true;
                isLift = false;
                animator.SetBool("isLift", false);

            }
        }
    }

    void ChangeSuperMario()
    {
        if (isSuperMario)
        {
            //���� ��ġ ����
            //Debug.Log("SuperMario!!");
            //���� ���
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
        //�ִϸ��̼� ����: ���۸�����
        
        animator.SetBool("ChangeSuperMario",true);
        //�̳Ѻ��� ���� : ���۸�����
        //Debug.Log("EndChange MarioForm");
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
        yield return new WaitForSecondsRealtime(1f);
        isInvincible = false;
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
                animator.Play("RMario_idle");
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
                UpdateMarioStatusAndHP(marioStatus);
                SetSMario();
                break;
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
        hitBox.enabled = false;
        StartCoroutine(StartDeathAnim());

    }
    IEnumerator StartDeathAnim()
    {

        yield return new WaitForSecondsRealtime(1f);
        Debug.Log("Death Anim Play");
        //�ö� ��
        float h = 0;
        float addH = 0;
        while(addH < 5)
        {
            h = Time.unscaledTime * 0.02f;
            transform.position = new Vector2(transform.position.x, transform.position.y + h );
            addH += h ;
            Debug.Log(addH);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        h = 0;
        addH = 0;
        //������ ��
        while(addH < 20)
        {
            h = Time.unscaledTime * 0.03f;
            transform.position = new Vector2(transform.position.x, transform.position.y - h );
            addH += h ;
            Debug.Log(addH);
            yield return new WaitForSecondsRealtime(0.01f);
        }

    }

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

