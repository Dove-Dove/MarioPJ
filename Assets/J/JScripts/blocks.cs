using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocks : MonoBehaviour
{
    //
    public bool itemBocks;
    //������ ������
    public GameObject itemPrefab;
    //������ �ڽ� ���� ���� �̹���
    public Sprite OpenImg;
    void Start()
    {
        itemBocks = false;
    }

    // Update is called once per frame
    void Update()
    {
        //��ġ�� �Ǿ��� ��� �������� �� Ȱ��ȭ ���·� ����
        if (itemBocks)
        {
            GetComponent<SpriteRenderer>().sprite = OpenImg;
            
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
                
            }
        }
        
        
    }
}
