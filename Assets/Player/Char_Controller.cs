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

    public Vector2 moveDirection = new Vector2();

    [SerializeField] private InputActionReference Move, Sprint, Punch, HeavyPunch;

    public GameObject punchBox;
    public GameObject heavyPunchBox;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }


    private float punchTimer = 0.0f;
    private float heavyPunchTimer = 0.0f;
    [SerializeField] private float punchDelay = 0.1f;
    [SerializeField] private float heavyPunchDelay = 0.3f;
    public bool attacking = false;

    public int currentDirection = 1; // 1 for right, -1 for left
    public float newDirection = 0.1f;

    private void Update()
    {
        if(punchTimer>punchDelay && heavyPunchDelay<heavyPunchTimer) moveDirection = Move.action.ReadValue<Vector2>();

        // Check if there's a change in input direction
        newDirection = moveDirection.x;

        if (newDirection > 0.1f) currentDirection = 1;
        if (newDirection < -0.1f) currentDirection = -1;

        // Set the local scale based on the currentDirection
        this.gameObject.transform.localScale = new Vector3(currentDirection * 2.4f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);

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
        if (!Sprint.action.IsPressed() && Move.action.ReadValue<Vector2>() == Vector2.zero)
        {
            if (Punch.action.IsPressed())
            {
                if (!attacking && punchTimer > punchDelay)
                {
                    Debug.Log("fuck");
                    punchTimer = 0.0f;
                    punchBox.SetActive(true);
                    attacking = true;
                }
            }
            else if (HeavyPunch.action.IsPressed())
            {
                if (!attacking && heavyPunchTimer > heavyPunchDelay)
                {
                    Debug.Log("Shit");
                    heavyPunchTimer = 0.0f;
                    heavyPunchBox.SetActive(true);
                    attacking = true;
                }
            }
            else
            {
                attacking = false;
            }
        }
        punchTimer += Time.deltaTime;
        heavyPunchTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(!sprinting)rb.velocity = new Vector2(moveDirection.x * walkSpeedX, moveDirection.y * walkSpeedY);
        else rb.velocity = new Vector2(moveDirection.x * sprintSpeedX, moveDirection.y * sprintSpeedY);
    }
}
