using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapManager : MonoBehaviour
{
    private GameManager gameManager;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found.");
            return;
        }

        if (Time.timeScale == 0 && count == 0)
        {
            Time.timeScale = 1;
            gameManager.StartMap();
            count = 1;
        }

    }

}
