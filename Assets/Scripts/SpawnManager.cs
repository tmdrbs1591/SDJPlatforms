using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] potal; // ��Ż ������ �迭
    [SerializeField] private Transform[] potalSpawnPos; // ��Ż ���� ��ġ �迭
    public float DeathMonster; // ���� ���� ����
    public float purposeDeathMonster; // ��ǥ ���� ���� ����

    void Start()
    {
        // �ʿ��� �ʱ� ������ ���⼭ �մϴ�.
    }

    void Update()
    {
        if (DeathMonster >= purposeDeathMonster)
        {
            for (int i = 0; i < potalSpawnPos.Length; i++)
            {
                // ��Ż ������ �� �ϳ��� �������� ����
                GameObject randomPortal = potal[Random.Range(0, potal.Length)];
                
                // ���� ���� ��ġ
                Transform currentSpawnPos = potalSpawnPos[i];
                
                // ���õ� ��ġ�� ��Ż ����
                Instantiate(randomPortal, currentSpawnPos.position, Quaternion.identity);
            }

            // ��Ż ���� �� DeathMonster�� �ʱ�ȭ (�ʿ��� ���)
            DeathMonster = 0;
        }
    }
}
