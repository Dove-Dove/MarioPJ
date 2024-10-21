using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawn : MonoBehaviour
{
    public GameObject orbitPrefab;   // 공전할 오브젝트 프리팹
    public float orbitRadius = 5f;   // 공전 반지름
    public float spawnRadius = 10f;  // 생성될 초기 위치 반경

    void Start()
    {
        // 공전할 오브젝트 생성
        GameObject orbitObject = Instantiate(orbitPrefab, Vector3.zero, Quaternion.identity);

        // 생성된 오브젝트에 중심 오브젝트와 반지름 설정
        Dummy orbitScript = orbitObject.GetComponent<Dummy>();
        orbitScript.centerObject = transform;
        orbitScript.orbitRadius = orbitRadius;
    }
}
