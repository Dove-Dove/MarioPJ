using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_Move : MonoBehaviour
{
    //������Ʈ
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;

    public float LMrio_LowJump_pow = 8f;
    public float LMrio_TopJump_pow = 12.5f;
    public float BMrio_LowJump_pow = 10f;
    public float BMrio_TopJump_pow = 17.5f;
    public float mario_AddedJumpPowLevel = 2.5f;
    //==������ Ȯ�ο�
    public bool isBigMario;
    public bool isLittleMario;
    //������ ���� Ȯ�ο�
    public bool isFireMario = false;
    public bool isRaccoonMario = false;
    //�� ����
    public bool isInvincibleStar = false;
    //��Ʈ
    public bool ishit = false;
    //������ ����
    public bool isRight = false;
    //���߿� ���� ��
    public bool inAir;

    //==�̵�
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
    //==�ִϸ��̼�
    public UnityEngine.KeyCode curKey=KeyCode.None;


    //�¿�
    public float input_x;
    //����
    public bool isJump=false;
    public bool onAir=false;
    private bool onceInputJumpBoutton=false;
    public float jumpInputTime = 0.5f;
    //�߰��߰� ��ü �ִϸ��̼� ���������ϴ� ����
    [SerializeField]
    private bool stopMoment;
    //���ӵ� ������
    [SerializeField]
    private float acceleGauge = 20;
    [SerializeField]
    private int marioHp;
    [SerializeField]
    private bool isLookRight;

    private Vector2 marioPos;

    //===����
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

        //ȸ�� ����
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        isBigMario = false;
        isLittleMario = true;
        //TODO:���� ���̷� �ٴ�Ȯ��
        onGround = true;

        //rigid.gravityScale = 2;

        animAccel = 8;

    }

    // Start is called before the first frame update
    void Start()
    {
        //���� �� ����������
        FlipPlayer(true);
    }

    // Update is called once per frame
    void Update()
    {

        marioPos =rigid.position;
        //�÷��̾� �̵��Ұ� ����
        if(ishit)
        {
            return;
        }
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
                    curAnimSpeed -= curAnimSpeed * 0.1f;
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
                    curAnimSpeed -= curAnimSpeed * 0.1f;
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
            //�ְ�ӵ� ����
            if (curAnimSpeed > maxAnimSpeed)
                curAnimSpeed = maxAnimSpeed;

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
        if (Input.GetKeyDown(KeyCode.X))
            { 
            jumpSound.Play();
            animator.SetBool("IsJump", true);
            //+ ü���ð������� �����ڼ�����
        }

        float jumpPower = LMrio_LowJump_pow;
        Debug.Log(jumpPower);
        //addforce
        if (onceInputJumpBoutton)
        {        
            Debug.Log("Jump");
            var direction = new Vector2(0, jumpPower);
            rigid.AddForce(direction,ForceMode2D.Impulse);
            //�� ����
            if (rigid.velocity.y > jumpPower)
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }
    }

    void CheckOnGround()
    {
        //����׿�
        Debug.DrawRay(rigid.position, new Vector2(0,-0.6f), new Color(1,0,0));

        RaycastHit2D groundHit = Physics2D.Raycast(rigid.position, Vector2.down, 0.6f,LayerMask.GetMask("Ground"));
        if(groundHit.collider !=null)
        {
            Debug.Log("onGround");
            onAir = false;
            onGround = true;//������ȯȿ�� �¿�����
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

            //��������� �̵��Է¾����� ����
            if (input_x == 0)
            { rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; }
            else
            { rigid.constraints = RigidbodyConstraints2D.FreezeRotation; }

            

        }
    }


}
