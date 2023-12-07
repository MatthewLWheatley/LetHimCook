using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Char_Controller : MonoBehaviour
{
    public Rigidbody2D rb;


    public float walkSpeedX = 5.0f;
    public float walkSpeedY = 2.5f;
    public float sprintSpeedX = 6.5f;
    public float sprintSpeedY = 3.25f;
    private float sprintTimer = 0.0f;
    public float sprintDelay = 0.75f;
    public bool sprinting = false;

    private PlayerInput playerInput;

    Vector2 moveDirection = new Vector2();

    [SerializeField] private InputActionReference Move, Sprint, Punch;

    public GameObject hitBox;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }


    private float attackTimer = 0.0f;
    [SerializeField] private float attackDelay = 0.25f;
    public bool attacking = false;

    private void Update()
    {
        moveDirection = Move.action.ReadValue<Vector2>();
        
        if (Sprint.action.IsPressed())
        {
            if (sprintTimer >= sprintDelay)
            {
                sprinting = true;
            }
            else 
            {
                sprintTimer += Time.deltaTime;
            }
        }
        else
        {
            sprinting = false;
            sprintTimer = 0.0f;
        }
        if (Punch.action.IsPressed() && !Sprint.action.IsPressed()) 
        {
            if (!attacking && attackTimer > attackDelay)
            {
                Debug.Log("fuck");
                attackTimer = 0.0f;
                hitBox.SetActive(true);
            }
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if(!sprinting)rb.velocity = new Vector2(moveDirection.x * walkSpeedX, moveDirection.y * walkSpeedY);
        else rb.velocity = new Vector2(moveDirection.x * sprintSpeedX, moveDirection.y * sprintSpeedY);
    }
}
