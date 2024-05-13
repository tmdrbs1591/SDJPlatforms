using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 스텟")]
    [SerializeField] private float moveSpeed; // 이동속도
    [SerializeField] private float jumpPower; // 점프 파워
    [SerializeField] private float walljumpPower; // 벽 점프 파워
    [SerializeField] private float attackcoolTime = 0.5f; // 공격 쿨타임
    [SerializeField] private float slidingSpeed; //벽 내려가는 스피드

    [Header("플레이어 바닥감지")]
    [Space]
    [SerializeField] private Transform overlapCirclePos;//플레이어 바닥 감지 위치
    [SerializeField] private float checkRadius; // 플레이어 바닥 감지 범위
    [SerializeField] private LayerMask isLayer; // 바닥 감지할 레이어

    [Header("플레이어 벽감지")]
    [Space]
    [SerializeField] private Transform wallChkPos; // 플레이어 벽 감지 위치
    [SerializeField] private float wallchkDistance; // 플레이어 벽 감지 범위
    [SerializeField] private LayerMask w_Layer; // 벽 감지 레이어



    private float isRight = 1f; // 오른쪽을 보고있는지 여부
    private float AttackcurTime;
    private float attackBuffer;

    private int attackIndex;

    private bool isAttack;
    private bool isGround;
    private bool isWall;
    [SerializeField] private bool isWallJump;
    private bool canDoubleJump = true; // 2단 점프 가능한 상태인지 여부
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
    void Wall() // 벽타기
    {
        isWall = Physics2D.Raycast(wallChkPos.position, Vector2.right * isRight, wallchkDistance, w_Layer); // 감지가 된다면 true 아니면 false
        anim.SetBool("isSliding", isWall);

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 내려갈때 
            slidingSpeed = 1f; 
        if (Input.GetKeyUp(KeyCode.DownArrow))
            slidingSpeed = 0.2f;

        if (isWall)
        {
            canDoubleJump = true; isWallJump = false; // 2단점프 가능하게 하고 벽점프 중이 아니다
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
    void Move() // 움직이는 함수
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
    void Jump() // 점프
    {
        isGround = Physics2D.OverlapCircle(overlapCirclePos.position, checkRadius, isLayer);
        isFall = rigid.velocity.y < 0; // 떨어질때 true 아닐땐 false

        if (isGround)
        {
            anim.SetBool("isFall", isFall); //false
            canDoubleJump = true; // 땅에 닿은 경우 2단 점프 가능 상태로 설정
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rigid.velocity = Vector2.up * jumpPower;
                anim.SetTrigger("isJump");
            }
        }
        else// 공중에 있는 경우
        {
            anim.SetBool("isFall", isFall); // true
            if (!isFall) // Fall 중이 아닐 때만
            {
                if (Input.GetKeyDown(KeyCode.Z) && !isAttack && canDoubleJump) // 공중에 있고 공격 중이 아니며 2단 점프 가능한 상태인 경우
                {
                    canDoubleJump = false; // 2단 점프 사용
                    rigid.velocity = Vector2.up * jumpPower;
                    anim.SetTrigger("isDoubleJump");
                }

            }
        }
    }

    void Attack() // 공격
    {
        attackBuffer -= Time.deltaTime; // 공격중일때 키 입력 되는지 감지하기 위한 버퍼
        if (attackBuffer <= 0) attackIndex = 1;//0이하로 내려가면 1로 설정
        if (Input.GetKeyDown(KeyCode.X))
            attackBuffer = 0.329f; // 키 누르면 버퍼 초 시작

        if (AttackcurTime <= 0) // 공격 쿨타임
        {
            if (attackBuffer > 0) //버퍼가 0이 되었으면 실행
            {
                switch (attackIndex)
                {
                    case 1: // 첫번째 공격
                        AttackcurTime = attackcoolTime;
                        StartCoroutine(IsAttacking(attackcoolTime));
                        break;
                    case 2: // 두번째 공격
                        AttackcurTime = attackcoolTime;
                        StartCoroutine(IsAttacking(attackcoolTime));
                        break;

                }
                anim.SetTrigger("isAttack" + attackIndex.ToString());


                attackIndex++;
                if (attackIndex >= 3) attackIndex = 1; // 3이 넘어가면 1로
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallChkPos.position, Vector2.right * isRight * wallchkDistance);
    }
}
