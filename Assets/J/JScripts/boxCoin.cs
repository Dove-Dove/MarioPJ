using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCoin : MonoBehaviour
{
    // �ڽ� ����(�Ϲ� �����̶� �ٸ��� �Ǿ��־ ���� �������)
    
    //ó�� ��ġ
    Vector2 nowpos;
    Vector2 moveEndPos;
    bool up = false;
    int point = 100;


    //����
    public AudioSource getCoinSound;
    void Start()
    {
        nowpos = transform.position;
        moveEndPos = nowpos + new Vector2(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //����� ����Ƽ�ȿ� play on Awake �� ����
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
        //���� �޴������� ���� ���� 
        GameObject.Find("GameManager").GetComponent<GameManager>().GetCoin();
        GameObject.Find("GameManager").GetComponent<GameManager>().GetPoint(100);
    }


}
