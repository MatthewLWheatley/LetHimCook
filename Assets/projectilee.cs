using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectilee : MonoBehaviour
{
    public class ProjectileMovement : MonoBehaviour
    {
        public float speed = 10f; // Adjust the speed as needed.
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * speed;
        }
    }
}
