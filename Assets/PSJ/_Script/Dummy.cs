using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    //2개
    public Transform centerObject;  // 기준이 되는 오브젝트
    public float orbitSpeed = 0.01f;   // 공전 속도
    public float orbitRadius = 2f;  // 공전 반지름

    private float angle;            // 현재 각도

    void Start()
    {
        // 오브젝트의 초기 위치를 반지름만큼 떨어뜨려 설정
        angle = 0f;
        Vector3 startPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
        transform.position = centerObject.position + startPos;
    }

    void Update()
    {
        // 각도 증가 (시간에 따른 궤도 이동)
        angle += orbitSpeed * Time.deltaTime;

        // X, Y 좌표를 원 궤도 상의 위치로 계산 (Z축은 고정)
        float x = Mathf.Cos(angle) * orbitRadius;
        float y = Mathf.Sin(angle) * orbitRadius;

        // 계산된 좌표를 기준 오브젝트의 위치에 더해줌
        transform.position = new Vector3(centerObject.position.x + x, centerObject.position.y + y, 0);
    }
}
