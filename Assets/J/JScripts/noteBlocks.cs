using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class noteBlocks : MonoBehaviour
{
    //���� ������Ʈ 
    public GameObject UpObj;
    public GameObject WellObj;
    public GameObject DownObj;
    public GameObject Items;
    

    //���� 
    public AudioSource hitBlockSound;

    //������ ���� Ȯ��
    public bool isItems = false;

    //������ �浹���� �Ʒ����� �浹 Ȯ��
    private bool jumping = false;
    private bool Down = false;
    //������ �浹�� �ö󰡰� �ϴ� ��
    private bool DownAUp = false;

    // ���� �� (��, �Ʒ� , ����ġ )
    private Vector2 upBlock;
    private Vector2 downBlock;
    private Vector2 BlockPos;

    //��Ÿ ����
    //���尡 ������ ������ ���� �����ϴ� ����
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
        //�ؿ��� ����
        if (DownObj.GetComponent<underCol>().WallUnderOpen
            || WellObj.GetComponent<BlockWall>().WallOpen)
            jumping = true;
        //������ ������ 
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
