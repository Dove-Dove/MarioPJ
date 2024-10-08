using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class blocks : MonoBehaviour
{
    //
    public bool itemBocksOpen;
    //������ ������
    public GameObject itemPrefab;
    //������ �ڽ� ���� ���� �̹���
    public Sprite OpenImg;
    void Start()
    {
        itemBocksOpen = false;
        itemPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //��ġ�� �Ǿ��� ��� �������� �� Ȱ��ȭ ���·� ����
        if (itemBocksOpen)
        {
            GetComponent<SpriteRenderer>().sprite = OpenImg;
            
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
                itemPrefab.SetActive(true);
                gameObject.GetComponent<blocks>().enabled = false;
            }
        }
        
        
    }
}
