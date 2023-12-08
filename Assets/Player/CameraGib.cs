using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraGib : MonoBehaviour
{

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    public Vector3 pos = Vector2.zero;

    public bool xLock = false;
    public float lockPos = 0f;

    public float ylock = -1.1f;
    public float xlock = 14.1f;

    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0) 
        { 
            player1 = players[0];
            if (players.Length > 1)
            {
            player2 = players[1];
            }
        }
    }

    private void FixedUpdate()
    {
        if (player1 == null)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                player1 = players[0];
            }
        }
        if (player2 == null || player1 == player2)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 1)
            {
                player2 = players[1];
            }
            else 
            {
                player2 = player1;
            }
        }

        if (player1 != null && !player2 != null)
        {

            Vector3 targetPos = (player1.transform.position + player2.transform.position) / 2;

            if (xLock) targetPos.x = lockPos;
            lockPos = targetPos.x;
            targetPos.z = -1;
            if (targetPos.y < ylock) targetPos.y = ylock;
            if (targetPos.x < xlock) targetPos.x = xlock;

            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
