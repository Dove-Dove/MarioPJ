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

    public int coin = 0;
    public int point = 1000;
    //�������� 
    public Stage stage = Stage.Map;

    //�÷��̾�
    public GameObject Player;
    public bool breakBlock = false;
    private float playerSpeed = 0;
    private float playerMaxSpeed = 0; 
    public float runingTime = 0;
    public int PlayerLife = 3;

    //�÷��̾� ���ʽ�(������ ���������� ȹ���Ѱ�)
    public int[] BounsItem = new int[3] { 0,0,0 };

    //�� 
    public int UITime = 300;
    private float GameTime = 0.0f;
    private int Ones = 1;
    //����
    public AudioSource mapAudio;
    public AudioSource CoinSound;
    public AudioClip[] AllSound;
    

    void Start()
    {
        //�� ����
        switch (stage)
        {
            case Stage.Map:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[0];
                break;
            case Stage.stage1:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[1];
                break;
            case Stage.stage2:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[2];
                break;
            case Stage.stageBoss:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[3];
                break;
        }
        mapAudio.Play();
    }



    void Update()
    {
        { //������ ũ�⿡ ���� �� �μ��� ���� Ȯ��
            if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.NormalMario)
                breakBlock = false;

            else
                breakBlock = true;
        }

        //�Ŀ�
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

        if (BounsItem[2] > 0)
        {
            calculateBouns();
        }

        //�� �ð�(1�ʽ� �پ�
        GameTime += Time.deltaTime;
        if (GameTime >= Ones)
        {
            UITime -= 1;
            GameTime = 0;
        }

        if (coin >= 100)
        {
            coin = 0;
            PlayerLife++;
        }



    }

    public void CoinGet()
    {
        CoinSound.Play();
        coin++;
        print(coin);
    }
    public void GetPoint(int getpoint)
    {
        point += getpoint;
    }

    //�ð� �ʱ�ȭ �� �Լ�
    public void SetTime()
    {
        GameTime = 300.0f;
    }

    public void Dead()
    {
        PlayerLife--;
        mapAudio.Stop();
    }

    public void getLife(int life)
    {
        PlayerLife += life;
    }
    public void getBouns(int itemNumber)
    {
        for(int i = 0; i<BounsItem.Length; i++)
        {
            if (BounsItem[i] == 0)
            {
                BounsItem[i] = itemNumber;
                break;
            }
        }


    }

    private void calculateBouns()
    {
        if (BounsItem[0] == BounsItem[1] && BounsItem[1] == BounsItem[2])
        {
            if (BounsItem[0] == 1)
            {
                getLife(2);
            }
            else if (BounsItem[0] == 2)
            {
                getLife(3);
            }
            else if (BounsItem[0] == 3)
            {
                getLife(5);
            }
        }
        else
            getLife(1);

        BounsItem = new int[3]{ 0, 0, 0 };
    }
}
