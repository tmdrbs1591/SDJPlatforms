using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private float speed = 3f; // �̵� �ӵ�
    [SerializeField] private float radius = 5f; // �߰� ���� ���� ������
    [SerializeField] private LayerMask targetLayer; // �߰��� Ÿ���� ���̾�
    [SerializeField] private Transform playerObj; // �÷��̾��� ��ġ
    [SerializeField] private float contactDistance = 1.5f; // ���� �Ÿ� ����
    [SerializeField] private float knockBackPowr = 1.5f; // �˹� ����

    [Header("���׸���")]
    [SerializeField] private Material hitMaterial; 
    [SerializeField] private Material nomalMaterial;

    [Header("ü��,������")]
    [SerializeField] private float curHp;
    [SerializeField] private float MaxHp;
    [SerializeField] private float scale;

    [Header("����Ʈ")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private GameObject damageEffect;


    private bool inChase = false; // �߰� ������ ����
    private bool isDie = false; 
    private Rigidbody2D rigid; // Rigidbody2D ������Ʈ
    private Animator anim;
    private SpriteRenderer spriteren;
    private float isRight;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteren = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
            playerObj = player.transform;
    }

    private void Update()
    {
      
        Chase();
        Flip();
        Die();
    }

    void Chase()
    {if (!isDie) { 
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        foreach (Collider2D col in hit)
        {
            if (col)
            {
                inChase = true;
                break;
            }
        }

        if (Vector2.Distance(transform.position, playerObj.position) > contactDistance && inChase)
        {
            Vector2 direction = (playerObj.position - transform.position).normalized;
            rigid.velocity = new Vector2(direction.x * speed, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        inChase = false;
        }
    }
    void Die()
    {
        if (curHp <= 0)
        {
            isDie = true;
            anim.SetTrigger("isDie");
            Invoke("Destroy", 6f);
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    void Flip()
    {
     
        if (rigid.velocity.x < 0) { transform.localScale = new Vector3(isRight, scale, scale); isRight = -scale; }
           
        else if (rigid.velocity.x > 0) { transform.localScale = new Vector3(isRight, scale, scale); isRight = scale; }
          

    }
    public void TakeDamage(float damage)
    {
        float randomX = Random.Range(transform.position.x+0.2f, transform.position.x - 0.4f);
        float randomY = Random.Range(transform.position.y  +0.3f, transform.position.y+ 0.7f);

        Vector3 randomPosition = new Vector3(randomX, randomY,-5);
        if (!isDie)
        {
            StartCoroutine(HitRoutine());

            Instantiate(damageText, randomPosition, Quaternion.identity).text = damage.ToString();
            Instantiate(damageEffect, randomPosition, Quaternion.identity);
            anim.SetTrigger("isHit");
            transform.Translate(new Vector2(isRight * -1 * knockBackPowr, rigid.velocity.y));
            curHp -= damage;
        }

    }

    private IEnumerator HitRoutine()
    {
        spriteren.material = hitMaterial;

        yield return new WaitForSeconds(0.1f);

        spriteren.material = nomalMaterial;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
