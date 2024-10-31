using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    public GameObject map1Obj;
    public GameObject map2Obj;
    public GameObject map3Obj;
    public GameObject map4Obj;
    public GameObject mapBossObj;


    public Transform StartPos;
    private Transform map1Pos;
    private Transform map2Pos;
    private Transform map3Pos;
    private Transform map4Pos;
    private Transform mapBossPos;

    public GameObject Player;

    //----
    private GameManager gameManager;
    //==--

    private float moveSpeed = 3f;

    public int clearStage; //클리어 스테이지
    public int currentStage = 0; //현재 스테이지
    public bool onRoad; //길목
    public bool itemStage;

    void Awake()
    {
        map1Pos = map1Obj.transform;
        map2Pos = map2Obj.transform;
        map3Pos = map3Obj.transform;
        map4Pos = map4Obj.transform;
        mapBossPos = mapBossObj.transform;
    }

    void Start()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        gameManager = GameManager.Instance;

        clearStage = gameManager.GameClearStage;
        currentStage = gameManager.GameCurrentStage;

        switch(currentStage)
        {
            case 0:
                Player.transform.position = StartPos.position;
                break;
            case 1:
                Player.transform.position = map1Pos.position;
                break;
            case 2:
                Player.transform.position = map2Pos.position;
                break;
            case 3:
                Player.transform.position = map3Pos.position;
                break;
            case 4:
                Player.transform.position = map4Pos.position;
                break;
            case 5:
                Player.transform.position = mapBossPos.position;
                break;
        }

        gameManager.orderMusicStart();
        //clearStage = 0;
    }

    void Update()
    {
        if(clearStage >= 1)
        {
            map1Obj.GetComponent<SpriteRenderer>().enabled = true;
            if(clearStage>=2)
                map2Obj.GetComponent<SpriteRenderer>().enabled = true;
            if(clearStage>=3)
                map3Obj.GetComponent<SpriteRenderer>().enabled = true;
            if(clearStage>=4)
                map4Obj.GetComponent<SpriteRenderer>().enabled = true;
            if(clearStage>=5)
                mapBossObj.GetComponent<SpriteRenderer>().enabled = true;

        }
        //Debug.Log("currentStage = " + currentStage);
        //Debug.Log("OnRoad: " + onRoad);
        switch (currentStage)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.RightArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(map1Pos.position.x, StartPos.position.y);
                    onRoad = true;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && onRoad)
                {
                    Player.transform.position = StartPos.position;
                    onRoad = false;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && onRoad)
                {
                    Player.transform.position = map1Pos.position;
                    currentStage = 1;
                    onRoad = false;
                }
                break;
             case 1:
                if (Input.GetKeyDown(KeyCode.DownArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(map1Pos.position.x, StartPos.position.y);
                    currentStage = 0;
                    onRoad = true;
                }
                else if(Input.GetKeyDown(KeyCode.Z) && !onRoad)
                {
                    SceneManager.LoadScene("Map1-1");
                }
                if (clearStage >= 1)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) && !onRoad)
                    {
                        Player.transform.position = new Vector2(mapBossPos.position.x, map1Pos.position.y);
                        onRoad = true;
                    }
                    else if(Input.GetKeyDown(KeyCode.LeftArrow) && onRoad)
                    {
                        Player.transform.position = map1Pos.position;
                        onRoad = false;
                    }
                    else if(Input.GetKeyDown(KeyCode.RightArrow) && onRoad)
                    {
                        Player.transform.position = map2Pos.position;
                        currentStage = 2;
                        onRoad = false;
                    }
                }
                break;

            case 2:
                if(Input.GetKeyDown(KeyCode.LeftArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(mapBossPos.position.x, map1Pos.position.y);
                    onRoad = true;
                }
                else if(Input.GetKeyDown(KeyCode.LeftArrow) && onRoad && !itemStage)
                {
                    Player.transform.position = map1Pos.position;
                    currentStage = 1;
                    onRoad = false;
                }
                else if(Input.GetKeyDown(KeyCode.Z) && !onRoad)
                {
                    SceneManager.LoadScene("Map1-2");
                    clearStage = 2;
                }
                if (clearStage >= 2)
                {
                    if(Input.GetKeyDown(KeyCode.RightArrow) && !onRoad)
                    {
                        Player.transform.position = map3Pos.position;
                        currentStage = 3;
                    }
                    else if(Input.GetKeyDown(KeyCode.DownArrow) && !onRoad)
                    {
                        Player.transform.position = new Vector2(Player.transform.position.x, map4Pos.position.y);
                        onRoad = true;
                    }
                    else if(Input.GetKeyDown(KeyCode.UpArrow) && onRoad && !itemStage)
                    {
                        Player.transform.position = map2Pos.position;
                        currentStage = 2;
                        onRoad = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow) && onRoad && itemStage)
                    {
                        Player.transform.position = new Vector2(Player.transform.position.x, map4Pos.position.y);
                        onRoad = true;
                        itemStage = false;
                    }
                    else if(Input.GetKeyDown(KeyCode.RightArrow) && onRoad && !itemStage)
                    {
                        Player.transform.position = map4Pos.position;
                        currentStage = 4;
                        onRoad = false;
                    }
                    else if(Input.GetKeyDown(KeyCode.DownArrow) && onRoad && clearStage >= 4)
                    {
                        Player.transform.position = new Vector2(Player.transform.position.x, mapBossPos.position.y);
                        onRoad = true;
                        itemStage = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) && onRoad && itemStage)
                    {
                        Player.transform.position = mapBossPos.position;
                        currentStage = 5;
                        onRoad = false;
                    }

                }
                break;

            case 3:
                if (Input.GetKeyDown(KeyCode.LeftArrow) && !onRoad)
                {
                    Player.transform.position = map2Pos.position;
                    currentStage = 2;
                    onRoad = false;
                }
                else if (Input.GetKeyDown(KeyCode.Z) && !onRoad)
                {
                    SceneManager.LoadScene("Map1-3");
                    clearStage = 3;
                }
                break;

            case 4:
                if(Input.GetKeyDown(KeyCode.LeftArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(map2Pos.position.x, Player.transform.position.y);
                    onRoad = true;
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow) && onRoad && !itemStage)
                {
                    Player.transform.position = map2Pos.position;
                    currentStage = 2;
                    onRoad = false;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && onRoad && itemStage)
                {
                    Player.transform.position = new Vector2(Player.transform.position.x, map4Pos.position.y);
                    onRoad = true;
                    itemStage = false;
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow) && onRoad && !itemStage)
                {
                    Player.transform.position = map4Pos.position;
                    currentStage = 4;
                    onRoad = false;
                }
                else if(Input.GetKeyDown(KeyCode.Z) && !onRoad && clearStage >= 3)
                {
                    SceneManager.LoadScene("Map1-4");
                    clearStage = 4;
                }
                if(clearStage >= 4)
                {
                    if(Input.GetKeyDown(KeyCode.DownArrow) && onRoad)
                    {
                        Player.transform.position = new Vector2(Player.transform.position.x, mapBossPos.position.y);
                        itemStage = true;
                        onRoad = true;
                    }
                    if(Input.GetKeyDown(KeyCode.LeftArrow) && onRoad && itemStage)
                    {
                        Player.transform.position = mapBossPos.position;
                        currentStage = 5;
                        itemStage = false;
                        onRoad = false;
                    }
                }
                break;

            case 5:
                if(Input.GetKeyDown(KeyCode.RightArrow) && !onRoad)
                {
                    Player.transform.position = new Vector2(map2Pos.position.x,Player.transform.position.y);
                    onRoad = true;
                    itemStage = true;
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow) && onRoad && !itemStage)
                {
                    Player.transform.position = map4Pos.position;
                    onRoad = false;
                    currentStage = 4;
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow) && onRoad && itemStage)
                {
                    Player.transform.position = new Vector2(Player.transform.position.x,map4Pos.position.y);
                    onRoad = true;
                    itemStage = false;
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow) && onRoad && !itemStage)
                {
                    Player.transform.position = map2Pos.position;
                    onRoad = false;
                    currentStage = 2;
                }
                else if(Input.GetKeyDown(KeyCode.LeftArrow) && onRoad && itemStage)
                {
                    Player.transform.position = mapBossPos.position;
                    onRoad = false;
                    itemStage = false;
                }
                else if(Input.GetKeyDown(KeyCode.DownArrow) && onRoad && !itemStage)
                {
                    Player.transform.position = new Vector2(Player.transform.position.x, mapBossPos.position.y);
                    onRoad = true;
                    itemStage = true;
                }
                else if(Input.GetKeyDown(KeyCode.Z) && !onRoad && !itemStage)
                {
                    SceneManager.LoadScene("MapBoss");
                    clearStage = 5;
                }
                break;
        }        

    }
}
