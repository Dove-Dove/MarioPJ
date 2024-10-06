using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class followplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var playerTrans = GameObject.FindWithTag("Player").transform;
        Vector2 playerPos= new Vector2(playerTrans.position.x, playerTrans.transform.position.y);

        transform.SetPositionAndRotation(playerPos, transform.rotation);
    }
}
