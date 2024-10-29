using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScore : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager.GetPoint(100);
        Invoke("DestroyScore", 0.5f);
    }

    void DestroyScore()
    {
        Destroy(gameObject);
    }
}
