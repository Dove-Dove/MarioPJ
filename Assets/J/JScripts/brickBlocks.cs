using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class brickBlocks : MonoBehaviour
{
    //opne , notopen ���� ����
    // ���� �������϶� ��ȣ�ۿ��� �־ �����ϱ� ���ؼ� �������
    private bool open = false;
    private bool notOpen = false;

    //----- ���� ������Ʈ 
    public GameObject coin;
    public GameObject WallObject;
    public GameObject WallUnderObject;
    public GameObject BreakAnim;

    //����
    public AudioSource hitBlockSound;
    public AudioSource BreakBlockSound;
    private int soundPlay;

    //���� �������� ���� ĥ�� 
    private Vector2 nowPos;
    private Vector2 movePos;
    public bool shakeBlocks = false;

    float moveTime = 0.0f;
    float animTime = 0.0f;

    //ū ������ + �ȿ� ������ ������
    public bool getCoin = false;

    //10�ʵ��� ĥ�� �ִ� ��
    public bool TimeBlock = false;

    //����� �̹���
    public Sprite usingBlock;

    


    void Start()
    {
        nowPos = new Vector2(transform.position.x, transform.position.y);
        movePos = nowPos+ new Vector2(0,0.5f);
        soundPlay = 0;
        coin.SetActive(false);
        BreakAnim.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (WallUnderObject.GetComponent<underCol>().WallUnderOpen)
        {     
            if(WallUnderObject.GetComponent<underCol>().big)
                open = true;
            else
                notOpen = true;
        }

        else if(WallObject.GetComponent<BlockWall>().WallOpen)
            open = true;

        //���� ������ �����϶� ��� ���� �̵�
       if (notOpen && moveTime <= 0.5f)
        {
            if (soundPlay == 0)
            {
                hitBlockSound.Play();
                soundPlay++;
            }
            transform.position = Vector2.MoveTowards(transform.position, movePos, Time.deltaTime * 4);
            shakeBlocks = true;
            moveTime += Time.deltaTime;

        }
       else if (transform.position.y >= nowPos.y && !open) 
        {
            moveTime = 0.0f;
            transform.position = Vector2.MoveTowards(transform.position, nowPos, Time.deltaTime * 4);
            shakeBlocks = false;
            notOpen = false;
            soundPlay = 0;
        }
       
       //ū ������ �ϋ�
       if((usingBlock !=null ) && getCoin && open)
        {
            coin.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = usingBlock;
            //�ڱ��ڽ��� �� Ȱ��ȭ �ؼ� �ߺ����� �ߵ��� ����
            gameObject.GetComponent<brickBlocks>().enabled = false;
        }

       else if(open)
        {
            animTime += Time.deltaTime;
            shakeBlocks = true;
            if (soundPlay == 0 )
            {
                BreakBlockSound.Play();
                soundPlay++;
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            BreakAnim.SetActive(true);

            if (animTime >= 0.5f)
                gameObject.SetActive(false);
        }
    }

}
