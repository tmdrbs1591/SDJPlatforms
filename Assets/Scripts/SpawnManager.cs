using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] potal; // 포탈 프리팹 배열
    [SerializeField] private Transform[] potalSpawnPos; // 포탈 생성 위치 배열
    public float DeathMonster; // 죽은 몬스터 개수
    public float purposeDeathMonster; // 목표 죽은 몬스터 개수

    void Start()
    {
        // 필요한 초기 설정을 여기서 합니다.
    }

    void Update()
    {
        if (DeathMonster >= purposeDeathMonster)
        {
            for (int i = 0; i < potalSpawnPos.Length; i++)
            {
                // 포탈 프리팹 중 하나를 랜덤으로 선택
                GameObject randomPortal = potal[Random.Range(0, potal.Length)];
                
                // 현재 스폰 위치
                Transform currentSpawnPos = potalSpawnPos[i];
                
                // 선택된 위치에 포탈 생성
                Instantiate(randomPortal, currentSpawnPos.position, Quaternion.identity);
            }

            // 포탈 생성 후 DeathMonster를 초기화 (필요한 경우)
            DeathMonster = 0;
        }
    }
}
