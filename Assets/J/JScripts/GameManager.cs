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
    int point = 0;
    //�������� 
    public Stage stage = Stage.Map;

    //�÷��̾�
    public GameObject Player;
    public bool breakBlock = false;
    public float playerSpeed = 0;
    public int PlayerLife = 3;

    //�� ���� 
    public AudioSource mapAudio;
    public AudioClip[] AllSound; 


    // Update is called once per frame
    void Update()
    {
        { //������ ũ�⿡ ���� �� �μ��� ���� Ȯ��
            if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.NormalMario)
                breakBlock = false;

            else
                breakBlock = true;
        }

        playerSpeed = Player.GetComponentInChildren<Player_Move>().curAnimSpeed;


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
