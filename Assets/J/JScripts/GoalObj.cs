using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalObj : MonoBehaviour
{
    //이미지 전환
    public Sprite[] spr;
    private float GoalTime = 4.0f;
    //반복실행 방지
    int count = 1;
    bool PlayerGoal = false;
    bool isGoal = false;
    float nextTime = 0.0f;


    // Update is called once per frame

    void Update()
    {
      
        nextTime += Time.unscaledDeltaTime;
        if (nextTime > 0.15f && !PlayerGoal)
        {
            count++;
            nextTime = 0.0f;
            if (count >= 4)
                count = 1;
        }

        if (PlayerGoal)
        {
            transform.Translate(Vector2.up * Time.unscaledDeltaTime * 10.0f);
            if(isGoal)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().getBouns(count);
                isGoal = false;
            }
          
            if (nextTime >= GoalTime)
            {
                Time.timeScale = 1;               
                SceneManager.LoadScene("SelectScene");
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
        {
            isGoal = true;
            PlayerGoal = true;
            GameObject.Find("Mario").GetComponent<Player_Move>().setMarioStatus(MarioStatus.Clear);
            GameObject.Find("GameManager").GetComponent<GameManager>().GameClear();
        }         
    }

}
