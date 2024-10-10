using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class brickBlocks : MonoBehaviour
{
    //�ӽ� ����
    /*
        �ϼ����� �ʾ��� - ���Ӹ޴����� ������� �Ǹ� �ٽ� �۾� �ؾ� �� 
    */
    public bool small = false;
    public bool big = true;
    //-----
    public GameObject coin;
    //-----
    //����
    public AudioSource hitBlockSound;
    public AudioSource BreakBlockSound;
    private int soundPlay;
    //���� �������� ���� ĥ�� 
    private Vector2 nowPos;
    private Vector2 movePos;
    float moveTime = 0.0f;

    //ū ������ + �ȿ� ������ ������
    public bool getCoin = false;

    //����� �̹���
    public Sprite usingBlock;

    void Start()
    {
        nowPos = new Vector2(transform.position.x, transform.position.y);
        movePos = nowPos+ new Vector2(0,0.5f);
        soundPlay = 0;
        coin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //���� ������ �����϶� ��� ���� �̵�
       if(small && moveTime <= 0.5f)
        {
            if (soundPlay == 0)
            {
                hitBlockSound.Play();
                soundPlay++;
            }
            transform.position = Vector2.MoveTowards(transform.position, movePos, 10.0f * 0.021f);
            moveTime += Time.deltaTime;

        }
       else if ( transform.position.y >= nowPos.y )
        {
            moveTime = 0.0f;
            transform.position = Vector2.MoveTowards(transform.position, nowPos, 10.0f * 0.021f);
            small = false;
            soundPlay = 0;
        }
       
       //ū ������ �ϋ�
       if((usingBlock !=null ) && getCoin)
        {
            coin.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = usingBlock;
            //�ڱ��ڽ��� �� Ȱ��ȭ �ؼ� �ߺ����� �ߵ��� ����
            gameObject.GetComponent<brickBlocks>().enabled = false;
        }

       else if(big)
        {
            BreakBlockSound.Play(); 
            gameObject.SetActive(false);
        }
    }
}
