using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class brickBlocks : MonoBehaviour
{
    //임시 변수
    /*
        완성하지 않았음 - 게임메니저가 어느정도 되면 다시 작업 해야 함 
    */
    public bool small = false;
    public bool big = true;
    //-----
    public GameObject coin;
    //-----
    //사운드
    public AudioSource hitBlockSound;
    public AudioSource BreakBlockSound;
    private int soundPlay;
    //작은 마리오가 벽돌 칠때 
    private Vector2 nowPos;
    private Vector2 movePos;
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

        //작은 마리오 상태일때 잠깐 위로 이동
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
       
       //큰 마리오 일떄
       if((usingBlock !=null ) && getCoin)
        {
            coin.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = usingBlock;
            //자기자신을 비 활성화 해서 중복으로 발동을 방지
            gameObject.GetComponent<brickBlocks>().enabled = false;
        }

       else if(big)
        {
            BreakBlockSound.Play(); 
            gameObject.SetActive(false);
        }
    }
}
