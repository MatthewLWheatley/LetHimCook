using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowChar : MonoBehaviour
{
    public GameObject GameManager;

    public GameObject player1;
    public GameObject player2;
    public GameObject TargetPlayer;
    
    public Vector3 targetPos = Vector3.zero;

    public float lerpSpeed = 1.0f;

    private void Awake()
    {
        GameObject[] players;
        players =GameObject.FindGameObjectsWithTag("Player");
        player1 = players[0];
        player2 = players[1];

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
        if (((player1.transform.position - this.transform.position) / 2).magnitude < ((player2.transform.position - this.transform.position) / 2).magnitude)
        {
            TargetPlayer = player1;
        }
        else
        {
            TargetPlayer = player2;
        }

        float step = lerpSpeed * Time.deltaTime;

        if (((TargetPlayer.transform.position + new Vector3(-1, 0, 0)) / 2).magnitude < (((TargetPlayer.transform.position + new Vector3(1, 0, 0))) / 2).magnitude)
        {
            targetPos = (TargetPlayer.transform.position + new Vector3(-1, 0, 0));
        }
        else
        {

            targetPos = (TargetPlayer.transform.position + new Vector3(1, 0, 0));

        }
        // Use Vector3.MoveTowards to move towards the target position at a constant speed.
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, step);
    }
}

