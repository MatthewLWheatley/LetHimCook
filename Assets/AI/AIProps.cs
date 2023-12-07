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

    private bool takingDamage = false;
    private float damageTimer = 0.0f;
    [SerializeField] private float damageDuration = 0.1f;
    [SerializeField] private Color damageColor = Color.red;


    private void Update()
    {
        damageflash();
    }

    private void damageflash() 
    {
        if (takingDamage && damageTimer < damageDuration)
        {
            damageTimer += Time.deltaTime;
            this.gameObject.GetComponent<SpriteRenderer>().color = damageColor;
        }
        else 
        {
            damageTimer = 0;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            takingDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (collision.gameObject.CompareTag("PlayerPunchBox"))
        {
            if (collision.gameObject.name == "PunchBox")
            {
                Debug.Log("small");
                health -= 1.0f;
                Vector2 hitDir = (collision.transform.position - this.transform.position).normalized;
                //rb.AddForce(-hitDir*knockbackPower,ForceMode2D.Impulse);
                takingDamage = true;
                this.GetComponent<FollowChar>().timer = 0;
            }
            if (collision.gameObject.name == "HeavyPunchBox") 
            {
                Debug.Log("heavy");
                health -= 69.0f;
                Vector2 hitDir = (collision.transform.position - this.transform.position).normalized;
                //rb.AddForce(-hitDir*knockbackPower,ForceMode2D.Impulse);
                takingDamage = true;
                this.GetComponent<FollowChar>().timer = 0;
            }
        }
        if (health <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }

    }
}
