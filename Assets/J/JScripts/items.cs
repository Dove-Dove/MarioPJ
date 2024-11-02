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
    public Rigidbody2D ridy;

    //�������� �ö���� ��ġ 
    public Vector2 target;
    //������ �̵��ӵ�(����, ��)
    public float movespeed =5 ;
    // �¿� ������ �Լ�
    public bool randomWay = false;
    float jumpForce = 5.0f;
    private int direction = 1;

    private float moveTime = 0 ;
    private bool isGround = false;

    bool openItem = true;
    private GameManager gameManager;
    public PhysicsMaterial2D mat;

    void Start()
    {
        gameManager = GameManager.Instance;
        if (itemtypys == Itemtypy.star)
            mat.bounciness = 0.5f;
        if (gameManager.Player_State == 1 && itemtypys != Itemtypy.star)
        {
            itemtypys = Itemtypy.mushroom;
        }
        if(itemtypys == Itemtypy.star)
            target = new Vector2(transform.position.x, transform.position.y + 2);
        else if (itemtypys == Itemtypy.leaf)
            target = new Vector2(transform.position.x, transform.position.y + 3);
        else
            target = new Vector2(transform.position.x, transform.position.y + 1);

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

        MoveStar();
    }

    void leaf()
    {
        GetComponent<SpriteRenderer>().sprite = itemImg[3];
        MoveItems();
    }

    void upItem()
    {

     //���� ������ ���� �ö� (���� ��ġ���� y ��ǥ +1 ��ŭ �ö� )
        if(openItem && (transform.position.y <= target.y-0.1))
        {
            if (itemtypys == Itemtypy.leaf)
                movespeed = 10.0f;

            transform.position = Vector2.MoveTowards(transform.position, target, movespeed * Time.deltaTime);
        }
        else
        {
            openItem = false;
            GetComponent<BoxCollider2D>().enabled = true;
            if(itemtypys == Itemtypy.leaf)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0.08f;
                movespeed = 5.0f;
            }
               
            else
                GetComponent<Rigidbody2D>().gravityScale = 1;
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

        if(itemtypys == Itemtypy.leaf)
        {
            moveTime += Time.deltaTime;
            if(moveTime >= 0.5f)
            {
                randomWay = !randomWay;
                moveTime = 0;
            }
                
        }
               
    }

    void MoveStar()
    {
        ridy.mass = 4;
        if (!openItem)
        {

            ridy.velocity = new Vector2(movespeed * direction, ridy.velocity.y);

            // �ٴڿ� ���� ���¿����� ����
            if (isGround)
            {
                ridy.velocity = new Vector2(ridy.velocity.x, jumpForce);
                isGround = false;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyWall" && itemtypys != Itemtypy.leaf)
        {
            randomWay = !randomWay;
            direction *= -1;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}


