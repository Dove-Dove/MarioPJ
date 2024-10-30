using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class followplayer : MonoBehaviour
{
    public Transform playerTrans;
    public GameObject player;
    private float deltaTime = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (player.transform)
        {
            playerTrans = player.transform;
        }

        Vector2 playerPos = new Vector2(playerTrans.position.x, playerTrans.position.y);
        transform.SetPositionAndRotation(playerPos, transform.rotation);

    }


    void OnGUI()
    {
        float fps = 1.0f / deltaTime;
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;
        string text = $"FPS: {Mathf.Ceil(fps)}";
        GUI.Label(rect, text, style);
    }
}
