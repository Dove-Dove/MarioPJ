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

    // Update is called once per frame
    private void Start()
    {
        CameraPos = transform.position;
    }

    void Update()
    {

        MapAnchor = GameObject.Find("MapManager").GetComponent<mapManager>().transform.position.y;

        Vector3 targetPos;
        if (MapAnchor <= player.position.y)
            targetPos = new Vector3(player.position.x, player.position.y , transform.position.z);
        else 
            targetPos = new Vector3(player.position.x, CameraPos.y, gameObject.transform.position.z);

        transform.position = targetPos;

    }
}
