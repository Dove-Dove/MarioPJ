using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    //�޾ƿ� ���� ������Ʈ
    public GameObject PipemMovement;
    private GameObject Player;
    private GameManager gameManager;
    private GameObject cam;
    private GameObject FadeUi;

    public AudioSource pipeSound;

    
    private bool Move = false;

    //���� ������
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
        // ������ �̵� �� ȿ���� ���� 2.5�� ��ٸ�
        yield return new WaitForSecondsRealtime(2.5f);
        cam.GetComponent<CameraController>().inPipe();
        // �÷��̾� ��ġ�� ������ ��ǥ �������� �̵�
        FadeUi.GetComponent<UIManager>().outFade(true);
        Player.transform.position = PipemMovement.transform.position;
             
  
        // �ִϸ��̼� ���� �� ������ ���� �׼�
        if (outUp)
            Player.GetComponent<Player_Move>().PipeAction("Up");
        
        else if(!outUp)
            Player.GetComponent<Player_Move>().PipeAction("Down");
        pipeSound.Play();
        yield return new WaitForSecondsRealtime(2.5f);
        
        //���� �������� ������ ���� �ִϸ��̼� ����
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

        // �ʱ�ȭ
        Time.timeScale = 1;
        outPipe = false;
        inPipe = false;
   

        
    }
}
