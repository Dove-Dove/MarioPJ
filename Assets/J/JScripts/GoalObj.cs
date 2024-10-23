using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalObj : MonoBehaviour
{
    public Sprite[] spr;

    int count = 1;
    bool PlayerGoal = false;

    float nextTime = 0.0f;

    //사운드 재생해야 하는대 사운드 자체가 없음

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        nextTime += Time.deltaTime;
        if (nextTime > 0.15f && !PlayerGoal)
        {
            count++;
            nextTime = 0.0f;
            if (count >= 4)
                count = 1;
        }

        if (PlayerGoal)
        {
            transform.Translate(Vector2.up * Time.deltaTime * 10.0f);
            if (nextTime >=2.0f)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().getBouns(count);
                gameObject.SetActive(false); 
            }
        }

        spriteImg(count);


    }

    void spriteImg(int count)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = spr[count-1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            PlayerGoal = true;

    }
}
