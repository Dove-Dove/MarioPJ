using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class noteBlocks : MonoBehaviour
{
    //게임 오브젝트 
    public GameObject UpObj;
    public GameObject WellObj;
    public GameObject DownObj;
    public GameObject Items;
    

    //사운드 
    public AudioSource hitBlockSound;

    //아이템 유무 확인
    public bool isItems = false;

    //위에서 충돌인지 아래에서 충돌 확인
    private bool jumping = false;
    private bool Down = false;
    //위에서 충돌시 올라가게 하는 값
    private bool DownAUp = false;

    // 벡터 값 (위, 아래 , 원위치 )
    private Vector2 upBlock;
    private Vector2 downBlock;
    private Vector2 BlockPos;

    //기타 변수
    //사운드가 여러번 나오는 것을 방지하는 변수
    int count = 0;



    void Start()
    {
        BlockPos = transform.position;
        upBlock = BlockPos + new Vector2(0, 0.5f);
        downBlock = BlockPos + new Vector2(0, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //밑에서 위로
        if (DownObj.GetComponent<underCol>().WallUnderOpen
            || WellObj.GetComponent<BlockWall>().WallOpen)
            jumping = true;
        //위에서 밑으로 
        else if (UpObj.GetComponent<underCol>().WallUnderOpen)
            Down = true;
      

        if(jumping )
            MoveBlock(upBlock);
        else if(Down && !DownAUp)
            MoveBlock(downBlock);
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, BlockPos, Time.deltaTime * 3);
            if(DownAUp && transform.position.y >= BlockPos.y)
            {
                DownAUp = false;
            }
            if (count == 1 && isItems)
            {
                Items.SetActive(true);
                isItems = false;
            }
            count = 0;
        }
    }

    void MoveBlock(Vector2 pos)
    {

        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * 3);
        
        if(count == 0)
        {
            hitBlockSound.Play();
            count++;

        }

        if ((transform.position.y >= upBlock.y || transform.position.y <= downBlock.y) && !DownAUp)
        {
            jumping = Down = false;
            DownAUp = true;
        }

    }
}
