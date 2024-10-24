using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarioCollision : MonoBehaviour
{
    private PlatformEffector2D pletformCol;
    private GameObject playerCom;
    public GameObject shell;
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
            

            //playerCom.GetComponent<Player_Move>().setMarioTransform(MarioStatus.SuperMario);
            player.NotInput = true;
            //아이템 종류별 효과적용
            var type = collision.gameObject.GetComponent<items>().itemtypys;
            switch(type)
            {
                case Itemtypy.mushroom://버섯
                    if(player.getMarioStatus() == MarioStatus.SuperMario 
                        || player.getMarioStatus() == MarioStatus.RaccoonMario 
                        || player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        Destroy(collision.gameObject); return; //점수추가
                    }
                    else if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.leaf://나뭇잎
                    if (player.getMarioStatus() == MarioStatus.RaccoonMario)
                    {
                        Destroy(collision.gameObject); return; //점수추가
                    }
                    else if(player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    else
                    {
                        player.setMarioStatus(MarioStatus.RaccoonMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.flower://불 꽃
                    if (player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        Destroy(collision.gameObject); return; //점수추가
                    }
                    else if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    else
                    {
                        player.setMarioStatus(MarioStatus.FireMario);
                        player.setChangeStatus();
                    }
                    break;
            }
            //TODO:정확한 확인과정 추가
            Destroy(collision.gameObject);
        }

        //에너미
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "MovingShell")
        {
            if(player.isEnemy)
            {
                player.isAttack = true;
            }
            else
           { player.ishit = true; }
            
        }
        //에너미
        if (collision.gameObject.tag == "Shell")
        {
            player.isLift = true;
            shell = collision.gameObject;
        }


        //에너미
        if (collision.gameObject.tag == "NoteBlock")
        {
            if (player.isNoteblock)
            {
                Debug.Log("노트박스");
                player.isNoteblockJump = true;
            }
            else
            { player.isNoteblockJump = false; }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shell")
        {
            if(player.isKick)
            {
                shell.GetComponent<Tuttle>().currentState = Enemy.State.ShellMove;
                shell = null;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }
    //트리거(E_Attack)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //에너미
        if ( collision.gameObject.tag == "E_Attack")
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
