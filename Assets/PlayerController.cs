using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10;
    [SerializeField] float friction = 0.025f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        var speed = new Vector2();

        if (Input.GetKey(KeyCode.W))
            speed += (Vector2.up * walkSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S))
            speed += (Vector2.down * walkSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            speed += (Vector2.left * walkSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            speed += (Vector2.right * walkSpeed * Time.deltaTime);

        rb.velocity = Vector2.Lerp(rb.velocity, speed, friction);
    }

    
}
