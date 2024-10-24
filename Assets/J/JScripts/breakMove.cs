using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMove : MonoBehaviour
{
    Rigidbody2D ridy;

    float MoveTime = 0.0f;

    public bool left = true;
    
    // Start is called before the first frame update
    void Start()
    {
        MoveTime = 0.0f;
        ridy.gravityScale = -0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTime += Time.deltaTime; 

        if(left)
        {
            transform.Translate(Vector2.left * Time.deltaTime *2.0f);
        }
    }
}
