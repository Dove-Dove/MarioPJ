using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class TitleSceneAnimContriler : MonoBehaviour
{
    public Image Curtain;
    public GameObject Title;
    public Image Num;
    public Image BackGround;
    public Image Tree1;
    public Image Tree2;
    public GameObject Select1;
    public GameObject Select2;
    public GameObject SelectArrow1;
    public GameObject SelectArrow2;
    public GameObject Bottom;
    

    Image[] TitleGroup;

    public float curtainSpeed = 10;
    public float curtainTime = 0;
    public bool isCurtainStart =false;
    public bool isCurtainEnd = false;

    public bool isBackGroundStart = false;
    public bool isBackGroundEnd = false;
    public float backGroundAlphaSpeed = 0.01f;
    public float backGroundTime = 0;

    public bool isTitleLogoStart=false;
    public bool isTitleLogoEnd=false;
    public float TitleGroupSpeed = 5f;
    public float titleGroupLimitY = 6;
    public float titleGroupCountY = 0;

    private int selectIndex = 0;
    private bool isSelect = false;
    private bool isSelectEnd = false;

    public AudioSource selectMoveSound;
    public AudioSource selectSound;

    void Start()
    {
        TitleGroup = GameObject.Find("Title").GetComponentsInChildren<Image>();
        Num.GetComponent<Animator>().enabled = false;
        Invoke("StartTitleScene", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //커튼업
        CurtainOpen();
        //
        TitleFallDown();
        //
        BrightBackGround();
        //
        BrightSelectMenu();
    }

    //커튼 효과
    void CurtainOpen()
    { 
        if(isCurtainStart)
        {
            if (curtainTime < 1)
            {
                curtainTime += Time.deltaTime;
                Curtain.transform.position = new Vector2(Curtain.transform.position.x, Curtain.transform.position.y + curtainSpeed * Time.deltaTime);
            }
            else 
            { 
                isCurtainStart=false;
                Invoke("CurtainEndFalse",1);
            }
        }
            
    }

    void CurtainEndFalse()
    {
        isCurtainEnd = true;
    }
    //타이틀 효과 y:390->120
    void TitleFallDown()
    {
        if(isCurtainEnd)
        { isTitleLogoStart = true;}
        if (isTitleLogoStart)
        {
            if (titleGroupCountY < 6) 
            { 
                titleGroupCountY +=Time.deltaTime * TitleGroupSpeed;
                Title.transform.position = new Vector2(Title.transform.position.x, Title.transform.position.y - Time.deltaTime * TitleGroupSpeed);
            }
            else
            {
                isTitleLogoStart=false; isTitleLogoEnd = true; Num.GetComponent<Animator>().enabled = true;
            }
        }
    }
    //시작ㄷ
    void StartTitleScene()
    { isCurtainStart = true; }


    //배경 효과
    void BrightBackGround()
    {
        if(isTitleLogoEnd)
        { isBackGroundStart=true;}

        if (isBackGroundStart)
        {
            if (backGroundTime < 2)
            {
                backGroundTime += Time.deltaTime;
                BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, BackGround.color.a + Time.deltaTime * backGroundAlphaSpeed);
                Tree1.color = new Color(Tree1.color.r, Tree1.color.g, Tree1.color.b, Tree1.color.a + Time.deltaTime * backGroundAlphaSpeed);
                Tree2.color = new Color(Tree2.color.r, Tree2.color.g, Tree2.color.b, Tree2.color.a + Time.deltaTime * backGroundAlphaSpeed);
            }
            else { isBackGroundStart = false; isBackGroundEnd = true; }
        }
    }

    void BrightSelectMenu()
    {
        if(isBackGroundEnd)
        {
            isSelect = true;
            Select1.SetActive(true);
            Select2.SetActive(true);
            SelectArrow1.SetActive(true);

        }

        if(isSelect && !isSelectEnd)
        {
            if (selectIndex==0 )
            {
                SelectArrow1.SetActive(true);
                SelectArrow2.SetActive(false);
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selectIndex = 1;
                    selectMoveSound.Play();
                }
            }
            if (selectIndex == 1)
            {
                SelectArrow1.SetActive(false);
                SelectArrow2.SetActive(true);

                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selectIndex = 0;
                    selectMoveSound.Play();
                }
            }

            if(Input.GetKeyDown(KeyCode.X) )
            {
                if(selectIndex==0)
                {
                    for (int i = 0; i < 4;i++)
                    {
                        StartCoroutine(SelecArrowBlink(SelectArrow1));
                    }
                    isSelectEnd = true;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        StartCoroutine(SelecArrowBlink(SelectArrow2));
                    }
                    isSelectEnd = true;
                }
            }

        }
        if (isSelectEnd)
        {
            Tree1.enabled = false;
            Tree2.enabled = false;
            Curtain.enabled = false;
            Title.SetActive(false);
            Select1.SetActive(false);
            Select2.SetActive(false);
            SelectArrow1.SetActive(false);
            SelectArrow2.SetActive(false);
            Num.enabled = false;
            Bottom.SetActive(false);

            Invoke("LordNextScene", 0.1f);
        }
    }
    IEnumerator SelecArrowBlink(GameObject Arrow)
    {
        yield return new WaitForSeconds(0.1f);
        if (Arrow.activeSelf)
        {
            Arrow.SetActive(false);
        }
        else
        {
            Arrow.SetActive(true);
        }
    }

    void LordNextScene()
    {
        SceneManager.LoadScene("SelectScene");
    }
}

