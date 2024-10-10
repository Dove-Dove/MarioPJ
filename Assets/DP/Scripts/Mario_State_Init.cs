using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario_State_Init : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Mario");
        
    }

    public void SetLMario()
    {
        var mario = player.GetComponent<Player_Move>();
        mario.jumpInputTime = 0.4f;

    }
}
