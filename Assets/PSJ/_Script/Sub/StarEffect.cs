using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    public GameObject starAnim;

    public int n = 9; 
    public int speed = 100;
    public float timer = 1.5f;

    private void Start()
    {
        for (int i = 0; i < n; i++)
        {
            GameObject boltObj = Instantiate(starAnim, transform.position + new Vector3(0, 0.3f),
            Quaternion.identity);
            Vector2 dirVec = 
                new Vector2(Mathf.Cos((Mathf.PI) * 2 * i / (n - 1)), 
                Mathf.Sin((Mathf.PI) * i * 2 / (n - 1)));


            boltObj.GetComponent<Rigidbody2D>().AddForce(dirVec * speed);
            Destroy(boltObj, timer);
        }
    }
}
