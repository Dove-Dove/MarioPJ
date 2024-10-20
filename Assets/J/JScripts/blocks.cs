using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BoxType
{
    Items,
    Coin
}

public class blocks : MonoBehaviour
{

    public BoxType boxType = BoxType.Items;
    
    // 오븐 상태 확인
    public bool itemBocksOpen;

    //아이템 프리펩
    public GameObject itemPrefab;

    //코인 오브젝트
    public GameObject coin;

    //애니메이션
    public Animator animator;

    //사운드
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
        if (itemBocksOpen && boxType == BoxType.Items)
        {
            
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
                animator.SetBool("Use", true);
                hitFloorSound.Play();
                itemPrefab.SetActive(true);
                gameObject.GetComponent<blocks>().enabled = false;
            }
        }

        else if(itemBocksOpen && boxType == BoxType.Coin)
        {
            coin.SetActive(true);
            animator.SetBool("Use", true);
            gameObject.GetComponent<blocks>().enabled = false;
        }
        
        
    }
}
