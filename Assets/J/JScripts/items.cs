using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

    //아이템이 올라오는 위치 
    public Vector2 target;
    //아이템 이동속도(버섯, 별)
    public float movespeed =5 ;
    // 좌우 랜덤용 함수
    public bool randomWay = false;
    float jumpForce = 5.0f;
    private int direction = 1;

    private float moveTime = 0 ;
    //별일때 땅으로 떨어지는것을 이용하기 위해 만듬
    private bool isGround = false;

    private bool PlayerEnter= false;
    private float EnterTime = 0.0f;

    //아이템이 한번 올라가는것을 확인 
    private bool openItem = true;
    //위에서 밑으로(노트블럭) 친것을 확인
    private bool downItem = false;

    //별일때 충돌 조건 충돌
    public float StarUpTime = 0.0f;

    private GameManager gameManager;
    public PhysicsMaterial2D mat;

    public GameObject Poits;


    void Start()
    {
        gameManager = GameManager.Instance;
        if (itemtypys == Itemtypy.star)
            mat.bounciness = 0.5f;
        if (gameManager.Player_State == 1 && itemtypys != Itemtypy.star)
            itemtypys = Itemtypy.mushroom;

        if (downItem)
            target = new Vector2(transform.position.x, transform.position.y - 0.4f);
        else if (itemtypys == Itemtypy.star)
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
        if(PlayerEnter)
        {
            EnterTime += Time.unscaledDeltaTime;
            if(EnterTime > 2.0f)
                Destroy(gameObject);
            return;
        }

        if(downItem)
            DownItems();
        else
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

        if(StarUpTime <= 2.0f && itemtypys == Itemtypy.star)
        {
            StarUpTime += Time.deltaTime;
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

     //먼저 아이템 위로 올라감 (현재 위치에서 y 좌표 +1 만큼 올라감 )
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
     //아이템에 따라서 움직임이 달라짐
    }

    void DownItems()
    {
        //먼저 아이템 위로 올라감 (현재 위치에서 y 좌표 +1 만큼 올라감 )
        if (openItem && (transform.position.y >= target.y + 0.1))
        {
            if (itemtypys == Itemtypy.leaf)
                movespeed = 7.0f;

            transform.position = Vector2.MoveTowards(transform.position, target, movespeed * Time.deltaTime);
        }
        else
        {
            openItem = false;
            GetComponent<BoxCollider2D>().enabled = true;
            if (itemtypys == Itemtypy.leaf)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0.08f;
                movespeed = 5.0f;
            }

            else
                GetComponent<Rigidbody2D>().gravityScale = 1;
        }
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

            // 바닥에 닿은 상태에서만 점프
            if (isGround)
            {
                ridy.velocity = new Vector2(ridy.velocity.x, jumpForce);
                isGround = false;
            }
        }
    }

    public void setDownItem()
    {
        downItem = true;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyWall" && itemtypys != Itemtypy.leaf)
        {
            randomWay = !randomWay;
            if (StarUpTime <= 2.0f) direction = 1;
            else direction *= -1;
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }

        if(collision.gameObject.tag == "Player")
        {
            mat.bounciness = 0f;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            gameManager.GetPoint(100);
            Poits.GetComponent<PointObj>().setPos(100);
            Poits.SetActive(true);
            PlayerEnter = true;
            
        }
            

            
    }

}


