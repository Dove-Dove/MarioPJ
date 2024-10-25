using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMove : MonoBehaviour
{

    float MoveTime = 0.0f;

    public bool left = true;
    public bool up = true;  
    public Rigidbody2D rigd;

    
    // Start is called before the first frame update
    void Start()
    {
        MoveTime = 0.0f;
        rigd.GetComponent<Rigidbody2D>().gravityScale = -2;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTime += Time.deltaTime;

        if (left)
        {
            transform.Translate(Vector2.left * Time.deltaTime * 5.0f);
        }
        else
        {
            transform.Translate(Vector2.right * Time.deltaTime * 5.0f);
        }


        if (MoveTime >= 0.2f && !up)
            rigd.gravityScale = 3.0f;
        else if(MoveTime >= 0.3)
            rigd.gravityScale = 3.0f;

        if(MoveTime >=1.0f)
            gameObject.SetActive(false);
    }
}
