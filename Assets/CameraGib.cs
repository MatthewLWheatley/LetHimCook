using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraGib : MonoBehaviour
{

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    public Vector3 pos = Vector2.zero;

    public bool xLock = false;
    public float lockPos = 0f;

    public float ylock = -1.1f;

    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        
        if (!player2.activeSelf) 
        {
            player2 = player1;
        }
    }

    private void FixedUpdate()
    {
        if (player2.IsDestroyed())
        {
            player2 = player1;
        }
        if (player1.IsDestroyed())
        {
            player1 = player2;
        }
        Vector3 targetPos = (player1.transform.position + player2.transform.position) / 2;
        
        if (xLock) targetPos.x = lockPos;
        lockPos = targetPos.x;
        targetPos.z = -1;
        if (targetPos.y < ylock) targetPos.y = ylock;

        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPos, ref velocity, smoothTime);
    }
}