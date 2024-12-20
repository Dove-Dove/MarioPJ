using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    public int coin;
    public int point;

    //플레이어
    private GameObject Player;
    public bool breakBlock = false;
    private float playerSpeed = 0;
    private float playerMaxSpeed = 0; 
    public float runingTime = 0;
    public int PlayerLife;
    public int Player_State = 0;

    //플레이어 별 먹었을때 
    private bool PlayerStar = false;
    private bool reStartBack = false;

    //플레이어 사망시 
    public float deadTime = 0.0f;
    private bool playerDead = false;
    private bool playerTimeOver =false; 

    //카메라
    private GameObject Cam;


    //플레이어 보너스(마지막 골지점에서 획득한거)
    public int[] BounsItem = new int[3] { 0,0,0 };

    //맵 
    //맵 들어가고 나서 시간
    public int UITime;
    //1초씩 줄어들도록 만들기 위한 시간
    private float GameTime = 0.0f;
    private int Ones = 1;
    bool StartMap = false;

    //사운드
    public AudioSource mapAudio;
    public AudioSource CoinSound;
    public AudioSource ClearSound;
    public AudioClip[] AllSound;

    //플레이어 스테이지 
    public int GameClearStage = 0;
    public int GameCurrentStage = 0;

    //기타
    //보너스 
    public bool LifeBouns = false;


    private void Awake()
    {
        Application.targetFrameRate = 60;

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
        Cam = GameObject.Find("Main Camera");

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "SelectScene")
        {
            mapAudio.GetComponent<AudioSource>().clip = AllSound[0];
            mapAudio.Play();
        }

    }


    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Mario"); // 씬 로드 시 다시 Player 찾기
        }
        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
        }

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SelectScene")
        {
            LifeBouns = false;
            return;
        }


        if (Player == null)
        {
            Player = GameObject.Find("Mario");
            if (Player == null)
            {
                Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
                return;
            }          
        }

        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
            if (Cam == null)
            {
                Debug.LogWarning("Camera 오브젝트를 찾을 수 없습니다.");
                return;
            }
        }




        PlayerStar = Player.GetComponent<Player_Move>().isInvincibleStar;

        // 선택 스테이지 
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Map1-1":
                    GameCurrentStage = 1;
                    break;
                case "Map1-2":
                    GameCurrentStage = 2;
                    break;
                case "Map1-3":
                    GameCurrentStage = 3;
                    break;
                case "Map1-4":
                    GameCurrentStage = 4;
                    break;
                case "MapBoss":
                    GameCurrentStage = 5;
                    break;
            }

            if (StartMap)
            {
                StartScene(GameCurrentStage);
            }

            if (PlayerStar || reStartBack)
                MarioStarSound(GameCurrentStage);
        }


        if (Player == null)
        {
            Player = GameObject.Find("Mario");  // "Mario"는 Player 오브젝트의 이름으로 설정해주세요
            if (Player == null)
            {
                Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
                return;
            }
        }

        { //마리오 크기에 따른 블럭 부수기 상태 확인 및 상태 저장
            // 상태 저장 안하니 다음 스테이지로 넘어갔을떄 문제가 생김
            if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.NormalMario)
            {
                Player_State = 1;
                breakBlock = false;
            }

            else
            {
                breakBlock = true;
                if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.SuperMario)
                    Player_State = 2;
                else if(Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.FireMario)
                    Player_State = 3;
                else if(Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.RaccoonMario)
                    Player_State = 4;                  
            }
                
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
            if(UITime <=0 && !playerTimeOver)
            {
                GameObject.Find("Mario").GetComponent<Player_Move>().setMarioStatus(MarioStatus.Death);
                playerTimeOver = true;
            }
        }

        if (coin >= 100)
        {
            coin = 0;
            PlayerLife++;
        }


        if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.Death)
        {

            deadTime += Time.unscaledDeltaTime;
            Dead();
        }



    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetTime();  // 씬 전환 시 SetTime() 호출
    }



    public void CoinGet()
    {
        CoinSound.Play();
        coin++;
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
        if (Player == null || Cam == null)
            return; // Player나 Cam이 null이면 메서드 종료

        if (!playerDead)
        {
            PlayerLife--;
            mapAudio.Stop();
            Cam.GetComponent<CameraController>().deadCam();
            playerDead = true;
        }

        if (deadTime >= 4.0f)
        {
            print(playerDead);
           
            Cam.GetComponent<CameraController>().deadCam();
            playerDead = false;
            deadTime = 0;
            Time.timeScale = 1.0f;
            Player_State = 1;
            if (PlayerLife <= 0)
                mapAudio.Stop();

            SceneManager.LoadScene("SelectScene");
        }
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
        if ( (BounsItem[0] == BounsItem[1] && BounsItem[1] == BounsItem[2]))
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

        LifeBouns = true;

        BounsItem = new int[3] { 0, 0, 0 };
    }

    private void StartScene(int BackMusicNumber)
    {
        UITime = 300;
        //맵 사운드
        switch (BackMusicNumber)
        {
            case 1:
            case 3:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[1];
                break;
            case 2:
            case 4:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[2];
                break;
            case 5:
                mapAudio.GetComponent<AudioSource>().clip = AllSound[3];
                break;
        }
        mapAudio.Play();

        StartMap = false;
        playerTimeOver = false;
        runingTime = 0;
    }

    private void MarioStarSound(int BackMusicNumber)
    {
        if(PlayerStar && !reStartBack)
        {
            mapAudio.Stop();
            mapAudio.GetComponent<AudioSource>().clip = AllSound[6];
            reStartBack = true;
            mapAudio.Play();
        }
        else if(!PlayerStar && reStartBack) 
        {
            mapAudio.Stop();
            switch (BackMusicNumber)
            {
                case 1:
                case 3:
                    mapAudio.GetComponent<AudioSource>().clip = AllSound[1];
                    break;
                case 2:
                case 4:
                    mapAudio.GetComponent<AudioSource>().clip = AllSound[2];
                    break;
                case 5:
                    mapAudio.GetComponent<AudioSource>().clip = AllSound[3];
                    break;
            }
            reStartBack = false;
            mapAudio.Play();
        }

    }


    //초기화 (게임 오버가 되면 초기화)
    public void StartGame()
    {
        PlayerLife = 3;
        point = 1000;
        coin = 0;
        GameClearStage = 0;
        GameCurrentStage = 0;
        BounsItem = new int[3] { 0, 0, 0 };
        mapAudio.Stop();
    }


    public void SetStartMap(bool Start)
    {
        StartMap = Start;
    }
    public void GameClear()
    {
        mapAudio.GetComponent<AudioSource>().Stop();
        Cam.GetComponent<CameraController>().pipeCam = false;
        ClearSound.Play();
        GameClearStage++;
    }

    public void orderMusicStart()
    {
        if(PlayerLife <=0)
            mapAudio.GetComponent<AudioSource>().clip = AllSound[5];
        else
            mapAudio.GetComponent<AudioSource>().clip = AllSound[0];
        mapAudio.Play();
    }
    public void BossMusicStart()
    {
        mapAudio.Stop();
        mapAudio.GetComponent<AudioSource>().clip = AllSound[4];
        mapAudio.Play();
    }


}
