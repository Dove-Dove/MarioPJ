using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumbaSpawner : MonoBehaviour
{
    public GameObject GumbaPrefab;
    protected GameObject player;

    public Transform spawnPos1;
    public Transform spawnPos2;

    public float nextSpawnTime;
    public float spawnCooldown = 2f;

    protected float range = 15f;

    public LayerMask playerLayer;

    public Vector2 boxSize = new Vector2(1f, 3f);
    public Vector2 boxSize2 = new Vector2(2f, 1f);
    public bool IsClose;


    void Start()
    {
        nextSpawnTime = Time.time + spawnCooldown;
        player = GameObject.Find("Mario");

        IsClose = false;
    }

    void Update()
    {
        Vector2 pipeOrigin = new Vector2(transform.position.x - 2f, transform.position.y - 1f + boxSize.y / 2);

        if (player.transform.position.x < transform.position.x)
            pipeOrigin.x -= 0.5f;
        else
            pipeOrigin.x += 4.5f;

        RaycastHit2D hit = Physics2D.BoxCast(pipeOrigin, boxSize, 0f, Vector2.zero, 0f, playerLayer);

        Vector2 pipeUp = new Vector2(transform.position.x, transform.position.y + 1f + boxSize2.y / 2);
        RaycastHit2D hit2 = Physics2D.BoxCast(pipeUp, boxSize2, 0f, Vector2.zero, 0f, playerLayer);


        if ((hit.collider != null && hit.collider.tag.Contains("Player"))
            || (hit2.collider != null && hit2.collider.tag.Contains("Player")))
            IsClose = true;
        else
            IsClose = false;

        FlowerEnemy.DrawBox(pipeOrigin, boxSize);
        FlowerEnemy.DrawBox(pipeUp, boxSize2);



        if (Vector2.Distance(transform.position, player.transform.position) < range && !IsClose)
        {
            if(Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnCooldown;
            }
        }

    }

    void SpawnEnemy()
    {
        if(player.transform.position.x - gameObject.transform.position.x > 0) //¿À¸¥ÂÊ
        {
            GameObject projectile = Instantiate(GumbaPrefab, spawnPos2.position, Quaternion.identity);
            projectile.GetComponent<Enemy>().movingLeft = false;
        }
    }
}
