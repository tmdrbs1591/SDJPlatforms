using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public SpawnManager spawnManager;
    public GameObject fadeOut;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        fadeOut.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
