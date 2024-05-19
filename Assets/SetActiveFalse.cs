using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ReturnToPool", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ReturnToPool()
    {
        ObjectPool.ReturnToPool("HitEffect",gameObject);
    }
}
