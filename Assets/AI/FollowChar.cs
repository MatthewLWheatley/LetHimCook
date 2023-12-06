using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowChar : MonoBehaviour
{
    public GameObject GameManager;

    public GameObject player1;
    public GameObject player2;
    public GameObject TargetPlayer;
    
    public Vector3 targetPos = Vector3.zero;


    private void Start()
    {
        player1 = GameManager.GetComponent<GameManager>().GetComponent<PlayerManager>().Player1;
        player2 = GameManager.GetComponent<GameManager>().GetComponent<PlayerManager>().Player1;
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
        this.transform.position = Vector3.Lerp(this.transform.position, TargetPlayer.transform.position, 0.5f);
    }
}
