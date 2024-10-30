using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject PipemMovement;
    public bool Move = false;

    public float pipeTime = 0;
    public bool inPipe = false;
    bool outPipe = false;

    void Start()
    {
        PipemMovement = GameObject.Find("Mario");
    }

    // Update is called once per frame
    void Update()
    {
        Move = PipemMovement.GetComponent<Player_Move>().isPipe;
        if (inPipe)
        {
            Time.timeScale = 0;
            pipeTime += Time.unscaledDeltaTime;
        }
          
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Move )
        {
            inPipe = true;

            //if (pipeTime >=3.0f )
            //{
            //    collision.gameObject.transform.position = PipemMovement.transform.position;
            //    Time.timeScale = 1;
            //    inPipe = false; 
            //    pipeTime = 0;
            //}

            //collision.gameObject.transform.position = PipemMovement.transform.position;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (pipeTime >= 3.0f)
        {
            Time.timeScale = 1;
            collision.gameObject.transform.position = PipemMovement.transform.position;
            inPipe = false;
            pipeTime = 0;
        }
    }

}
