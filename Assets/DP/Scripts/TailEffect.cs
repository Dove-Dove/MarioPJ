using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailEffect : MonoBehaviour
{
    public GameObject tailEffect;
    // Start is called before the first frame update
    public GameObject tail;

    public void onTailEffectHitbox()
    {
        if (tail.GetComponent<TailCollision>().tailHIt)
        {
            tailEffect.transform.position = tail.GetComponent<TailCollision>().hitPos;
            tailEffect.SetActive(true); 
        }

    }

    public void offTailEffectHitbox()
    {
        tailEffect.SetActive(false);
    }
}
