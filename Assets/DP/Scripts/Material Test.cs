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
    [SerializeField]
    private float invisibleCount = 0;
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
            StartCoroutine(StarEffect());
            invisibleCount++;
            
        }

    }

    private IEnumerator StarEffect()
    {
        if (0 == invisibleCount % 3)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(originalColor.r, originalColor.g, 0.5f - originalColor.b, 255);
        }
        else if (1 == invisibleCount % 3)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(102, originalColor.g, 102, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }

        yield return new WaitForSecondsRealtime(0.1f);
    }
}
