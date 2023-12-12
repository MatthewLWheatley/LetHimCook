using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class AIProps : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private float health = 3.0f;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject PunchBox;

    private bool takingDamage = false;
    private float damageTimer = 0.0f;
    [SerializeField] private float damageDuration = 0.1f;
    [SerializeField] private Color damageColor = Color.red;


    [SerializeField] private float baseKnockBack = 0.1f;
    [SerializeField] private float punchKnockBack = 0.3f;
    [SerializeField] private float punchDamage = 2.0f;
    [SerializeField] private float punchStunTime = 0.1f;
    [SerializeField] private float heavyPunchKnockBack = 1.2f;
    [SerializeField] private float heavyPunchDamage = 5.0f;
    [SerializeField] private float HeavyPunchStunTime = 0.3f;
    [SerializeField] private float kickKnockBack = 0.5f;
    [SerializeField] private float kickDamage = 1.5f;
    [SerializeField] private float KickStunTime = 0.2f;
    [SerializeField] private float heavyKickKnockBack = 2.0f;
    [SerializeField] private float heavyKickDamage = 4.0f;
    [SerializeField] private float HeavyKickStunTime = 0.4f;

    Vector2 knockbackDestination;
    public float knockBackDelay = 0.1f;
    public float knockBackStep = 0.5f;
    public float Timer = 0.1f;
    public float stunTimer = 0.0f;
    public float stunDelay = 0.0f;

    // Variables from FollowChar
    private GameObject player1;
    private GameObject player2;
    private GameObject TargetPlayer;
    [SerializeField] private Vector3 targetPos = Vector3.zero;
    public float xSpeed = 1.0f;
    public float ySpeed = 1.0f;
    public float followCharTimer = 0;  // Renamed timer from FollowChar to avoid conflict with AIProps
    public int state = 0;  // State variable from FollowChar
    [SerializeField] private float delayTimer = 2.0f;
    [SerializeField] private float floatingDistantce = 1.5f;

    private Vector3 lastPosition;
    private Transform myTransform;
    private float randomYRange = .5f;

    [SerializeField] private float attackInterval = 0.8f; // Time between attacks
    [SerializeField] private float attackDuration = 0.1f; // Duration of each attack

    private float attackTimer = 0.0f; // Timer to track time since last attack
    private bool isAttacking = false; // Flag to check if currently attacking

    [SerializeField] private float floatingRegion = 0.5f;

    public GameObject[] allAIs;

    private void FindPlayers()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0) player1 = players[0];
        if (players.Length > 1) player2 = players[1];
        if (player1 != null && player2 != null)
        {
            if (((player1.transform.position - this.transform.position) / 2).magnitude < ((player2.transform.position - this.transform.position) / 2).magnitude)
            {
                TargetPlayer = player1;
            }
            else
            {
                TargetPlayer = player2;
            }
        }
        else if (player1 != null)
        {
            TargetPlayer = player1;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackDestination = this.transform.position;

        //Debug.Log("awake");
        FindPlayers();

        myTransform = transform;
        lastPosition = myTransform.position;
        allAIs = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private bool IsOtherAICloseToTargetOnSide(Vector3 side)
    {
        allAIs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject ai in allAIs)
        {
            if (ai != this.gameObject)
            { // Ignore the current AI
                float distanceToTarget = Vector3.Distance(ai.transform.position, TargetPlayer.transform.position);
                bool isOnSameSide = Vector3.Dot(ai.transform.position - TargetPlayer.transform.position, side) > 0;

                if (distanceToTarget < floatingDistantce + floatingRegion && isOnSameSide)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Update()
    {
        damageflash();

        LookAtPlayerOnX();

        if (Timer < knockBackDelay)
        {
            Vector3 temp;
            temp.y = Vector3.MoveTowards(new Vector3(0, this.transform.position.y, 0), new Vector3(0, knockbackDestination.y, 0), knockBackStep).y;
            temp.x = Vector3.MoveTowards(new Vector3(this.transform.position.x, 0, 0), new Vector3(knockbackDestination.x, 0, 0), knockBackStep).x;
            temp.z = 0;
            this.transform.position = temp;
        }

        Timer += Time.deltaTime;

        if (player1 == null) return;

        FindPlayers();
        //Debug.Log(state);

        followCharTimer += Time.deltaTime;

        if (state == 0)
        {
            if (followCharTimer > delayTimer)
            {
                Vector2 pos = this.transform.position;
                Vector2 pos2 = TargetPlayer.transform.position;



                // Calculate the distance separately for X and Y
                float xDistance = Mathf.Abs(pos.x - pos2.x);
                float yDistance = Mathf.Abs(pos.y - pos2.y);

                if (xDistance >= floatingDistantce + floatingRegion + 0.1f || yDistance >= 0.5f)
                {
                    state = 1;
                    followCharTimer = 0;
                }
                else
                {
                    state = 2;
                    followCharTimer = 0;
                }
            }
        }
        if (state == 1) 
        {
            float xStep = xSpeed * Time.deltaTime;
            float yStep = ySpeed * Time.deltaTime;


            Vector3 leftOffset = new Vector3(-(floatingDistantce * 1.25f), 0, 0);
            Vector3 rightOffset = new Vector3(floatingDistantce * 1.25f, 0, 0);

            bool isLeftOccupied = IsOtherAICloseToTargetOnSide(leftOffset);
            bool isRightOccupied = IsOtherAICloseToTargetOnSide(rightOffset);

            Vector3 chosenOffset;

            if (isLeftOccupied && isRightOccupied)
            {
                // If both sides are occupied, choose the closest side
                float distanceToLeft = Vector3.Distance(this.transform.position, TargetPlayer.transform.position + leftOffset);
                float distanceToRight = Vector3.Distance(this.transform.position, TargetPlayer.transform.position + rightOffset);

                chosenOffset = (distanceToLeft <= distanceToRight) ? leftOffset : rightOffset;
            }
            else if (isLeftOccupied)
            {
                // If only left is occupied, choose right
                chosenOffset = rightOffset;
            }
            else
            {
                // If left is not occupied (or both are free), choose left
                chosenOffset = leftOffset;
            }

            // Calculate target position with the chosen offset
            targetPos = TargetPlayer.transform.position + chosenOffset;

            // Add randomness to the y-coordinate of the target position
            float randomYOffset = Random.Range(-randomYRange, randomYRange);
            targetPos.y += randomYOffset;

            Vector3 temp = Vector3.zero;
            temp.y = Vector3.MoveTowards(new Vector3(0, this.transform.position.y, 0), new Vector3(0, targetPos.y, 0), yStep).y;
            temp.x = Vector3.MoveTowards(new Vector3(this.transform.position.x, 0, 0), new Vector3(targetPos.x, 0, 0), xStep).x;
            temp.z = 0;

            float distanceToTarget = Vector3.Distance(this.transform.position, targetPos);

            if (distanceToTarget <= floatingRegion)
            {
                // Stop movement by not updating the position
                followCharTimer = 0;
                state = 0;
            }
            else
            {
                this.transform.position = temp;
            }
        }
        if (state == 2)
        {
            attackTimer += Time.deltaTime;

            //Debug.Log("Attacking");

            if (!isAttacking && attackTimer >= attackInterval)
            {
                // Start attacking
                //Debug.Log("Attacking");
                isAttacking = true;
                attackTimer = 0.0f;

                // Activate punch boxes here
                PunchBox.gameObject.SetActive(true);
            }
            if (isAttacking && attackTimer >= attackDuration)
            {
                //Debug.Log("attack stop");
                // Stop attacking
                isAttacking = false;
                attackTimer = 0.0f;

                // Deactivate punch boxes here
                PunchBox.gameObject.SetActive(false);

                // Calculate distance from the target player
                float distanceTTarget = Vector2.Distance(transform.position, TargetPlayer.transform.position);

                // Check if distance is less than floating distance
                if (distanceTTarget < floatingDistantce + floatingRegion)
                {
                    // Keep attacking state
                    state = 2;
                    followCharTimer = 0; // Reset timer if you want to immediately reevaluate attacking
                }
                else
                {
                    //Debug.Log("switch");
                    // Switch back to state 0
                    state = 0;
                }
            }
        }
        if (state == 4) 
        { 
            //stunned
            stunTimer += Time.deltaTime;

            if (stunTimer >= stunDelay) 
            {
                state = 2;
            }
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

    public void HitReg(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerPunchBox"))
        {
            //Debug.Log("punch");
            health -= punchDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position).normalized;
            hitDir.y = 0;
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * punchKnockBack * baseKnockBack;
            takingDamage = true;
            followCharTimer = 0;
            Timer = 0;
            state = 4;
            stunDelay = punchStunTime;
            stunTimer = 0;
        }
        if (collision.gameObject.CompareTag("PlayerHeavyPunchBox"))
        {
            //Debug.Log("heavypunch");
            health -= heavyPunchDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position);
            hitDir.y = 0;
            hitDir.Normalize();
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * heavyPunchKnockBack * baseKnockBack;
            takingDamage = true;
            takingDamage = true;
            followCharTimer = 0;
            Timer = 0;
            state = 4;
            stunDelay = HeavyPunchStunTime;
            stunTimer = 0;
        }
        if (collision.gameObject.CompareTag("PlayerKickBox"))
        {
            //Debug.Log("kick");
            health -= kickDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position).normalized;
            hitDir.y = 0;
            hitDir.Normalize();
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * kickKnockBack * baseKnockBack;
            takingDamage = true;
            takingDamage = true;
            followCharTimer = 0;
            Timer = 0;
            state = 4;
            stunDelay = KickStunTime;
            stunTimer = 0;
        }
        if (collision.gameObject.CompareTag("PlayerHeavyKickBox"))
        {
            //Debug.Log("heavykick");
            health -= heavyKickDamage;
            Vector2 hitDir = (collision.gameObject.transform.parent.transform.position - this.transform.position).normalized;
            hitDir.y = 0;
            hitDir.Normalize();
            knockbackDestination = this.transform.position;
            knockbackDestination.x -= hitDir.x * heavyKickKnockBack * baseKnockBack;
            takingDamage = true;
            takingDamage = true;
            followCharTimer = 0;
            Timer = 0;
            state = 4;
            stunDelay = HeavyKickStunTime;
            stunTimer = 0;
        }
        if (health <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void LookAtPlayerOnX()
    {
        if (TargetPlayer != null)
        {
            // Determine the direction to the target player on the X plane
            Vector3 directionToTarget = TargetPlayer.transform.position - transform.position;

            // Check if the target is on the left or right side of the AI
            if (directionToTarget.x > 0)
            {
                // Target is to the right, face right
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 0.25f, transform.localScale.z);
            }
            else if (directionToTarget.x < 0)
            {
                // Target is to the left, face left
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 0.25f, transform.localScale.z);
            }
            // If the target is directly above or below, the AI will maintain its current orientation
        }
    }
}