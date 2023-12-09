using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class AIProps : MonoBehaviour
{
    [SerializeField] private float health = 3.0f;
    [SerializeField] private Rigidbody2D rb;



    private bool takingDamage = false;
    private float damageTimer = 0.0f;
    [SerializeField] private float damageDuration = 0.1f;
    [SerializeField] private Color damageColor = Color.red;


    [SerializeField] private float baseKnockBack = 0.1f;
    [SerializeField] private float punchKnockBack = 0.3f;
    [SerializeField] private float punchDamage = 2.0f;
    [SerializeField] private float heavyPunchKnockBack = 1.2f;
    [SerializeField] private float heavyPunchDamage = 5.0f;
    [SerializeField] private float kickKnockBack = 0.5f;
    [SerializeField] private float kickDamage = 1.5f;
    [SerializeField] private float heavyKickKnockBack = 2.0f;
    [SerializeField] private float heavyKickDamage = 4.0f;

    Vector2 knockbackDestination;
    public float knockBackDelay = 0.1f;
    public float knockBackStep = 0.5f;
    float Timer = 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackDestination = this.transform.position;
    }

    private void Update()
    {
        damageflash();

        if (Timer < knockBackDelay)
        {
            Vector3 temp;
            temp.y = Vector3.MoveTowards(new Vector3(0, this.transform.position.y, 0), new Vector3(0, knockbackDestination.y, 0), knockBackStep).y;
            temp.x = Vector3.MoveTowards(new Vector3(this.transform.position.x, 0, 0), new Vector3(knockbackDestination.x, 0, 0), knockBackStep).x;
            temp.z = 0;
            this.transform.position = temp;
        }
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
        if (collision.gameObject.CompareTag("PlayerPunchBox"))
        {
            Debug.Log("punch");
            health -= punchDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position).normalized;
            hitDir.y = 0;
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x*punchKnockBack*baseKnockBack;
            takingDamage = true;
            this.GetComponent<FollowChar>().timer = 0;
            Timer = 0;
        }
        if (collision.gameObject.CompareTag("PlayerHeavyPunchBox"))
        {
            Debug.Log("heavypunch");
            health -= heavyPunchDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position);
            hitDir.y = 0;
            hitDir.Normalize();
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * heavyPunchKnockBack * baseKnockBack;
            takingDamage = true;
            takingDamage = true;
            this.GetComponent<FollowChar>().timer = 0;
            Timer = 0;
        }
        if (collision.gameObject.CompareTag("PlayerKickBox"))
        {
            Debug.Log("kick");
            health -= kickDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position).normalized;
            hitDir.y = 0;
            hitDir.Normalize();
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * kickKnockBack * baseKnockBack;
            takingDamage = true;
            takingDamage = true;
            this.GetComponent<FollowChar>().timer = 0;
            Timer = 0;
        }
        if (collision.gameObject.CompareTag("PlayerHeavyKickBox"))
        {
            Debug.Log("heavykick");
            health -= heavyKickDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position).normalized;
            hitDir.y = 0;
            hitDir.Normalize();
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * heavyKickKnockBack * baseKnockBack;
            takingDamage = true;
            takingDamage = true;
            this.GetComponent<FollowChar>().timer = 0;
            Timer = 0;
        }
        if (health <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
