using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchHitBox : MonoBehaviour
{
    public float Timer = 0.0f;
    public float deathTimer = 0.15f;
    private void Awake()
    {
        Timer = 0.0f;
    }

    private void FixedUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer > deathTimer)
        {
            this.gameObject.SetActive(false);
            Timer = 0.0f;
        }
    }
}