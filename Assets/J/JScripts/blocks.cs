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

    public Animator animator;

    //����
    public AudioSource itemSound;
    public AudioSource hitFloorSound;
    public AudioSource coinSound;

    //�浹 ���� ������Ʈ
    public GameObject under;
    public GameObject BlockWall;

    void Start()
    {
        itemBocksOpen = false;
        itemPrefab.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (under.GetComponent<underCol>().WallUnderOpen
            || BlockWall.GetComponent<BlockWall>().WallOpen)
            itemBocksOpen = true;

        //��ġ�� �Ǿ��� ��� �������� �� Ȱ��ȭ ���·� ����
        if (itemBocksOpen)
        {
            GetComponent<SpriteRenderer>().sprite = OpenImg;
            
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
                animator.SetBool("Use", true);
                hitFloorSound.Play();
                itemSound.Play();
                itemPrefab.SetActive(true);
                gameObject.GetComponent<blocks>().enabled = false;
            }
        }
        
        
    }
}
