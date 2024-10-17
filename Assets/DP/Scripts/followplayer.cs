using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class followplayer : MonoBehaviour
{
    public Transform playerTrans;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform)
        {
            playerTrans = player.transform;
        }

        Vector2 playerPos = new Vector2(playerTrans.position.x, playerTrans.position.y);
        transform.SetPositionAndRotation(playerPos, transform.rotation);



    }
}
