using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float lifeTime = 3.0f;

    void Start()
    {
        Invoke("destroy", 3.0f);
    }

    void destroy()
    {
        Destroy(this);
    }
}
