using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScore : MonoBehaviour
{
    private GameManager gameManager;
    public int points = 100;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        if ( gameManager != null )
        {
            gameManager.GetPoint(points);
        }

        Destroy(gameObject, 0.5f);
    }

}
