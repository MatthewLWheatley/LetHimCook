using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitReg : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.gameObject.transform.parent.GetComponent<AIProps>().HitReg(collision);
    }
}
