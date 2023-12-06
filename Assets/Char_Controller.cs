using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Char_Controller : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeedX = 5.0f;
    public float moveSpeedY = 2.5f;

    private PlayerInput playerInput;

    Vector2 moveDirection = new Vector2();

    [SerializeField] private InputActionReference Move, Fire;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveDirection = Move.action.ReadValue<Vector2>();
        if (Fire.action.IsPressed())
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeedX, moveDirection.y * moveSpeedY);
    }
}
