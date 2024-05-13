using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Contolloer : MonoBehaviour
{
    public float Speed = 3f;
    private float ReSpeed;
    public float radius = 5f;        //추격 범위 원의 반지름 //기본값 5

    public LayerMask TarGetLayer;
    public Transform PlayerObj;
    public float contactDistance = 1.5f;    //근접거리 범위

    public float Whidth = 1.5f;       //Box Collider Width
    public float Height = 1.5f;       //Box Collider Heiht
    public float AttakeRange;           //공격 범위
    public Transform AttakeRangePotition;   //공격 위치

    public bool Inchase = false;    //추격 할지 말지 선언
    public bool InAttake = false;   //공격중인지 확인
    private Vector2 BoxColliderSize;

    public Animator animator;   //애니메이션을 가진 무기 지어주삼
    private float PlayTime;
    private void Start()
    {
        ReSpeed = Speed;
    }
    public void Update()
    {
        BoxColliderSize = new Vector2(Whidth, Height);
        if (Vector2.Distance(transform.position, PlayerObj.position) > AttakeRange + contactDistance)
        {
            Chase();
            Attake();
        }
    }
    void Chase()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius,TarGetLayer);

        foreach (Collider2D col in hit)
        {
            if (col)    Inchase = true;
        }

        if (Vector2.Distance(transform.position, PlayerObj.position) > contactDistance && Inchase)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerObj.position, Speed * Time.deltaTime);
        }
            
            Inchase = false;
    }
    private void Attake()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(AttakeRangePotition.position, BoxColliderSize, AttakeRange, TarGetLayer);

        foreach(Collider2D col in hit)
        {
            if(col)
            {
                //공격 애니메이션 실행
                InAttake = true;
                Debug.Log("attack");

                //Test용 공격 모션
            }
        }
        AttackAnamtion(InAttake);
        if (InAttake)
        {
            Speed = 0;
            Invoke("re",PlayTime);
        }
        InAttake = false;
    }
    void re()
    {
        Speed = ReSpeed;
    }
    public void AttackAnamtion(bool InAttake)
    {
        animator.SetBool("Attake",InAttake); PlayTime = animator.GetCurrentAnimatorStateInfo(0).length;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
        Gizmos.DrawWireCube(AttakeRangePotition.position, BoxColliderSize);
    }
}
