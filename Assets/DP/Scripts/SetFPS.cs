using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFPS : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60; // FPS�� 60���� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
