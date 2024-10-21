using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    public Transform StartPos;
    public Transform map1Pos;
    public Transform map2Pos;
    public Transform map3Pos;
    public Transform map4Pos;
    public Transform mapBossPos;

    public GameObject Player;
    


    public int clearStage; //클리어 스테이지
    public int currentStage = 0; //현재 스테이지
    public bool onRoad; //길목


    void Start()
    {
        Player.transform.position = StartPos.position;
        clearStage = 0;
    }

    void Update()
    {
        Debug.Log("currentStage = " + currentStage);
        Debug.Log("OnRoad: " + onRoad);
        switch (currentStage)
        {
            case 0:
                if (Input.GetKey(KeyCode.RightArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(map1Pos.position.x, StartPos.position.y);
                    onRoad = true;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && onRoad)
                {
                    Player.transform.position = StartPos.position;
                    onRoad = false;
                }
                else if (Input.GetKey(KeyCode.UpArrow) && onRoad)
                {
                    Player.transform.position = map1Pos.position;
                    currentStage = 1;
                    onRoad = false;
                }
                break;
             case 1:
                if (Input.GetKey(KeyCode.DownArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(map1Pos.position.x, StartPos.position.y);
                    currentStage = 0;
                    onRoad = true;
                }
                else if(Input.GetKey(KeyCode.Z) && !onRoad)
                {
                    //SceneManager.CreateScene("Map1-1"); 씬전환
                    //클리어했다면
                    clearStage = 1;
                }
                if (clearStage >= 1)
                {
                    if(Input.GetKey(KeyCode.RightArrow) && !onRoad)
                    {
                        Player.transform.position = new Vector2(mapBossPos.position.x, map1Pos.position.y);
                        onRoad = true;
                    }
                    else if(Input.GetKey(KeyCode.LeftArrow) && onRoad)
                    {
                        Player.transform.position = map1Pos.position;
                        onRoad = false;
                    }
                    else if(Input.GetKey(KeyCode.RightArrow) && onRoad)
                    {
                        Player.transform.position = map2Pos.position;
                        currentStage = 2;
                        onRoad = false;
                    }
                }
                break;

            case 2:
                if(Input.GetKey(KeyCode.LeftArrow) && !onRoad)
                {
                    Player.transform.position = map1Pos.position;
                    currentStage = 1;
                    onRoad = false;
                }
                else if(Input.GetKey(KeyCode.Z) && !onRoad)
                {
                    //SceneManager.CreateScene("Map1-2"); 씬전환
                    //클리어했다면
                    clearStage = 2;
                }
                if(clearStage >= 2)
                {

                }
                break;

            case 3:
                {

                }
                break;

            case 4:
                {

                }
                break;

            case 5:
                {

                }
                break;
        }        

    }
}
