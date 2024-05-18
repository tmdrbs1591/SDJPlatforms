using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance { get; private set; }

    [Header("�÷��̾� ����")]
    public float moveSpeed; // �̵��ӵ�
    public float jumpPower; // ���� �Ŀ�
    public float walljumpPower; // �� ���� �Ŀ�
    public float attackcoolTime = 0.5f; // ���� ��Ÿ��
    public float dashcoolTime = 0.5f; // �뽬 ��Ÿ��
    public float attackdamage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
    }
}
