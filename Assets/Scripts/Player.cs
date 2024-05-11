using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private Transform overlapCirclePos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask isLayer;

    [SerializeField] private float AttackcoolTime = 0.5f;
    [SerializeField] private float AttackcurTime;

    private float attackBuffer;
    private int attackIndex;
    private bool isAttack;

    private Rigidbody2D rigid;
    [SerializeField] private Animator anim;


    bool isGround;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        Attack();

    }
    void Move() // 움직이는 함수
    {
      
        float x;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) x = Input.GetAxis("Horizontal");
        else x = 0;

        if (!isAttack) rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);


        if (Mathf.Abs(rigid.velocity.x) > 0.1f)
        {
            if (rigid.velocity.x < 0) transform.localScale = new Vector3(-1f, 1f, 1f);
            if (rigid.velocity.x > 0) transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (Mathf.Abs(rigid.velocity.x) > 0) anim.SetBool("isRun", true);
        else anim.SetBool("isRun", false);
       
    }
    void Jump()
    {
        isGround = Physics2D.OverlapCircle(overlapCirclePos.position, checkRadius, isLayer);
        if (isGround)
        {
            anim.SetBool("isFall", false);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rigid.velocity = Vector2.up * jumpPower;
                anim.SetTrigger("isJump");
            }
        }
        if (!isAttack)
            if (rigid.velocity.y < 0) anim.SetBool("isFall", true);

    }
    void Attack()
    {
        attackBuffer -= Time.deltaTime;
        if (attackBuffer <= 0) attackIndex = 1;
        if (Input.GetKeyDown(KeyCode.X))
            attackBuffer = 0.329f;

        if (AttackcurTime <= 0)
        {
            if (attackBuffer > 0)
            {
                switch (attackIndex)
                {
                    case 1:
                        AttackcurTime = AttackcoolTime;
                        StartCoroutine(IsAttacking(AttackcoolTime));
                        break;
                    case 2:
                        AttackcurTime = AttackcoolTime;
                        StartCoroutine(IsAttacking(AttackcoolTime));
                        break;
               
                }
                anim.SetTrigger("isAttack" + attackIndex.ToString());
                

                attackIndex++;
                if (attackIndex >= 3) attackIndex = 1;
            }
        }
        else AttackcurTime -= Time.deltaTime;

    }
    IEnumerator IsAttacking(float attackTime) // 공격중임 나타내는 bool값
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;

    }
}
