using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public enum Itemtypy
{
    mushroom,
    flower,
    star,
    leaf
}

public class items : MonoBehaviour
{
    public Sprite[] itemImg;
    public Itemtypy itemtypys;


    //�������� �ö���� ��ġ 
    public Vector2 target;
    //������ �̵��ӵ�(����, ��)
    public float movespeed =2 ;
    // �¿� ������ �Լ�
    public bool randomWay = false;

    bool openItem = true;



    void Start()
    {
        if(itemtypys != Itemtypy.leaf)
            target = new Vector2(transform.position.x, transform.position.y+1);
        else
            target = new Vector2(transform.position.x, transform.position.y + 3);
        randomWay = (Random.value > 0.5f);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        upItem();
        switch (itemtypys)
        {
            case Itemtypy.mushroom:
                mushroom();
                break;
            case Itemtypy.flower:
                flower();
                break;
            case Itemtypy.star:
                star();
                break;
            case Itemtypy.leaf:
                leaf();
                break;
        }
    }

    void mushroom()
    {
        GetComponent<SpriteRenderer>().sprite = itemImg[0];
        MoveItems();

    }

    void flower()
    {
        GetComponent<SpriteRenderer>().sprite = itemImg[1];
        
    }

    void star()
    {
        GetComponent<SpriteRenderer>().sprite = itemImg[2];
    }

    void leaf()
    {
        GetComponent<SpriteRenderer>().sprite = itemImg[3];
        MoveItems();
    }

    void upItem()
    {
     float speed = 1;

     //���� ������ ���� �ö� (���� ��ġ���� y ��ǥ +1 ��ŭ �ö� )
        if(openItem && (transform.position.y <= target.y-0.1))
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * 0.021f);
        }
        else
        {
            openItem = false;
            GetComponent<BoxCollider2D>().enabled = true;
            if(itemtypys != Itemtypy.leaf)
                GetComponent<Rigidbody2D>().gravityScale = 1;
            else
                GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        }

            
     //�����ۿ� ���� �������� �޶���
    }


    void MoveItems()
    {
        //&& itemtypys == Itemtypy.mushroom
        if (!openItem )
        {
            if (randomWay)
                transform.Translate(Vector2.left * movespeed * Time.deltaTime);
            else if(!randomWay)
                transform.Translate(Vector2.right * movespeed * Time.deltaTime);
        }
               
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyWall" && itemtypys != Itemtypy.leaf)
        {
            randomWay = !randomWay;
        }
    }
}


