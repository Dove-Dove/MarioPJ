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
    public Camera mainCam;

    public float MaxSpeed=10.0f;

    public float LMrio_Jump_pow = 10;
    public float BMrio_Jump_pow = 10;

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
    public float maxSpeed = 10;
    public float curAnimSpeed;
    private bool isInputMove = false;
    private bool beginMove = false;
    private float timer = 0;
    public float animAccel = 0;
    //==�ִϸ��̼�
    public UnityEngine.KeyCode curKey=KeyCode.None;


    //�¿�
    public float input_x;
    //����

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
        //���� �� ����������
        FlipPlayer(true);
    }

    // Update is called once per frame
    void Update()
    {
        marioPos=rigid.position;
        //�÷��̾� �̵��Ұ� ����
        if(ishit)
        {
            return;
        }
        else
        {
            //������ȯ �ִϸ��̼ǿ�
            //if(curKey== KeyCode.RightArrow && Input.GetKey(KeyCode.LeftArrow))
            //    animator.SetBool("ChangeDirection", true);
            //if (curKey == KeyCode.LeftArrow && Input.GetKey(KeyCode.RightArrow))
            //    animator.SetBool("ChangeDirection", true);

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
                    curAnimSpeed -= curAnimSpeed * 0.01f;
                    if (curAnimSpeed <= 0.1f)
                    {
                        curAnimSpeed = 0;
                        animator.SetBool("isRun", false);
                    }
                    animator.SetFloat("Speed", curAnimSpeed);
                }

                //Ÿ�̸� �ʱ�ȭ
                timer = 0;
                Debug.Log("Right key up");
            }
            //��->�� �϶� ������ȯ �ִϸ��̼�
            if (curKey== KeyCode.RightArrow && Input.GetKey(KeyCode.LeftArrow))
                animator.SetBool("ChangeDirection", true);

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
                    curAnimSpeed -= curAnimSpeed * 0.01f;
                    if (curAnimSpeed <= 0.1f)
                    {
                        curAnimSpeed = 0;
                        animator.SetBool("isRun", false);
                    }
                    animator.SetFloat("Speed", curAnimSpeed);
                }

                //Ÿ�̸� �ʱ�ȭ
                timer = 0;
                Debug.Log("Right key up");
            }
            //��->�� �϶� ������ȯ �ִϸ��̼�
           if (curKey == KeyCode.LeftArrow && Input.GetKey(KeyCode.RightArrow))
                animator.SetBool("ChangeDirection", true);

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
    //������ �ð��� ���� ��Ǽӵ��� ���� �Լ�
    //�����ʿ� ����addForce�� ������ �ٽ�

    //Ű �Է½� ��ġ �����ϰ� �� ��ġ������ �Ÿ��� ���� �ӵ�������� ������ ǥ��
    void Destination()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            //Anim :run
            animator.SetBool("isRun", true);
            timer += Time.deltaTime;

            curAnimSpeed = timer * animAccel;
            //Debug.Log(curSpeed);
            //�ְ�ӵ� ����
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
            //�ְ�ӵ� ����
            if (curAnimSpeed > maxSpeed)
                curAnimSpeed = maxSpeed;

            animator.SetFloat("Speed", curAnimSpeed);
        }
    }
}
