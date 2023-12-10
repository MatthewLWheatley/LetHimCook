using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowChar : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;

    /// <summary>
    /// 0 = waiting, 1 = moving, 2= attacking, 3 = stunned
    /// </summary>
    public int state = 0;
    [SerializeField] private float delayTimer = 2.0f;
    [SerializeField] private float floatingDistantce = 1.5f;

    private GameObject player1;
    private GameObject player2;
    private GameObject TargetPlayer;
    
    [SerializeField] private Vector3 targetPos = Vector3.zero;

    public float xSpeed = 1.0f;
    public float ySpeed = 1.0f;

    public float timer = 0;

    private void Start()
    {
        Debug.Log("fuck");
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0) player1 = players[0];
        if (players.Length > 1) player2 = players[1];
        if (player1 != null)
            if (((player1.transform.position - this.transform.position) / 2).magnitude < ((player2.transform.position - this.transform.position) / 2).magnitude)
            {
                TargetPlayer = player1;
            }
            else
            {
                TargetPlayer = player2;
            }
    }


    private void Update()
    {
        if (player1 != null)
        {
            Debug.Log("test");
            TargetPlayer = player1;
            if (player2 == null)
            {
                player2 = player1;

            }
            if (player1 != player2) 
            {
                if (((player1.transform.position - this.transform.position) / 2).magnitude > ((player2.transform.position - this.transform.position) / 2).magnitude)
                {
                    TargetPlayer = player1;
                }
                else
                {
                    TargetPlayer = player2;
                }
            }

            timer += Time.deltaTime;

            if (state == 0)
            {
                if (timer > delayTimer)
                {
                    Vector2 pos = this.transform.position;
                    Vector2 pos2 = TargetPlayer.transform.position;

                    //Debug.Log($"{Vector2.Distance(new Vector2(pos.x, pos.y), pos2)}");
                    if (Vector2.Distance(new Vector2(pos.x, pos.y), pos2) >= floatingDistantce + 0.1f)
                    {
                        state = 1;
                        timer = 0;
                    }
                    else
                    {
                        //Debug.Log("bal");
                        state = 2;
                        timer = 0;
                    }

                }
            }
            if (state == 1)
            {
                float xStep = xSpeed * Time.deltaTime;
                float yStep = ySpeed * Time.deltaTime;

                if (((TargetPlayer.transform.position + new Vector3(-floatingDistantce, 0, 0)) / 2).magnitude < (((TargetPlayer.transform.position + new Vector3(floatingDistantce, 0, 0))) / 2).magnitude)
                {
                    targetPos = (TargetPlayer.transform.position + new Vector3(-floatingDistantce, 0, 0));
                }
                else
                {
                    targetPos = (TargetPlayer.transform.position + new Vector3(floatingDistantce, 0, 0));
                }

                Vector3 temp = Vector3.zero;
                temp.y = Vector3.MoveTowards(new Vector3(0, this.transform.position.y, 0), new Vector3(0, targetPos.y, 0), yStep).y;
                temp.x = Vector3.MoveTowards(new Vector3(this.transform.position.x, 0, 0), new Vector3(targetPos.x, 0, 0), xStep).x;
                temp.z = 0;
                this.transform.position = temp;

                if (targetPos.x == this.transform.position.x && targetPos.y == this.transform.position.y)
                {
                    timer = 0;
                    state = 0;
                }
            }
            if (state == 2)
            {
                Debug.Log("Attacking ");



                state = 0;
                timer = 0;
            }
        }

    }
}

