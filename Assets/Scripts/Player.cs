using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("�÷��̾� �⺻����")]
    [SerializeField] private float attackBuffercoolTime = 0.5f; 
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

    [Header("�÷��̾� ����")]
    [SerializeField] private Vector2 attackboxSize;
    [SerializeField] private Transform Attackpos;
    [SerializeField] private Ghost ghost;


    private float isRight = 1f; // �������� �����ִ��� ����
    private float AttackcurTime;
    private float dashcurTime;
    private float attackBuffer;

    private int attackIndex;

    private bool isAttack;
    private bool isGround;
    private bool isDash;
    private bool isWall;
    private bool isWallJump;
    private bool canDoubleJump = true; // 2�� ���� ������ �������� ����
    private bool isFall;

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
        attackBuffercoolTime = PlayerStatManager.instance.attackcoolTime - 0.01f;



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
                rigid.velocity = new Vector2(-isRight * PlayerStatManager.instance.walljumpPower, 2.5f * PlayerStatManager.instance.walljumpPower);
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

        if (!isDash)
        {
            float x;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) x = Input.GetAxis("Horizontal"); // �����̱�
            else x = 0;

            rigid.velocity = new Vector2(x * PlayerStatManager.instance.moveSpeed, rigid.velocity.y);

            if (Mathf.Abs(rigid.velocity.x) > 0) anim.SetBool("isRun", true);
            else anim.SetBool("isRun", false);

            if (dashcurTime <= 0)
            {
                if (Input.GetKey(KeyCode.C)) // �뽬
                {

                    dashcurTime = PlayerStatManager.instance.dashcoolTime;
                    StartCoroutine(Dash()); 
                }
            }
            else dashcurTime -= Time.deltaTime;
        }


        if (isAttack && isGround)
            rigid.velocity = new Vector2(0, rigid.velocity.y);
    }
    IEnumerator Dash()
    {
        if (isGround)
            anim.SetTrigger("isDash");
        isAttack = false;

        isDash = true; ghost.makeGhost = true;

        // ��� ���⿡ ���� ���� ����
        Vector2 dashForce = new Vector2(isRight * 4 * 5, 0);
        rigid.AddForce(dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.08f);

        isDash = false; ghost.makeGhost = false;

    }

    void Flip()
    {

        if (rigid.velocity.x < 0)
            isRight = -1f;
        else if (rigid.velocity.x > 0)
            isRight = 1f;
        else
            ghost.makeGhost = false;

    }
    void Jump() // ����
    {
        isGround = Physics2D.OverlapCircle(overlapCirclePos.position, checkRadius, isLayer);
        isFall = rigid.velocity.y < -0.1; // �������� true �ƴҶ� false

        if (isGround)
        {
            anim.SetBool("isFall", isFall); //false

            canDoubleJump = true; // ���� ���� ��� 2�� ���� ���� ���·� ����

            if (Input.GetKeyDown(KeyCode.Z))
            {
                rigid.velocity = Vector2.up * PlayerStatManager.instance.jumpPower;
                anim.SetTrigger("isJump");
            }

        }
        else// ���߿� �ִ� ���
        {
            anim.SetBool("isFall", isFall); // true
            if (!isGround) // Fall ���� �ƴ� ����
            {
                if (Input.GetKeyDown(KeyCode.Z) && !isAttack && canDoubleJump) // ���߿� �ְ� ���� ���� �ƴϸ� 2�� ���� ������ ������ ���
                {
                    canDoubleJump = false; // 2�� ���� ���
                    rigid.velocity = Vector2.up * PlayerStatManager.instance.jumpPower;
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
            attackBuffer = attackBuffercoolTime; // Ű ������ ���� �� ����

        if (AttackcurTime <= 0) // ���� ��Ÿ��
        {
            if (attackBuffer > 0) //���۰� 0�� �Ǿ����� ����
            {

                switch (attackIndex)
                {
                    case 1: // ù��° ����

                        StartCoroutine(DamageAttack(0.2f,0));
                        AttackcurTime = PlayerStatManager.instance.attackcoolTime;
                        StartCoroutine(IsAttacking(PlayerStatManager.instance.attackcoolTime - 0.08f));

                        break;
                    case 2: // �ι�° ����
                        StartCoroutine(DamageAttack(0.2f,1));
                        AttackcurTime = PlayerStatManager.instance.attackcoolTime;
                        StartCoroutine(IsAttacking(PlayerStatManager.instance.attackcoolTime - 0.08f));
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
    IEnumerator DamageAttack(float waitTime,float plusDamage)
    {
        yield return new WaitForSeconds(waitTime);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Attackpos.position, attackboxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider != null)
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponent<EnemyBase>().TakeDamage(PlayerStatManager.instance.attackdamage + plusDamage);
                }
            }
        }
    }
 
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallChkPos.position, Vector2.right * isRight * wallchkDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Attackpos.position, attackboxSize);
    }
}
