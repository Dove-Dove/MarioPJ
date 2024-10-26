using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Stage
{
    Map,
    stage1,
    stage2,
    stageBoss
}

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    public int coin;
    public int point;
    //스테이지 
    public Stage stage = Stage.Map;

    //플레이어
    private GameObject Player;
    public bool breakBlock = false;
    private float playerSpeed = 0;
    private float playerMaxSpeed = 0; 
    public float runingTime = 0;
    public int PlayerLife;



    //플레이어 보너스(마지막 골지점에서 획득한거)
    public int[] BounsItem = new int[3] { 0,0,0 };

    //맵 
    //맵 들어가고 나서 시간
    public int UITime;
    //1초씩 줄어들도록 만들기 위한 시간
    private float GameTime = 0.0f;
    private int Ones = 1;
    //사운드
    public AudioSource mapAudio;
    public AudioSource CoinSound;
    public AudioSource ClearSound;
    public AudioClip[] AllSound;


    private void Awake()
    {
        // 싱글톤 패턴 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 후에도 GameManager 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager가 있으면 삭제
        }

        Player = GameObject.Find("Mario");

    }


    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Mario"); // 씬 로드 시 다시 Player 찾기
        }

        //맵 사운드
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
        if (Player == null)
        {
            Player = GameObject.Find("Mario");  // "Mario"는 Player 오브젝트의 이름으로 설정해주세요
            if (Player == null)
            {
                Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
                return;
            }
        }

        { //마리오 크기에 따른 블럭 부수기 상태 확인
            if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.NormalMario)
                breakBlock = false;

            else
                breakBlock = true;
        }

        if (Player != null)
        {
            playerSpeed = Player.GetComponentInChildren<Player_Move>().rigid.velocity.x;
            playerMaxSpeed = Player.GetComponentInChildren<Player_Move>().addedLimitVelocity;
        }

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

        //맵 시간(1초식 줄어
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

    //시간 초기화 용 함수
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

    public void StartGame()
    {
        PlayerLife = 3;
        point = 1000;
        coin = 0;
    }

    

    public void StartMap()
    {
        UITime = 300;
    }

    public void GameClear()
    {
        mapAudio.GetComponent<AudioSource>().Stop();
        ClearSound.Play();
    }


}
