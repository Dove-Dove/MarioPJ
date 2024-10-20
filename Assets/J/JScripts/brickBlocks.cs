using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class brickBlocks : MonoBehaviour
{
    //opne , notopen 구분 이유
    // 작은 마리오일때 상호작용이 있어서 구분하기 위해서 만들었음
    private bool open = false;
    private bool notOpen = false;

    //----- 게임 오브젝트 
    public GameObject coin;
    public GameObject WallObject;
    public GameObject WallUnderObject;

    //사운드
    public AudioSource hitBlockSound;
    public AudioSource BreakBlockSound;
    private int soundPlay;

    //작은 마리오가 벽돌 칠때 
    private Vector2 nowPos;
    private Vector2 movePos;
    public bool shakeBlocks = false;

    float moveTime = 0.0f;

    //큰 마리오 + 안에 코인이 있을때
    public bool getCoin = false;

    //사용후 이미지
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
        if (WallUnderObject.GetComponent<underCol>().WallUnderOpen)
        {     
            if(WallUnderObject.GetComponent<underCol>().big)
                open = true;
            else
                notOpen = true;
        }

        else if(WallObject.GetComponent<BlockWall>().WallOpen)
            open = true;

        //작은 마리오 상태일때 잠깐 위로 이동
       if (notOpen && moveTime <= 0.5f)
        {
            if (soundPlay == 0)
            {
                hitBlockSound.Play();
                soundPlay++;
            }
            transform.position = Vector2.MoveTowards(transform.position, movePos, Time.deltaTime * 3);
            shakeBlocks = true;
            moveTime += Time.deltaTime;

        }
       else if ( transform.position.y >= nowPos.y ) 
        {
            moveTime = 0.0f;
            transform.position = Vector2.MoveTowards(transform.position, nowPos, Time.deltaTime * 3);
            shakeBlocks = false;
            notOpen = false;
            soundPlay = 0;
        }
       
       //큰 마리오 일떄
       if((usingBlock !=null ) && getCoin && open)
        {
            coin.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = usingBlock;
            //자기자신을 비 활성화 해서 중복으로 발동을 방지
            gameObject.GetComponent<brickBlocks>().enabled = false;
        }

       else if(open)
        {
            shakeBlocks = true;
            BreakBlockSound.Play(); 
            gameObject.SetActive(false);
        }
    }

}
