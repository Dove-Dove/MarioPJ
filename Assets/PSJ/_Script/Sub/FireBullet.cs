using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float lifeTime = 3.0f;

    void Start()
    {
        Invoke("destroy", lifeTime);
    }

    void destroy()
    {
        Destroy(gameObject);
    }
}
