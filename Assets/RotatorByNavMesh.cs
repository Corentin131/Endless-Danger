using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotatorByNavMesh : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speedMultiplier;
    [SerializeField] Vector3 rotationAxes;

    // Update is called once per frame
    void Update()
    {
        float speedNav = navMeshAgent.velocity.magnitude/navMeshAgent.speed;
        transform.Rotate(rotationAxes*speedMultiplier*speedNav*Time.deltaTime);
    }
}
