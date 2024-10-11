using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCoin : MonoBehaviour
{
    // 박스 코인(일반 코인이랑 다르게 되어있어서 따로 만들었음)
    
    //처음 위치
    Vector2 nowpos;
    Vector2 moveEndPos;
    bool up = false;
    int point = 100;


    //사운드
    public AudioSource getCoinSound;
    void Start()
    {
        nowpos = transform.position;
        moveEndPos = nowpos + new Vector2(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //사운드는 유니티안에 play on Awake 로 실행
        if(!up)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveEndPos, 12.0f * Time.deltaTime);
            if(transform.position.y >= moveEndPos.y)
                up = true;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, nowpos, 12.0f * Time.deltaTime);
            if(transform.position.y <= nowpos.y)
            {
                pointget();
                gameObject.SetActive(false);
            }
        }
            
           

    }

    void pointget()
    {
        //게임 메니저에게 값을 전달 
        GameObject.Find("GameManager").GetComponent<GameManager>().GetCoin();
        GameObject.Find("GameManager").GetComponent<GameManager>().GetPoint(100);
    }


}
