using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    [SerializeField] private float moveSpeed; // �̵��ӵ�
    [SerializeField] private float jumpPower; // ���� �Ŀ�
    [SerializeField] private float walljumpPower; // �� ���� �Ŀ�
    [SerializeField] private float attackcoolTime = 0.5f; // ���� ��Ÿ��
    [SerializeField] private float slidingSpeed; //�� �������� ���ǵ�

    [Header("�÷��̾� �ٴڰ���")]
    [Space]
    [SerializeField] private Transform overlapCirclePos;//�÷��̾� �ٴ� ���� ��ġ
    [SerializeField] private float checkRadius; // �÷��̾� �ٴ� ���� ����
    [SerializeField] private LayerMask isLayer; // �ٴ� ������ ���̾�

    [Header("�÷��̾� ������")]
    [Space]
    [SerializeField] private Transform wallChkPos; // �÷��̾� �� ���� ��ġ
    [SerializeField] private float wallchkDistance; // �÷��̾� �� ���� ����
    [SerializeField] private LayerMask w_Layer; // �� ���� ���̾�



    private float isRight = 1f; // �������� �����ִ��� ����
    private float AttackcurTime;
    private float attackBuffer;

    private int attackIndex;

    private bool isAttack;
    private bool isGround;
    private bool isWall;
    [SerializeField] private bool isWallJump;
    private bool canDoubleJump = true; // 2�� ���� ������ �������� ����
    [SerializeField] private bool isFall;

    private Rigidbody2D rigid;
    private Animator anim;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        transform.localScale = new Vector3(isRight, 1f, 1f);

        if (!isWallJump) { Move(); Flip(); }
        Jump();
        Attack();
        Wall();
    }
    void Wall() // ��Ÿ��
    {
        isWall = Physics2D.Raycast(wallChkPos.position, Vector2.right * isRight, wallchkDistance, w_Layer); // ������ �ȴٸ� true �ƴϸ� false
        anim.SetBool("isSliding", isWall);

        if (Input.GetKeyDown(KeyCode.DownArrow)) // �������� 
            slidingSpeed = 1f; 
        if (Input.GetKeyUp(KeyCode.DownArrow))
            slidingSpeed = 0.2f;

        if (isWall)
        {
            canDoubleJump = true; isWallJump = false; // 2������ �����ϰ� �ϰ� ������ ���� �ƴϴ�
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
          

            if (Input.GetKeyDown(KeyCode.Z))
            {
                isWallJump = true;
                Invoke("FreezeX", 0.25f);
                rigid.velocity = new Vector2(-isRight * walljumpPower, 2.5f * walljumpPower);
                isRight = isRight * -1; anim.SetTrigger("isJump");
            }
        }
    }
    void FreezeX()
    {
        isWallJump = false;
    }
    void Move() // �����̴� �Լ�
    {
        float x;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) x = Input.GetAxis("Horizontal");
        else x = 0;

        if (!isAttack) rigid.velocity = new Vector2(x * moveSpeed, rigid.velocity.y);

        if (Mathf.Abs(rigid.velocity.x) > 0) anim.SetBool("isRun", true);
        else anim.SetBool("isRun", false);

    }
    void Flip()
    {
        if (rigid.velocity.x < 0) isRight = -1f;
        if (rigid.velocity.x > 0) isRight = 1f;
    }
    void Jump() // ����
    {
        isGround = Physics2D.OverlapCircle(overlapCirclePos.position, checkRadius, isLayer);
        isFall = rigid.velocity.y < 0; // �������� true �ƴҶ� false

        if (isGround)
        {
            anim.SetBool("isFall", isFall); //false
            canDoubleJump = true; // ���� ���� ��� 2�� ���� ���� ���·� ����
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rigid.velocity = Vector2.up * jumpPower;
                anim.SetTrigger("isJump");
            }
        }
        else// ���߿� �ִ� ���
        {
            anim.SetBool("isFall", isFall); // true
            if (!isFall) // Fall ���� �ƴ� ����
            {
                if (Input.GetKeyDown(KeyCode.Z) && !isAttack && canDoubleJump) // ���߿� �ְ� ���� ���� �ƴϸ� 2�� ���� ������ ������ ���
                {
                    canDoubleJump = false; // 2�� ���� ���
                    rigid.velocity = Vector2.up * jumpPower;
                    anim.SetTrigger("isDoubleJump");
                }

            }
        }
    }

    void Attack() // ����
    {
        attackBuffer -= Time.deltaTime; // �������϶� Ű �Է� �Ǵ��� �����ϱ� ���� ����
        if (attackBuffer <= 0) attackIndex = 1;//0���Ϸ� �������� 1�� ����
        if (Input.GetKeyDown(KeyCode.X))
            attackBuffer = 0.329f; // Ű ������ ���� �� ����

        if (AttackcurTime <= 0) // ���� ��Ÿ��
        {
            if (attackBuffer > 0) //���۰� 0�� �Ǿ����� ����
            {
                switch (attackIndex)
                {
                    case 1: // ù��° ����
                        AttackcurTime = attackcoolTime;
                        StartCoroutine(IsAttacking(attackcoolTime));
                        break;
                    case 2: // �ι�° ����
                        AttackcurTime = attackcoolTime;
                        StartCoroutine(IsAttacking(attackcoolTime));
                        break;

                }
                anim.SetTrigger("isAttack" + attackIndex.ToString());


                attackIndex++;
                if (attackIndex >= 3) attackIndex = 1; // 3�� �Ѿ�� 1��
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
        Gizmos.DrawRay(wallChkPos.position, Vector2.right * isRight * wallchkDistance);
    }
}
