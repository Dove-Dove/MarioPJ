using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject PipemMovement;
    public bool Move = false;

    float pipeTime = 0; 
    bool inPipe = false;
    bool outPipe = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inPipe)
            pipeTime = Time.unscaledDeltaTime;

   

            
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Move )
        {
            inPipe = true;
            if (pipeTime >= 3.0f)
                collision.gameObject.transform.position = PipemMovement.transform.position;
            
            
        }
    }

}
