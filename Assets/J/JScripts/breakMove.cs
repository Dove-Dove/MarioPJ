using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class breakMove : MonoBehaviour
{
    [SerializeField] private AnimationCurve x_animationCurve;
    [SerializeField] private AnimationCurve y_animationCurve;

    private float curTime;
    [SerializeField] private float period = 2f;


    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= period)
        {
            curTime -= curTime;
        }


        float xValue = x_animationCurve.Evaluate(curTime)  ; 
        float yValue = y_animationCurve.Evaluate(curTime)   ;

        transform.position = new Vector3(xValue, yValue, 0);
    }
}
