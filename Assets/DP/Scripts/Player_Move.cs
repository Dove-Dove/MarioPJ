using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    //������Ʈ
    public BoxCollider2D hitBox;
    public Rigidbody2D rigid;
    public Animator animator;

    public float MaxSpeed;

    public float LMrio_Jump_pow;
    public float BMrio_Jump_pow;

    //������ Ȯ�ο�
    public bool isBigMario;
    public bool isLittleMario;
    //������ ���� Ȯ�ο�
    public bool isFireMario;
    public bool isRaccoonMario;
    //
    [SerializeField]
    private int marioHp;
    [SerializeField]
    private bool isLookRight;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
