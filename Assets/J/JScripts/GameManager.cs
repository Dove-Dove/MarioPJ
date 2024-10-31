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
    // �̱��� �ν��Ͻ�
    public static GameManager Instance { get; private set; }

    public int coin;
    public int point;
    //�������� 
    public Stage stage = Stage.Map;

    //�÷��̾�
    private GameObject Player;
    public bool breakBlock = false;
    private float playerSpeed = 0;
    private float playerMaxSpeed = 0; 
    public float runingTime = 0;
    public int PlayerLife;
    public int Player_State = 0;

    //�÷��̾� ����� 
    public float deadTime = 0.0f;
    private bool playerDead = false;

    //ī�޶�
    private GameObject Cam;


    //�÷��̾� ���ʽ�(������ ���������� ȹ���Ѱ�)
    public int[] BounsItem = new int[3] { 0,0,0 };

    //�� 
    //�� ���� ���� �ð�
    public int UITime;
    //1�ʾ� �پ�鵵�� ����� ���� �ð�
    private float GameTime = 0.0f;
    private int Ones = 1;
    //����
    public AudioSource mapAudio;
    public AudioSource CoinSound;
    public AudioSource ClearSound;
    public AudioClip[] AllSound;

    public int GameClearStage = 0;
    public int GameCurrentStage = 0;


    private void Awake()
    {
        Application.targetFrameRate = 60;

        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �Ŀ��� GameManager ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� GameManager�� ������ ����
        }

        Player = GameObject.Find("Mario");
        Cam = GameObject.Find("Main Camera");

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;


    }


    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Mario"); // �� �ε� �� �ٽ� Player ã��
        }
        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
        }

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
        if (SceneManager.GetActiveScene().name == "SelectScene")
            return;

        if (Player == null)
        {
            Player = GameObject.Find("Mario");
            if (Player == null)
            {
                Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�.");
                return;
            }
        }

        if (Cam == null)
        {
            Cam = GameObject.Find("Main Camera");
            if (Cam == null)
            {
                Debug.LogWarning("Camera ������Ʈ�� ã�� �� �����ϴ�.");
                return;
            }
        }

        // ���� �������� 
        {
            if (SceneManager.GetActiveScene().name == "Map1-1")
                GameCurrentStage = 1;
            else if (SceneManager.GetActiveScene().name == "Map1-2")
                GameCurrentStage = 2;
            else if (SceneManager.GetActiveScene().name == "Map1-3")
                GameCurrentStage = 3;
            else if (SceneManager.GetActiveScene().name == "Map1-4")
                GameCurrentStage = 4;
            else if (SceneManager.GetActiveScene().name == "MapBoss")
                GameCurrentStage = 5;
        }


        if (Player == null)
        {
            Player = GameObject.Find("Mario");  // "Mario"�� Player ������Ʈ�� �̸����� �������ּ���
            if (Player == null)
            {
                Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�.");
                return;
            }
        }

        { //������ ũ�⿡ ���� �� �μ��� ���� Ȯ�� �� ���� ����
            // ���� ���� ���ϴ� ���� ���������� �Ѿ���� ������ ����
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


        if (Player.GetComponentInChildren<Player_Move>().getMarioStatus() == MarioStatus.Death)
        {

            deadTime += Time.unscaledDeltaTime;
            Dead();
        }

    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� �ε�� �� ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetTime();  // �� ��ȯ �� SetTime() ȣ��
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
        if (Player == null || Cam == null)
            return; // Player�� Cam�� null�̸� �޼��� ����

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
        GameClearStage++;
    }

}
