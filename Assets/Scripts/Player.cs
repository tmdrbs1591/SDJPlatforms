using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float attackcoolTime = 0.5f;
    [SerializeField] private float slidingSpeed;
   
    [Space]
    [SerializeField] private Transform overlapCirclePos;//�÷��̾� �ٴ� ���� ��ġ
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask isLayer;

    [SerializeField] private Transform wallChk;
    [SerializeField] private float wallchkDistance;
    [SerializeField] private LayerMask w_Layer;




    private float AttackcurTime;
    private float attackBuffer;
    private int attackIndex;
    private bool isAttack;
    private bool isWall;
    [SerializeField] private bool isFall;
    private float isRight;

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
        Wall();

    }
    void Wall() // ��Ÿ��
    {
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance,w_Layer);
        anim.SetBool("isSliding", isWall);

        if (isWall)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
        }
    }
    void Move() // �����̴� �Լ�
    {
      
        float x;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) x = Input.GetAxis("Horizontal");
        else x = 0;

        if (!isAttack) rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);


        if (Mathf.Abs(rigid.velocity.x) > 0.1f) // �¿����
        {
            transform.localScale = new Vector3(isRight, 1f, 1f);
            if (rigid.velocity.x < 0) isRight = -1f;
            if (rigid.velocity.x > 0) isRight = 1f;
        }
        if (Mathf.Abs(rigid.velocity.x) > 0) anim.SetBool("isRun", true);
        else anim.SetBool("isRun", false);
       
    }
    void Jump() // ����
    {
        isGround = Physics2D.OverlapCircle(overlapCirclePos.position, checkRadius, isLayer);
        isFall = rigid.velocity.y < 0;
        if (isGround)
        {
            anim.SetBool("isFall", isFall);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rigid.velocity = Vector2.up * jumpPower;
                anim.SetTrigger("isJump");
            }
        }
            if (rigid.velocity.y < 0) anim.SetBool("isFall", isFall);


    }
    void Attack() // ����
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
                        AttackcurTime = attackcoolTime;
                        StartCoroutine(IsAttacking(attackcoolTime));
                        break;
                    case 2:
                        AttackcurTime = attackcoolTime;
                        StartCoroutine(IsAttacking(attackcoolTime));
                        break;
               
                }
                anim.SetTrigger("isAttack" + attackIndex.ToString());
                

                attackIndex++;
                if (attackIndex >= 3) attackIndex = 1;
            }
        }
        else AttackcurTime -= Time.deltaTime;

    }
    IEnumerator IsAttacking(float attackTime) // �������� ��Ÿ���� bool��
    {
        isAttack = true;
        yield return new WaitForSeconds(attackTime);
        isAttack = false;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }
}
