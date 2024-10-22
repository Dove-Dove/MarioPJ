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
                    if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    else
                    {
                        Destroy(collision.gameObject); return; //점수추가
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
                case Itemtypy.star://별 TODO: 만들어야함
                    player.setMarioStatus(MarioStatus.InvincibleMario);
                    player.setChangeStatus();
                    break;
            }
            //TODO:정확한 확인과정 추가
            Destroy(collision.gameObject);
        }

        //에너미
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "E_Attack")
        {
            if(playerCom.GetComponentInChildren<Player_Move>().isEnemy)
            {
                playerCom.GetComponentInChildren<Player_Move>().isAttack = true;
            }
            else
           { playerCom.GetComponentInChildren<Player_Move>().ishit = true; }
            
        }
        //에너미
        if (collision.gameObject.tag == "Shell")
        {
            playerCom.GetComponentInChildren<Player_Move>().isLift = true;
            shell = collision.gameObject;
        }
        else
        { shell=null; }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
