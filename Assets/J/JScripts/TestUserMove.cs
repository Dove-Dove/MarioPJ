using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUserMove : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Mario");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.Find("Mario"); // �� �ε� �� �ٽ� Player ã��
        }

        if(Input.GetKeyDown(KeyCode.U)) Player.GetComponent<Transform>().transform.position = transform.position;

            
    }
}
