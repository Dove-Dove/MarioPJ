using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    //아이템이 올라오는 위치 
    public Vector2 target;
    //아이템 이동속도(버섯, 별)
    public float movespeed =2 ;
    // 좌우 랜덤용 함수
    public bool randomWay = false;

    bool openItem = true;

    public Transform RayLeft, RayRight;
    public float rayDistance = 0.1f;
    public LayerMask groundLayer;   

    void Start()
    {
        target = new Vector2(transform.position.x, transform.position.y+1);
        randomWay = (Random.value > 0.5f);

        GetComponent<BoxCollider2D>().enabled = false;
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
    }

    void upItem()
    {
     float speed = 1;

     //먼저 아이템 위로 올라감 (현재 위치에서 y 좌표 +1 만큼 올라감 )
     //transform.position = Vector2.MoveTowards(transform.position, target, speed * 0.1f);
        if(openItem && (transform.position.y <= target.y-0.1))
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * 0.021f);
        }
        else
        {
            openItem = false;
            GetComponent<BoxCollider2D>().enabled = true;
            
        }

            
     //아이템에 따라서 움직임이 달라짐
    }

    void MoveItems()
    {
        RaycastHit2D hitLeft =
            Physics2D.Raycast(RayLeft.position, Vector2.left, rayDistance, groundLayer);
        RaycastHit2D hitRigth =
            Physics2D.Raycast(RayRight.position, Vector2.right, rayDistance, groundLayer);

        Debug.DrawRay(RayRight.position, Vector2.right * rayDistance, Color.yellow);
        Debug.DrawRay(RayLeft.position, Vector2.left * rayDistance, Color.red);

        if (hitLeft || hitRigth)
        {
            randomWay = !randomWay;
        }
            


        if (!openItem && itemtypys == Itemtypy.mushroom)
        {
            if (randomWay)
                transform.Translate(Vector2.left * movespeed * Time.deltaTime);
            else if(!randomWay)
                transform.Translate(Vector2.right * movespeed * Time.deltaTime);
        }
               
    }
}


