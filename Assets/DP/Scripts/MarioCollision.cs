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
        //������
        if(collision.collider.tag=="Items")
        {
            //������ ������ ȿ������
            var type = collision.gameObject.GetComponent<items>().itemtypys;
            switch(type)
            {
                case Itemtypy.mushroom://����
                    if(player.getMarioStatus() == MarioStatus.SuperMario 
                        || player.getMarioStatus() == MarioStatus.RaccoonMario 
                        || player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        player.powerUpSound.Play();
                        //Destroy(collision.gameObject); return; //�����߰�
                    }
                    else if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        //�̵��Ұ� �� ����
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.leaf://������
                    if (player.getMarioStatus() == MarioStatus.RaccoonMario)
                    {
                        player.powerUpSound.Play();
                        //Destroy(collision.gameObject); return; //�����߰�
                    }
                    else if(player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        //�̵��Ұ� �� ����
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                        break;
                    }
                    else
                    {
                        //�̵��Ұ� �� ����
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.RaccoonMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.flower://�� ��
                    if (player.getMarioStatus() == MarioStatus.FireMario)
                    {
                        player.powerUpSound.Play();
                        //Destroy(collision.gameObject); return; //�����߰�
                    }
                    else if (player.getMarioStatus() == MarioStatus.NormalMario)
                    {
                        //�̵��Ұ� �� ����
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.SuperMario);
                        player.setChangeStatus();
                        break;
                    }
                    else
                    {
                        //�̵��Ұ� �� ����
                        player.NotInput = true;
                        player.setMarioStatus(MarioStatus.FireMario);
                        player.setChangeStatus();
                    }
                    break;
                case Itemtypy.star://��Ÿ
                    player.isInvincibleStar = true;
                    //Destroy(collision.gameObject);
                    break;
            }
            player.powerUpSound.Play();
            //Destroy(collision.gameObject);
        }

        //�������°� �ƴϸ�
        if (!player.isInvincibleStar || !player.isInvincible)
        {
            //���ʹ�
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "MovingShell" || collision.gameObject.tag == "BossAttack" ||collision.gameObject.name=="Flower")
            {
                if (player.isEnemy)
                {
                    player.isAttack = true;
                }
                else
                { player.ishit = true; }

            }
            //���ʹ�
            if (collision.gameObject.tag == "Shell")
            {
                player.isLift = true;
                shell = collision.gameObject;
            }
        }


        //��Ʈ��
        if (collision.gameObject.tag == "NoteBlock")
        {
            if (player.isNoteblock)
            {
                if(collision.gameObject.GetComponentInChildren<noteBlocks>().Jump)
                {
                    //player.isNoteblockJump = true;
                    //player.isJumpInput = true;
                    player.isAttack = true;
                }
            }
            else
            { player.isNoteblockJump = false; }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shell" && !player.onAir)
        {
            if(player.isKick && !player.onGround)
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
                shell.GetComponent<Tuttle>().currentState = Enemy.State.ShellMove;
                shell = null;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //��Ʈ��
        if (collision.gameObject.tag == "NoteBlock")
        {

            player.isNoteblockJump = false;
        }
    }
    //Ʈ����(E_Attack)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������°� �ƴϸ�
        if (!player.isInvincibleStar || !player.isInvincible)
        {
            //���ʹ�
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
        //��
        if (collision.gameObject.tag == "Door")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.inDoor = true;
                //0.3�� �� false��
                StartCoroutine(player.FlaseInDoor());
            }
        }

    }
}
