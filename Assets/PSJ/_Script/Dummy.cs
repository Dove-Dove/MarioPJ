using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public GameObject Fire;

    public float distance = 4f; //������
    public float speed = 20f; //�ӵ�
    public float deg; //����

    void Update()
    {
        deg += Time.deltaTime * speed;
        if(deg<360)
        {
            var rad = Mathf.Deg2Rad * (deg);
            var x = distance + Mathf.Sin(rad);
            var y = distance + Mathf.Cos(rad);
            Fire.transform.position = transform.position + new Vector3(x, y);
        }
        else
        { deg = 0; }

    }
}
