using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            cam.GetComponent<CameraGib>().xLock = true;
        }
        //Debug.Log(collision.gameObject.ToString());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) 
        {
            cam.GetComponent<CameraGib>().xLock = false;
            this.gameObject.SetActive(false);
        }
    }
}
