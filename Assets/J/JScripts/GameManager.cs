using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage
{
    Map,
    stage1,
    stage2,
    stageBoss
}

public class GameManager : MonoBehaviour
{
    
    int coin = 0;
    public int point = 1000;
    //스테이지 
    public Stage stage = Stage.Map;

    //플레이어
    public GameObject Player;
    public bool breakBlock = false;
    private float playerSpeed = 0;
    private float playerMaxSpeed = 0; 
    public float runingTime = 0;
    public int PlayerLife = 3;

    //맵 사운드 
    public AudioSource mapAudio;
    public AudioClip[] AllSound; 


    // Update is called once per frame
    void Update()
    {
        { //마리오 크기에 따른 블럭 부수기 상태 확인
            if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.NormalMario)
                breakBlock = false;

            else
                breakBlock = true;
        }

        playerSpeed = Player.GetComponentInChildren<Player_Move>().rigid.velocity.x;
        playerMaxSpeed = Player.GetComponentInChildren<Player_Move>().addedLimitVelocity;

        if ( ((playerSpeed  > 4) || (playerSpeed < -4.0f)) && playerMaxSpeed == 7)
        {
            if(runingTime <= 3.0f)
                runingTime += Time.deltaTime + 0.005f;
        }        
        else
        {
            if (runingTime > 0)
                runingTime -= Time.deltaTime;
        }

        if (runingTime < 0)
            runingTime = 0;


        switch (stage)
        {
            case Stage.Map:
                break;
            case Stage.stage1:
                break;
            case Stage.stage2:
                break;
            case Stage.stageBoss: 
                break;

            
        }

        
    }

    public void GetCoin()
    {
        coin++;
        print(coin);
    }
    public void GetPoint(int getpoint)
    {
        point += getpoint;
    }

}
