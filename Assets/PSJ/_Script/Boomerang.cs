using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 10f; // �ʱ� �ӵ�
    public float returnTime = 0.5f; // ���� �ð��� ������ �ǵ��ƿ�
    private Vector2 targetPosition; // ��ǥ ����
    private Vector2 startPosition; // ���� ��ġ

    private float minDistance = 3f; //�θ޶� �ּ� �Ÿ�
    private float maxY = 10f; //�θ޶� ���Ʒ� ���� ����

    private bool isReturning = false; // �ǵ��ƿ��� �ִ��� ����
    private Transform origin; // �θ޶��� ���� ���� ��ġ

    private Boo shooter;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        origin = GameObject.FindGameObjectWithTag("Enemy").transform; // �θ޶��� ���� ���� ��ġ

        // �θ޶��� ó���� ��ǥ�� ��ġ�� �÷��̾�� ����
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        // �θ޶� ���Ʒ� ��������
        targetPosition.y = Mathf.Clamp(targetPosition.y, startPosition.y - maxY, startPosition.y + maxY);

        // �ּ� �̵��Ÿ���ŭ �̵�
        float travelTime = Mathf.Max(returnTime, minDistance / speed);

        // ���� �ð��� ������ �ǵ��ƿ�����
        Invoke("ReturnToOrigin", travelTime);
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
            // �ǵ��ƿ��� ���� ��
            Vector2 returnDirection = ((Vector2)origin.position - (Vector2)transform.position).normalized;
            rb.velocity = returnDirection * speed;
        }

        if(shooter == null)
        {
            Destroy(gameObject);
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

        if (collision.CompareTag("Enemy") && isReturning)
        {
            DestroyBoomerang();
        }
    }

    void DestroyBoomerang()
    {
        if(shooter != null)
        {
            shooter.DecreaseBoomerangCount();
            Destroy(gameObject);
        }
    }
}
