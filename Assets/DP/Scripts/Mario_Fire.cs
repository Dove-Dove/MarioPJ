using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioFire : MonoBehaviour
{
    public float lifeTime = 3.0f;
    public GameObject player;
    private Rigidbody2D rigid;
    private BoxCollider2D hitbox;
    private Player_Move playerCom;
    private Sprite sprite;
    private Animator animator;

    private bool isGround=false;
    private bool isRight;

    public AudioSource shootFireSound;
    public AudioSource distroyFireSound;
    public AudioSource hitEnemySound;

    //Ground 바운스, 적 출동시 제거

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        sprite = GetComponent<Sprite>();
        animator = GetComponent<Animator>();

        player = GameObject.Find("Mario");
        playerCom = player.GetComponent<Player_Move>();
        shootFireSound.Play();

        isRight = playerCom.isRight;
        Invoke("destroy", lifeTime);
        Debug.Log(isRight);
    }

    private void Update()
    {
        onGround();
        FireBounce();
    }

    void destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Box")
        {
            //부딫힌게 바닦이 아니라면
            if(!isGround)
            { 
                distroyFireSound.Play();
                animator.Play("SmokeEffect");
                rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                Invoke("destroy", 0.5f);
                //destroy();
            }
        }

        if(collision.collider.tag =="Enemy")
        {
            Debug.Log("불꽃 충돌");
            hitEnemySound.Play();
            
            destroy();
        }
        

    }

    void FireBounce()
    {
        //오른쪽으로 갈 때 
        if(isRight && isGround)
        {
            //rigid.AddForce(new Vector2(1, 10), ForceMode2D.Impulse);
            rigid.velocity = new Vector2(5,5);
        }

        if (!isRight && isGround) 
        {
            //rigid.AddForce(new Vector2(-1, 10), ForceMode2D.Impulse);
            rigid.velocity = new Vector2(-5, 5);
        }
    }

    void onGround()
    {
        Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0), 0.4f);
        RaycastHit2D groundHit = Physics2D.Raycast(rigid.position, Vector2.down, 0.4f, LayerMask.GetMask("Ground"));
        if (groundHit.collider != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}
