using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    //2��
    public Transform centerObject;  // ������ �Ǵ� ������Ʈ
    public float orbitSpeed = 0.01f;   // ���� �ӵ�
    public float orbitRadius = 2f;  // ���� ������

    private float angle;            // ���� ����

    void Start()
    {
        // ������Ʈ�� �ʱ� ��ġ�� ��������ŭ ����߷� ����
        angle = 0f;
        Vector3 startPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
        transform.position = centerObject.position + startPos;
    }

    void Update()
    {
        // ���� ���� (�ð��� ���� �˵� �̵�)
        angle += orbitSpeed * Time.deltaTime;

        // X, Y ��ǥ�� �� �˵� ���� ��ġ�� ��� (Z���� ����)
        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

        // ���� ��ǥ�� ���� ������Ʈ�� ��ġ�� ������
        transform.position = new Vector3(centerObject.position.x + x, centerObject.position.y + y, 0);
    }
}
