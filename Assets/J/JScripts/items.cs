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
    public Vector2 target;
    int randomWay = 0;

    void Start()
    {
        target = new Vector2(transform.position.x, transform.position.y+1);
        randomWay = Random.Range(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        moveItem();
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

    void moveItem()
    {
     float speed = 1;

     //���� ������ ���� �ö� (���� ��ġ���� y ��ǥ +1 ��ŭ �ö� )
     transform.position = Vector2.MoveTowards(transform.position, target, speed * 0.1f);
     //�����ۿ� ���� �������� �޶���
    }
}


