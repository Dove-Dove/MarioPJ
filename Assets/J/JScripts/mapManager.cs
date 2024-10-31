using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private bool MapStart = true;
    private int player_state = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        gameManager = GameManager.Instance;
       

        if (gameManager == null)
        {
            //Debug.LogError("GameManager instance not found.");
            return;
        }

        if (MapStart)
        {
            Time.timeScale = 1;
            gameManager.SetStartMap(MapStart);
            MapStart = false;
        }

        player = GameObject.Find("Mario");

        player_state = gameManager.Player_State;

        if (player_state == 1)
        {
            player.GetComponent<Player_Move>().StartMarioStatus = MarioStatus.NormalMario;
        }
        else if (player_state == 2)
        {
            player.GetComponent<Player_Move>().StartMarioStatus = MarioStatus.SuperMario;
        }

        else if (player_state == 3)
        {
            player.GetComponent<Player_Move>().StartMarioStatus = MarioStatus.FireMario;
        }

        else if (player_state == 4)
        {
            player.GetComponent<Player_Move>().StartMarioStatus = MarioStatus.RaccoonMario;
        }
    }

}
