using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.Arm;

public class UIManager : MonoBehaviour
{
    //받아올 game object,  이미지
    public GameObject gameManager;
    public Sprite[] num;
    public Sprite[] PowerImg;
    public Sprite[] BounsImg;
    public GameObject[] Power;
    public GameObject[] Money;
    public GameObject[] Life;
    public GameObject[] Point;
    public GameObject[] Time;
    public GameObject[] Bouns;

    private float nowPower = 0;
    private bool lastPower = false;
    private int nowCoin = 0;
    private int nowLife = 3;
    private int nowPoint = 0;
    private int nowTime = 0;
 
    //배열 (텍스트를 따로 못써서 이미지를 사용)
    private int[] arrCoin = new int[2];
    private int[] arrLife = new int[2];
    private int[] arrPoint = new int[7];
    private int[] arrTims = new int[3];
    private int[] arrBouns;



    // Update is called once per frame
    void Update()
    {
        nowPower = gameManager.GetComponentInChildren<GameManager>().runingTime;
        nowCoin = gameManager.GetComponentInChildren<GameManager>().coin;
        nowLife = gameManager.GetComponentInChildren<GameManager>().PlayerLife;
        nowPoint = gameManager.GetComponentInChildren<GameManager>().point;
        nowTime = gameManager.GetComponentInChildren<GameManager>().UITime;
        arrBouns = gameManager.GetComponentInChildren<GameManager>().BounsItem;
        ShowPower(nowPower);
        ShowCoin(nowCoin);
        ShowLife(nowLife);
        ShowPoint(nowPoint);
        ShowTime(nowTime);
        BounsItemPrint();
    }



    void ShowPower(float now)
    {
        //일단 보류
        now *= 2;

        for(int i =0; i < Power.Length; i++)
        {
            if(now > i)
            {
                if (i == Power.Length - 1 && now > 3)
                    lastPower = !lastPower;
                else
                    Power[i].GetComponent<Image>().sprite = PowerImg[1];
            }
                
            else
            {
                if (i == Power.Length-1 && now < 3)
                    lastPower =false;
                else if(i != Power.Length - 1)
                    Power[i].GetComponent<Image>().sprite = PowerImg[0];
            }

            if (i == Power.Length - 1 && lastPower )
            {
                Power[i].GetComponent<Image>().sprite = PowerImg[3];
            }
            else if((i == Power.Length - 1 && !lastPower))
                Power[i].GetComponent<Image>().sprite = PowerImg[2];

        }

    }

    void ShowCoin(int nCoin)
    {
        number(nCoin, arrCoin, arrCoin.Length);
        int count = 0;
        for (int i = 1; i >= 0; --i)
        {
            if (i == 0 && arrCoin[count] == 0)
                Money[0].SetActive(false);
            else
                Money[0].SetActive(true);               
            Money[i].GetComponent<Image>().sprite = num[arrCoin[count]];
               
                
            count++;
        }
    }

    void ShowLife(int nLife)
    {
        number(nLife, arrLife, arrLife.Length);
        int count = 0;
        for (int i = 1; i >= 0; --i)
        {
            
            Life[i].GetComponent<Image>().sprite = num[arrLife[count]];
            count++;
        }
    }

    void ShowPoint(int nowp)
    {
        number(nowp, arrPoint, arrPoint.Length);
        int count = 0;
        for(int i = 6; i>= 0;--i)
        {
            Point[i].GetComponent<Image>().sprite = num[arrPoint[count]];
            count++;
        }    
    }

    void ShowTime(int ntime)
    {
        number(ntime, arrTims, arrTims.Length);
        int count = 0;
        for (int i = 2; i >= 0; --i)
        {
            Time[i].GetComponent<Image>().sprite = num[arrTims[count]];
            count++;
        }
    }

    void number(int num, int[] arr, int arrCoint)
    {
        int count = 0;
        while (arrCoint > 0)
        {
            int s = num % 10;
            arr[count] = s;
            count++;
            num /= 10;
            arrCoint--;
        }
    }
    
    void BounsItemPrint()
    {
        int num = arrBouns.Length;
        int count = 0;
        while (count != num)
        {
            if(arrBouns[count] == 0)
                Bouns[count].SetActive(false);
            else
            {

                Bouns[count].SetActive(true);
                Bouns[count].GetComponent<Image>().sprite = BounsImg[arrBouns[count] - 1]; 
            }
            count++;
        }

    }
}
