using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialTest : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Color originalColor;
    public Color changeColor;
    public Material originalMate;
    public Material mate;
    [SerializeField]
    private float invisibleTimeCount = 0;
    private float cutTimeCount = 0;
    [SerializeField]
    private int invisibleCount = 0;
    bool effectOn=true;
    public Color Color1;
    public Color Color2;
    public Color Color3;


    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor =sprite.material.color;
        changeColor = new Color(mate.color.r, mate.color.g, mate.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (effectOn)
        {
            //marioInvisibleEffect();
            ChangeFireMario();
        }
    }

    public void marioInvisibleEffect()
    {
 

        if (invisibleTimeCount > 7)
        {
            Debug.Log("¹«Àû³¡");
            StopCoroutine("Blink");
            GetComponent<SpriteRenderer>().material.color = originalColor;
            GetComponent<SpriteRenderer>().material = originalMate;
            invisibleTimeCount = 0;
            effectOn = false;
        }
        else
        {
            invisibleTimeCount += Time.unscaledDeltaTime;
            cutTimeCount += Time.unscaledDeltaTime;
            if(cutTimeCount > 0.1f)
            { 
                StartCoroutine(InvincibilityEffect());
                invisibleCount++;
                cutTimeCount = 0;
            }
           
        }

    }

    private IEnumerator InvincibilityEffect()
    {
        int num = invisibleCount;

        if (0 == num % 2)
        {
            GetComponent<SpriteRenderer>().material = mate;
            GetComponent<SpriteRenderer>().material.color = changeColor;
        }

        else
        {
            GetComponent<SpriteRenderer>().material = originalMate;
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }

        yield return new WaitForSecondsRealtime(0.1f);
    }

    public void ChangeFireMario()
    {


        if (invisibleTimeCount > 3)
        {
            StopCoroutine("Blink");
            GetComponent<SpriteRenderer>().material.color = originalColor;
            invisibleTimeCount = 0;
            effectOn = false;
        }
        else
        {
            invisibleTimeCount += Time.unscaledDeltaTime;
            cutTimeCount += Time.unscaledDeltaTime;
            if (cutTimeCount > 0.05f)
            {
                StartCoroutine(ChangeFireMarioEffect());
                invisibleCount++;
                cutTimeCount = 0;
            }

        }

    }

    private IEnumerator ChangeFireMarioEffect()
    {
        int num = invisibleCount;

        if (0 == num % 4)
        {
            GetComponent<SpriteRenderer>().material.color = Color1;
        }
        else if (1 == num % 4)
        {
            GetComponent<SpriteRenderer>().material.color = Color2;
        }
        else if (2 == num % 4)
        {
            GetComponent<SpriteRenderer>().material.color = Color3;
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }

        yield return new WaitForSecondsRealtime(0.1f);
    }

}
