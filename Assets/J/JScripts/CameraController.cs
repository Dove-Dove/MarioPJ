using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform player;
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] Vector2 minCameraBoundary;
    [SerializeField] Vector2 maxCameraBoundary;

    private float MapAnchor;
    private Vector2 CameraPos;
    private bool playerdead = false;

    public bool pipeCam = false;

    public bool moveMap = false;
    //
    private bool moveCam = false;

    public float moveSpeed = 1.5f;
    private float MoveStop = 39.94f;
    Vector3 targetPos;
    // Update is called once per frame
    private void Start()
    {
        CameraPos = transform.position;
    }

    void Update()
    {
        if (playerdead)
            return;

        MapAnchor = GameObject.Find("MapManager").GetComponent<mapManager>().transform.position.y;


       if (moveMap)
        {
            if (MoveStop >= transform.position.x)
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            else
                moveSpeed = 0;


        }

        else if (pipeCam)
        {
            targetPos = new Vector3(player.position.x, player.position.y + 1.5f, transform.position.z);
            transform.position = targetPos;
        }




        else
        {
            if (MapAnchor <= player.position.y)
            {
                targetPos = new Vector3(player.position.x,
                    player.position.y
                    , transform.position.z);
                
            }


            else
                targetPos = new Vector3(player.position.x,
                    CameraPos.y,
                    transform.position.z);

            transform.position = targetPos;

        }

        // 카메라 위치 제한
        if(!pipeCam)
        {
            transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minCameraBoundary.x, maxCameraBoundary.x),
            Mathf.Clamp(transform.position.y, minCameraBoundary.y, maxCameraBoundary.y),
            transform.position.z
);
        }



    }

    public void deadCam()
    {
        playerdead= true;
    }

    public void inPipe()
    {
        pipeCam = !pipeCam;
        moveMap= false;
        gameObject.transform.position = new Vector3(40f, 0, transform.position.z);
    }


}
