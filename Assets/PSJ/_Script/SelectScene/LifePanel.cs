using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    private GameManager gameManager;

    int life;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        life = gameManager.PlayerLife;

    }

    

}
