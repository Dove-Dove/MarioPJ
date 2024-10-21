using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialTest : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Color originalColor;
    [SerializeField]
    private float invisibleTimeCount = 0;
    private float cutTimeCount = 0;
    [SerializeField]
    private int invisibleCount = 0;
    bool effectOn=true;


    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor=sprite.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (effectOn)
        {
        marioInvisibleEffect();
        }
    }

    public void marioInvisibleEffect()
    {
 

        if (invisibleTimeCount > 7)
        {
            Debug.Log("¹«Àû³¡");
            StopCoroutine("Blink");
            GetComponent<SpriteRenderer>().material.color = originalColor;
            invisibleTimeCount = 0;
            effectOn = false;
        }
        else
        {
            invisibleTimeCount += Time.deltaTime;
            cutTimeCount += Time.deltaTime;
            if(cutTimeCount > 0.1f)
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
            GetComponent<SpriteRenderer>().material.color = new Color(255, 155, 56, 255);
            //GetComponent<SpriteRenderer>().material.color = Color.green;
        }
        else if (1 == num % 4)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(155, 255, 233, 255);
        }
        else if (2 == num % 4)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(58, 58, 58, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }


        yield return new WaitForSecondsRealtime(0.1f);

    }
}
