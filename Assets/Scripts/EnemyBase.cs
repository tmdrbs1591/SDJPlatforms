using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] Slider Hpbar;
    [SerializeField] Slider Hpbar2;

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
        isRight = scale;

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
            playerObj = player.transform;
    }

    private void Update()
    {
        HPBar();

        if (!isDie)
        {
            Chase();
            Flip();
            Die();
         
        }
    }
    void HPBar()
    {
        Hpbar.value = Mathf.Lerp(Hpbar.value, (float)curHp / (float)MaxHp, Time.deltaTime * 20); ;
        Hpbar2.value = Mathf.Lerp(Hpbar2.value, (float)curHp / (float)MaxHp, Time.deltaTime * 3f); ;
    }
    void Chase() // ������°�
    {
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

    void Die()
    {
        if (curHp <= 0)
        {
            isDie = true;
            anim.SetTrigger("isDie");
            Invoke("DestroyEnemy", 5f);
            GameManager.instance.spawnManager.DeathMonster += 1; // ���� ī��Ʈ �߰�

            Hpbar.gameObject.SetActive(false);
            Hpbar2.gameObject.SetActive(false);
        }
  
    
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void Flip()
    {
        if (rigid.velocity.x < 0)
        {
            isRight = -scale;
        }
        else if (rigid.velocity.x > 0)
        {
            isRight = scale;
        }
        //transform.localScale = new Vector3(isRight, scale, scale);
        if (isRight == -1)
            spriteren.flipX = true;
        else
            spriteren.flipX = false;

    }

    public void TakeDamage(float damage)
    {
        float randomX = Random.Range(transform.position.x + 0.2f, transform.position.x - 0.4f);
        float randomY = Random.Range(transform.position.y + 0.3f, transform.position.y + 0.7f);

        Vector3 randomPosition = new Vector3(randomX, randomY, -5);

        if (!isDie)
        {
            Hpbar.gameObject.SetActive(true);
            Hpbar2.gameObject.SetActive(true);

            StartCoroutine(HitRoutine());

            Instantiate(damageText, randomPosition, Quaternion.identity).text = damage.ToString();

            ObjectPool.SpawnFromPool("HitEffect", randomPosition);
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
