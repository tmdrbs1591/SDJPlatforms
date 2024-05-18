using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] potal;
    [SerializeField] private Transform[] potalSpawnPos; // 포탈 생성 위치
    public float DeathMonster; //죽은 몬스터 개수 이 수 만큼 생성
    public float purposeDeathMonster; //죽은 몬스터 개수


    void Start()
    {
        
    }

    void Update()
    {
        if (DeathMonster >= purposeDeathMonster)
        {
            // 포탈 생성
            potal[0].SetActive(true);
        }
    }
}
