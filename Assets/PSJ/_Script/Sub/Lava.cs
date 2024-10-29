using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    //7°³

    private bool isJumping;

    protected float jumpForce = 12f;
    protected float jumpInterveal = 4f;
    protected float nextJumpTime;

    protected bool movingup = true;

    protected Transform player;
    public float range = 10f;


    private Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nextJumpTime = Time.time + jumpInterveal;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(player != null)
        {
            if (Vector2.Distance(transform.position, player.position) < range)
            {
                if (Time.time >= nextJumpTime && !isJumping)
                {
                    Jump();
                    nextJumpTime = Time.time + jumpInterveal;
                    isJumping = false;
                    Invoke("setAnim", 1.0f);
                }
            }
        }
    }

    void Jump()
    {
        animator.SetBool("MoveUp", true);

        isJumping = true;
        gameObject.GetComponent<Rigidbody2D>().velocity
            = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, jumpForce);
    }

    void setAnim()
    {
        animator.SetBool("MoveUp", false);
    }
}
