using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteBlocks : MonoBehaviour
{
    // Start is called before the first frame update
    public bool jumping = false;
    public bool Down = false;

    private Vector2 upBlock;
    private Vector2 downBlock;
    private Vector2 BlockPos;

    void Start()
    {
        BlockPos = transform.position;
        upBlock = BlockPos + new Vector2(0, 0.5f);
        downBlock = BlockPos + new Vector2(0, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(jumping)
            MoveBlock(upBlock);
        else if(Down)
            MoveBlock(downBlock);
        else
            transform.position = Vector2.MoveTowards(transform.position, BlockPos, Time.deltaTime * 3);

    }

    void MoveBlock(Vector2 pos)
    {
        if (jumping || Down)
        {
            transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * 3);
            if(transform.position.y >= upBlock.y || transform.position.y <= downBlock.y)
                jumping = Down = false;
        }
        
    }
}
