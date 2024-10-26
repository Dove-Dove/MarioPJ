using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapManager : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found.");
            return;
        }

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
           // gameManager.SetTime();
        }

    }

    // Update is called once per frame
    void Update()
    {
        

        
    }
}
