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
        //������
        if(collision.collider.tag=="Items")
        {
            //TODO:��Ȯ�� Ȯ�ΰ��� �߰�
            Destroy(collision.gameObject);

            //playerCom.GetComponent<Player_Move>().setMarioTransform(MarioStatus.SuperMario);
            playerCom.GetComponent<Player_Move>().NotInput = true;
            playerCom.GetComponent<Player_Move>().UpdateMarioStatusAndHP(MarioStatus.SuperMario);
        }


    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    //Ʈ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���ʹ�
        if (collision.tag == "Enemy")
        {
            playerCom.GetComponentInChildren<Player_Move>().ishit = true;
            //playerCom.GetComponentInChildren<Player_Move>().animator.SetBool("isHit", true);
        }
        //���ʹ�
        if (collision.tag == "Attack")
        {
            playerCom.GetComponentInChildren<Player_Move>().isLift = true;
            shell = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
