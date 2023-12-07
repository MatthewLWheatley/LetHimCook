using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIProps : MonoBehaviour
{
    [SerializeField] private float health = 3.0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackPower = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.gameObject.CompareTag("PlayerPunchBox"))
        {
            Debug.Log("oo");
            health -= 1.0f;
            Vector2 hitDir = (collision.transform.position - this.transform.position).normalized;
            rb.AddForce(hitDir*knockbackPower);
        }
        if (health <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
