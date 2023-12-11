using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.gameObject.transform.parent.GetComponent<Char_Controller>().HitReg(collision);
    }
}

