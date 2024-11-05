using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    //받아올 게임 오브젝트
    public GameObject PipemMovement;
    private GameObject Player;
    private GameManager gameManager;
    private GameObject cam;
    private GameObject FadeUi;

    public AudioSource pipeSound;

    
    private bool Move = false;

    //현재 파이프
    private bool inPipe = false;
    private bool outPipe = false;
    public bool outUp = false;
    void Start()
    {
        Player = GameObject.Find("Mario");
        gameManager = GameManager.Instance;
        cam = GameObject.Find("Main Camera");
        FadeUi = GameObject.Find("Canvas");
    }

    void Update()
    {
        Move = Player.GetComponent<Player_Move>().isPipe;

        if (Move && inPipe && !outPipe)
        {
            outPipe = true;
            inPipe = false;
            
            StartCoroutine(PipeTransition());
        }
    }
     
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inPipe = true;
            
        }
    }

    private IEnumerator PipeTransition()
    {
        Time.timeScale = 0;
        pipeSound.Play();
        // 파이프 이동 중 효과를 위해 2.5초 기다림
        yield return new WaitForSecondsRealtime(2.5f);
        cam.GetComponent<CameraController>().inPipe();
        // 플레이어 위치를 파이프 목표 지점으로 이동
        FadeUi.GetComponent<UIManager>().outFade(true);
        Player.transform.position = PipemMovement.transform.position;
             
  
        // 애니메이션 설정 및 파이프 진입 액션
        if (outUp)
            Player.GetComponent<Player_Move>().PipeAction("Up");
        
        else if(!outUp)
            Player.GetComponent<Player_Move>().PipeAction("Down");
        pipeSound.Play();
        yield return new WaitForSecondsRealtime(2.5f);
        
        //현재 마리오의 상태의 따라 애니메이션 변경
        switch (gameManager.Player_State)
        {
            case 1:
                Player.GetComponent<Animator>().Play("LMario_idle");
                break;
            case 2:
                Player.GetComponent<Animator>().Play("SMario_idle");
                break;
            case 3:
                Player.GetComponent<Animator>().Play("FMario_idle");
                break;
            case 4:
                Player.GetComponent<Animator>().Play("RMario_idle");
                break;
        }

        // 초기화
        Time.timeScale = 1;
        outPipe = false;
        inPipe = false;
   

        
    }
}
