using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    //7°³

    private bool isJumping;

    protected float jumpForce = 14f;
    protected float jumpInterveal = 4f;
    protected float nextJumpTime;

    protected bool movingup = true;

    protected Transform player;
    public float range = 10f;

    protected Vector2 originPos;
    bool isGrounded;

    private Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nextJumpTime = Time.time + jumpInterveal;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originPos = transform.position;
    }

    void Update()
    {
        if (gameObject.transform.position.y > originPos.y)
            isGrounded = false;
        else
            isGrounded = true;

        if (isGrounded)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (player != null)
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
