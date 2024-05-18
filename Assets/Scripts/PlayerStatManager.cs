using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance { get; private set; }

    [Header("플레이어 스텟")]
    public float moveSpeed; // 이동속도
    public float jumpPower; // 점프 파워
    public float walljumpPower; // 벽 점프 파워
    public float attackcoolTime = 0.5f; // 공격 쿨타임
    public float dashcoolTime = 0.5f; // 대쉬 쿨타임
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
