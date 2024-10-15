using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //받아올 game object,  이미지
    public GameObject gameManager;
    public Sprite[] num;
    public Sprite[] PowerImg;
    public GameObject[] Power;
    public GameObject[] Money;
    public GameObject[] Life;
    public GameObject[] Point;
    public GameObject[] Time;

    public float nowPower = 0;
    private int nowCoin = 0;
    private int nowLife = 3;
    private int nowPoint = 0;
    private int nowTime = 0;
    private int numberCount = 7;

    private int[] numPrint = new int[7];



    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        nowPower = gameManager.GetComponentInChildren<GameManager>().runingTime;
        nowPoint = gameManager.GetComponentInChildren<GameManager>().point;
        ShowPower(nowPower);
        ShowPoint(nowPoint);
    }



    void ShowPower(float now)
    {
        //일단 보류
        now *= 2;

        for(int i =0; i < Power.Length; i++)
        {
            if(now > i)
            {
                if( i == Power.Length-1 && now > 3)
                    Power[i].GetComponent<Image>().sprite = PowerImg[3];
                else
                    Power[i].GetComponent<Image>().sprite = PowerImg[1];
            }
                
            else
            {
                if (i == Power.Length-1 && now < 3)
                    Power[i].GetComponent<Image>().sprite = PowerImg[2];
                else if(i != Power.Length - 1)
                    Power[i].GetComponent<Image>().sprite = PowerImg[0];
            }
               
        }

    }

    void ShowCoin()
    {

    }

    void ShowLife()
    {

    }

    void ShowPoint(int nowp)
    {
        number(nowp);
        int count = 0;
        for(int i = 6; i>= 0;--i)
        {
            //Point[i].GetComponent<Image>().sprite = num[numPrint[count]];
            switch(numPrint[count])
            {
                case 0:
                    Point[i].GetComponent<Image>().sprite = num[0];
                    break;
                case 1:
                    Point[i].GetComponent<Image>().sprite = num[1];
                    break;
                case 2:
                    Point[i].GetComponent<Image>().sprite = num[2];
                    break;
                case 3:
                    Point[i].GetComponent<Image>().sprite = num[3];
                    break;
                case 4:
                    Point[i].GetComponent<Image>().sprite = num[4];
                    break;
                case 5:
                    Point[i].GetComponent<Image>().sprite = num[5];
                    break;
                case 6:
                    Point[i].GetComponent<Image>().sprite = num[6];
                    break;
                case 7:
                    Point[i].GetComponent<Image>().sprite = num[7];
                    break;
                case 8:
                    Point[i].GetComponent<Image>().sprite = num[8];
                    break;
                case 9:
                    Point[i].GetComponent<Image>().sprite = num[9];
                    break;
                default:
                    Point[i].GetComponent<Image>().sprite = num[0];
                    break;
            }

            count++;
        }    
    }

    void ShowTime()
    {

    }

    void number(int num)
    {
        int count = 0;
        while (num > 0)
        {
            int s = num % 10;
            numPrint[count] = s;
            count++;
            num /= 10;
        }

    }
}
