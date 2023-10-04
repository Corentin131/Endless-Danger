using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceOnStart : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 velocity;

    void Update()
    {
        rb.velocity = velocity;
    }
}
