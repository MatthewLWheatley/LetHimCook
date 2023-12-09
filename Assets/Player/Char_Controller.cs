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
    public float sprintTimer = 0.0f;
    public float sprintDelay = 0.75f;
    public bool sprinting = false;

    private PlayerInput playerInput;

    public Vector2 moveDirection = new Vector2();

    //[SerializeField] private InputActionReference Move, Sprint, Punch, HeavyPunch;

    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    public float currentDelay  = 0.0f;
    public float Timer = 0.0f;

    [SerializeField] private float punchDelay = 0.1f;
    [SerializeField] private float heavyPunchDelay = 0.75f;
    public bool punching = false;
    public bool heavyPunching = false;
    public GameObject punchBox;
    public GameObject heavyPunchBox;
    [SerializeField] private float kickDelay = 0.2f;
    [SerializeField] private float heavyKickDelay = 0.8f;
    public bool kicking = false;
    public bool heavyKicking = false;
    public GameObject kickBox;
    public GameObject heavyKickBox;

    public bool attacking = false;

    public int currentDirection = 1; // 1 for right, -1 for left
    public Vector2 newDirection = new Vector2(0.1f,0);

    public void OnMove(InputAction.CallbackContext context)
    {
         moveDirection = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // Sprint button is pressed
            sprinting = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Sprint button is released
            sprinting = false;
        }
    }

    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

            punching = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            punching = false;
        }
    }

    public void OnHeavyPunch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            heavyPunching = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            heavyPunching = false;
        }
    }

    public void OnKick(InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Started)
        {
            kicking = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            kicking = false;
        }
    }
    
    public void OnHeavyKick(InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Started)
        {
            heavyKicking = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            heavyKicking = false;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }

    private void Update()
    {


        if (Timer * 2 < currentDelay) newDirection = Vector2.zero;
        else newDirection = moveDirection;

        if (newDirection.x > 0.1f) currentDirection = 1;
        if (newDirection.x < -0.1f) currentDirection = -1;

            // Set the local scale based on the currentDirection
            this.gameObject.transform.localScale = new Vector3(currentDirection * 2.4f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);

        if (!sprinting && newDirection == Vector2.zero)
        {
            if (punching)
            {
                if (!attacking && Timer > currentDelay)
                {
                    Timer = 0.0f;
                    currentDelay = punchDelay;
                    punchBox.SetActive(true);
                    attacking = true;
                    punching = true;
                }
            }
            else if (heavyPunching)
            {
                if (!attacking && Timer > currentDelay)
                {
                    Timer = 0.0f;
                    currentDelay = heavyPunchDelay;
                    heavyPunchBox.SetActive(true);
                    attacking = true;
                    heavyPunching = false;
                }
            }
            else if (kicking)
            {
                if (!attacking && Timer > currentDelay)
                {
                    Timer = 0.0f;
                    currentDelay = kickDelay;
                    kickBox.SetActive(true);
                    attacking = true;
                    kicking = false;
                }
            }
            else if (heavyKicking)
            {
                if (!attacking && Timer > currentDelay)
                {
                    Timer = 0.0f;
                    currentDelay = heavyKickDelay;
                    heavyKickBox.SetActive(true);
                    attacking = true;
                    heavyKicking = false;
                }
            }
            else
            {
                attacking = false;
                punching = false;
                heavyPunching = false;
                kicking = false;
                heavyKicking = false;
            }
        }
        Timer += Time.deltaTime;
    }
        
    void FixedUpdate()
    {
        if (!sprinting) rb.velocity = new Vector2(newDirection.x * walkSpeedX, newDirection.y * walkSpeedY);

        if (sprinting && sprintDelay < sprintTimer) rb.velocity = new Vector2(newDirection.x * sprintSpeedX, newDirection.y * sprintSpeedY);
        else if (sprinting)
        {
            sprintTimer += Time.deltaTime;
        }
        if(!sprinting)
        {
            sprintTimer = 0;
        }
    }
}
