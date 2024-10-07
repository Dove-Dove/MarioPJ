using System.Collections;
using System.Collections.Generic;
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
    public bool touchItem = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if (touchItem)
        {
            //먼저 아이템 위로 올라감

            //아이템에 따라서 움직임이 달라짐

        }
    }
}


