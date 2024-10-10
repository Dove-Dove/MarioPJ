using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    int coin = 0;
    int point = 0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCoin()
    {
        coin++;
        print(coin);
    }
    public void GetPoint(int getpoint)
    {
        point += getpoint;
    }
}
