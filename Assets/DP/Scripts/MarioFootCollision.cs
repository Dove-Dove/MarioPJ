using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioFootCollision : MonoBehaviour
{
    public GameObject playerCom;
    //public GameObject shell;
    // Start is called before the first frame update
    void Start()
    {
        //playerCom = GameObject.Find("Mario");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position = playerCom.transform.position;
        Debug.Log(playerCom.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //¿¡³Ê¹Ì
        if (collision.gameObject.tag == "Enemy")
        {
            playerCom.GetComponentInChildren<Player_Move>().isAttack = true;
            Debug.Log("Attack!");
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }
}
