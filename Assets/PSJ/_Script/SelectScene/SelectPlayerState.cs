using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerState : MonoBehaviour
{
    private GameManager gameManager;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager != null)
        {
            if(gameManager.Player_State == 1)
            {
                animator.SetBool("IsNormal", true);
                animator.SetBool("IsSuper", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsRaccoon", false);

            }
            else if(gameManager.Player_State == 2)
            {
                animator.SetBool("IsSuper", true);
                animator.SetBool("IsNormal", false);
                animator.SetBool("IsFire", false);
                animator.SetBool("IsRaccoon", false);

            }
            else if (gameManager.Player_State == 3)
            {
                animator.SetBool("IsFire", true);
                animator.SetBool("IsNormal", false);
                animator.SetBool("IsSuper", false);
                animator.SetBool("IsRaccoon", false);

            }
            else if (gameManager.Player_State == 4)
            {
                animator.SetBool("IsRaccoon", true);
                animator.SetBool("IsNormal", false);
                animator.SetBool("IsSuper", false);
                animator.SetBool("IsFire", false);
            }
        }
    }
}
