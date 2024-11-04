using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarioCollision : MonoBehaviour
{
    private PlatformEffector2D pletformCol;
    private GameObject playerCom;
    public GameObject shell;
    public GameObject marioFoot;
    private Player_Move player;
    // Start is called before the first frame update
    void Start()
    {
        pletformCol = GetComponent<PlatformEffector2D>();
        playerCom = GameObject.Find("Mario");
        player = playerCom.GetComponent<Player_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //아이템
        if(collision.collider.tag=="Items")
        {
            //아이템 종류별 효과적용
            var type = collision.gameObject.GetComponent<items>().itemtypys;
            switch(type)
            {
                case Itemtypy.mushroom://버섯
                    if(player.getMarioStatus() == MarioStatus.SuperMario 
                        || player.getMarioStatus() == MarioStatus.RaccoonMario 
                        || player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        player.powerUpSound.Play();
                        //Destroy(collision.gameObject); return; //점수추가
                    }
                    else if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        //이동불가 및 변신
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.leaf://나뭇잎
                    if (player.getMarioStatus() == MarioStatus.RaccoonMario)
                    {
                        player.powerUpSound.Play();
                        //Destroy(collision.gameObject); return; //점수추가
                    }
                    else if(player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        //이동불가 및 변신
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                        break;
                    }
                    else
                    {
                        //이동불가 및 변신
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.RaccoonMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.flower://불 꽃
                    if (player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        player.powerUpSound.Play();
                        //Destroy(collision.gameObject); return; //점수추가
                    }
                    else if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        //이동불가 및 변신
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                        break;
                    }
                    else
                    {
                        //이동불가 및 변신
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.FireMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.star://스타
                    player.isInvincibleStar = true;
                    //Destroy(collision.gameObject);
                    break;
            }
            player.powerUpSound.Play();
            //Destroy(collision.gameObject);
        }

        //무적상태가 아니면
        if (!player.isInvincibleStar || !player.isInvincible)
        {
            //에너미
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "MovingShell" || collision.gameObject.tag == "BossAttack" ||collision.gameObject.name=="Flower")
            {
                if (player.isEnemy)
                {
                    player.isAttack = true;
                }
                else
                { player.ishit = true; }

            }
            //에너미
            if (collision.gameObject.tag == "Shell")
            {
                shell = null;
                player.isLift = true;
                shell = collision.gameObject;
            }
        }


        //노트블럭
        if (collision.gameObject.tag == "NoteBlock")
        {
            if (player.isNoteblock)
            {
                if(collision.gameObject.GetComponentInChildren<noteBlocks>().Jump)
                {
                    player.isAttack = true;
                }
            }
            else
            { player.isNoteblockJump = false; }
        }

        //일반 킥
        if (collision.gameObject.tag == "Shell" && !player.onAir)
        {
            if (player.isKick && !player.isEnemy)
            {
                switch (player.getMarioStatus())
                {
                    case MarioStatus.NormalMario:
                        player.animator.Play("LMario_kick");
                        break;
                    case MarioStatus.SuperMario:
                        player.animator.Play("SMario_kick");
                        break;
                    case MarioStatus.FireMario:
                        player.animator.Play("FMario_kick");
                        break;
                    case MarioStatus.RaccoonMario:
                        player.animator.Play("RMario_kick");
                        break;
                }
                if (shell)
                {
                    if (player.isRight)
                        shell.GetComponent<Tuttle>().movingLeft = false;
                    else
                        shell.GetComponent<Tuttle>().movingLeft = true;

                    shell.GetComponent<Tuttle>().currentState = Enemy.State.ShellMove;

                }
                //킥 사운드
                if (!player.iskcikSound)
                {
                    player.iskcikSound = true;
                    player.kickSound.Play();
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //잡고있을 때 킥
        if (collision.gameObject.tag == "Shell")
        {
            if(player.isKick&& Input.GetKeyUp(KeyCode.Z))
            {
                switch (player.getMarioStatus())
                {
                    case MarioStatus.NormalMario:
                        player.animator.Play("LMario_kick");
                        break;
                    case MarioStatus.SuperMario:
                        player.animator.Play("SMario_kick");
                        break;
                    case MarioStatus.FireMario:
                        player.animator.Play("FMario_kick");
                        break;
                    case MarioStatus.RaccoonMario:
                        player.animator.Play("RMario_kick");
                        break;
                }
                if (shell)
                {
                    if(player.isRight)
                        shell.GetComponent<Tuttle>().movingLeft=false;
                    else
                        shell.GetComponent<Tuttle>().movingLeft = true;

                    shell.GetComponent<Tuttle>().currentState = Enemy.State.ShellMove; 

                }

                //킥 사운드
                if (!player.iskcikSound)
                {
                    player.iskcikSound = true;
                    player.kickSound.Play();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

 
        if (collision.gameObject.tag == "Shell")
        {
            //쉘 매모리 해제
            shell = null;
            player.iskcikSound = false;
        }
        //노트블럭
        if (collision.gameObject.tag == "NoteBlock")
        {

            player.isNoteblockJump = false;
        }
    }
    //트리거(E_Attack)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //무적상태가 아니면
        if (!player.isInvincibleStar || !player.isInvincible)
        {
            //에너미
            if (collision.gameObject.tag == "E_Attack")
            {
                if (player.isEnemy)
                {
                    player.isAttack = true;
                }
                else
                { player.ishit = true; }

            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //문
        if (collision.gameObject.tag == "Door")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.inDoor = true;
                //0.3초 후 false로
                StartCoroutine(player.FlaseInDoor());
            }
        }

    }
}
