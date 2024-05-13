using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class TestPlayerContolloer : MonoBehaviour
{
    Rigidbody2D rd;

    public float Speed;
    public float JumpForce;

    public float PosX;
    public float PosY;
    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();


    }
    void Update()
    {
        SetInputKey();
        MoveingController();
    }
    void SetInputKey()
    {
        PosX = Input.GetAxisRaw("Horizontal") * Speed;
    }
    void MoveingController()
    {
        Moveing();
        Jumping();
    }
    void Moveing()
    {
        rd.velocity = new Vector2(PosX, rd.velocity.y);
    }
    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rd.velocity = Vector2.up * JumpForce;
    }
}
