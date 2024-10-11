using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class blocks : MonoBehaviour
{
    //
    public bool itemBocksOpen;
    //아이템 프리펩
    public GameObject itemPrefab;
    //아이템 박스 열린 이후 이미지
    public Sprite OpenImg;

    public Animator animator;

    //사운드
    public AudioSource itemSound;
    public AudioSource hitFloorSound;
    public AudioSource coinSound;

    //충돌 관련 오브젝트
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

        //터치가 되었을 경우 아이템은 비 활성화 상태로 변경
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
