using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //받아올 game object,  이미지
    public GameObject GameManager;
    public Sprite[] num;
    public Sprite[] PowerImg;
    public GameObject[] Power;
    public GameObject[] Money;
    public GameObject[] Life;
    public GameObject[] Point;
    public GameObject[] Time;

    public int nowPower = 0;
    private int nowCoin = 0;
    private int nowLife = 3;
    private int nowPoint = 0;
    private int nowTime = 0;
    private int numberCount = 7;



    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ShowPower(nowPower);
    }



    void ShowPower(int now)
    {
        if(now == 0)
            return;
        

        for(int i =0; i <now;i++)
        {
            Power[i].GetComponent<Image>().sprite = PowerImg[1];
        }
    }

    void ShowCoin()
    {

    }

    void ShowLife()
    {

    }

    void ShowPoint()
    {

    }

    void ShowTime()
    {

    }
}
