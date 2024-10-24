using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawn : MonoBehaviour
{
    public GameObject orbitPrefab;   // ������ ������Ʈ ������
    public float orbitRadius = 5f;   // ���� ������
    public float spawnRadius = 10f;  // ������ �ʱ� ��ġ �ݰ�

    void Start()
    {
        // ������ ������Ʈ ����
        GameObject orbitObject = Instantiate(orbitPrefab, Vector3.zero, Quaternion.identity);

        // ������ ������Ʈ�� �߽� ������Ʈ�� ������ ����
        Dummy orbitScript = orbitObject.GetComponent<Dummy>();
        orbitScript.centerObject = transform;
        orbitScript.orbitRadius = orbitRadius;
    }
}
