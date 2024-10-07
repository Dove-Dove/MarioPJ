using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocks : MonoBehaviour
{
    public bool itemBocks;
    public GameObject itemPrefab;
    public Sprite OpenImg;
    void Start()
    {
        itemBocks = false;
    }

    // Update is called once per frame
    void Update()
    {
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
