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
        //������
        if(collision.collider.tag=="Items")
        {
            

            //playerCom.GetComponent<Player_Move>().setMarioTransform(MarioStatus.SuperMario);
            player.NotInput = true;
            //������ ������ ȿ������
            var type = collision.gameObject.GetComponent<items>().itemtypys;
            switch(type)
            {
                case Itemtypy.mushroom://����
                    if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    else
                    {
                        Destroy(collision.gameObject); return; //�����߰�
                    }
                    break;
                case Itemtypy.leaf://������
                    if (player.getMarioStatus() == MarioStatus.RaccoonMario)
                    {
                        Destroy(collision.gameObject); return; //�����߰�
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
                case Itemtypy.flower://�� ��
                    if (player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        Destroy(collision.gameObject); return; //�����߰�
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
                case Itemtypy.star://�� TODO: ��������
                    player.setMarioStatus(MarioStatus.InvincibleMario);
                    player.setChangeStatus();
                    break;
            }
            //TODO:��Ȯ�� Ȯ�ΰ��� �߰�
            Destroy(collision.gameObject);
        }

        //���ʹ�
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "E_Attack")
        {
            if(playerCom.GetComponentInChildren<Player_Move>().isEnemy)
            {
                playerCom.GetComponentInChildren<Player_Move>().isAttack = true;
            }
            else
           { playerCom.GetComponentInChildren<Player_Move>().ishit = true; }
            
        }
        //���ʹ�
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
