using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] potal;
    [SerializeField] private Transform[] potalSpawnPos; // ��Ż ���� ��ġ
    public float DeathMonster; //���� ���� ���� �� �� ��ŭ ����
    public float purposeDeathMonster; //���� ���� ����


    void Start()
    {
        
    }

    void Update()
    {
        if (DeathMonster >= purposeDeathMonster)
        {
            // ��Ż ����
            potal[0].SetActive(true);
        }
    }
}
