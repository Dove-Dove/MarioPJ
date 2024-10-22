using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 10f; // 초기 속도
    public float returnTime = 0.5f; // 일정 시간이 지나면 되돌아옴
    private Vector2 targetPosition; // 목표 지점
    private Vector2 startPosition; // 시작 위치

    private float minDistance = 5f; //부메랑 최소 거리
    private float maxY = 10f; //부메랑 위아래 각도 제한

    private bool isReturning = false; // 되돌아오고 있는지 여부
    private Transform origin; // 부메랑을 던진 적의 위치

    private Boo shooter;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        origin = GameObject.Find("Boo").transform; // 부메랑을 던진 적의 위치

        // 부메랑이 처음에 목표할 위치를 플레이어로 설정
        //targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        targetPosition = shooter.ThrowDistance.position;

        // 부메랑 위아래 범위제한
        targetPosition.y = Mathf.Clamp(targetPosition.y, startPosition.y - maxY, startPosition.y + maxY);

        // 최소 이동거리만큼 이동
        float travelTime = Mathf.Max(returnTime, minDistance / speed);

        // 일정 시간이 지나면 되돌아오도록
        Invoke("ReturnToOrigin", travelTime);

        // 3초 지나면 무조건 파괴
        Invoke("DestroyBoomerang", 4f);
    }

    void Update()
    {
        if (!isReturning)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 direction2 = new Vector2(direction.x, 0);
            rb.velocity = direction2 * speed;
        }
        else
        {
            // 되돌아오는 중일 때
            Vector2 returnDirection = ((Vector2)origin.position - (Vector2)transform.position).normalized;
            Vector2 direction2 = new Vector2(returnDirection.x, 0);
            rb.velocity = direction2 * speed;
        }
    }

    public void SetShooter(Boo shooter)
    {
        this.shooter = shooter;
    }


    void ReturnToOrigin()
    {
        if(Vector2.Distance(startPosition,transform.position) >= minDistance)
        {
            isReturning = true;
        }
        else
        {
            Invoke("ReturnToOrigin", 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player hit by boomerang!");
        }

        if (collision.gameObject.name.Contains("Boo") && isReturning)
        {
            DestroyBoomerang();
        }
    }

    void DestroyBoomerang()
    {
        shooter.DecreaseBoomerangCount();
        Destroy(gameObject);
    }
}
