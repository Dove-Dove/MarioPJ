using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerState : MonoBehaviour
{
    public MarioStatus status;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(status == MarioStatus.SuperMario)
        {
            animator.SetBool("IsSuper", true);
        }
        else if(status == MarioStatus.RaccoonMario) 
        {
            animator.SetBool("IsRaccoon", true);
        }
        else if(status == MarioStatus.FireMario)
        {
            animator.SetBool("IsFire", true);
        }
        else
        {
            animator.SetBool("IsNormal", true);
        }
    }
}
