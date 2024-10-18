using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform player;
    [SerializeField] float smoothing = 0.2f;
    [SerializeField] Vector2 minCameraBoundary;
    [SerializeField] Vector2 maxCameraBoundary; 


    // Update is called once per frame
    void FixedUpdate()
   {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y + 3, transform.position.z);
        transform.position =  targetPos;
    }
}
