using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObj : MonoBehaviour
{
    public Sprite[] pointImg;
    public SpriteRenderer sprRen;

    int point = 100;


    public bool start = false;
    private Vector2 EndPos;
    private float MoveSpeed = 1.0f;

    void Start()
    {
        EndPos.y = transform.position.y + 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

      if (transform.position.y <= EndPos.y)
       {
         transform.Translate(Vector2.up * MoveSpeed * Time.unscaledDeltaTime);
       }
      else
      {
         start = false; 
         gameObject.SetActive(false);
      }


           
    }

    public void setPos(int point)
    {

        switch (point)
        {
            case 100:
                sprRen.sprite = pointImg[0];
                break;
            case 200:
                sprRen.sprite = pointImg[0];
                break;
            case 400:
                sprRen.sprite = pointImg[0];
                break;
        }
    }
}
