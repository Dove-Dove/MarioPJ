using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarioCollision : MonoBehaviour
{
    private PlatformEffector2D pletformCol;
    private GameObject playerCom;
    public GameObject shell;
    // Start is called before the first frame update
    void Start()
    {
        pletformCol = GetComponent<PlatformEffector2D>();
        playerCom = GameObject.Find("Mario");
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
            //TODO:정확한 확인과정 추가
            Destroy(collision.gameObject);

            //playerCom.GetComponent<Player_Move>().setMarioTransform(MarioStatus.SuperMario);
            playerCom.GetComponent<Player_Move>().NotInput = true;
            playerCom.GetComponent<Player_Move>().UpdateMarioStatusAndHP(MarioStatus.SuperMario);
        }

        //에너미
        if (collision.gameObject.tag == "Enemy")
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

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
}
