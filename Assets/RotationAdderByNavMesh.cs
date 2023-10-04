using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class RotationAdderByNavMesh : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Vector3 rotationsAxes;
    [SerializeField] float maxRotation;

    Vector3 startRotation;

    void Start()
    {
        startRotation = transform.eulerAngles;
    }

    void Update()
    {
        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        float rotation = speed / navMeshAgent.speed * Mathf.Abs(maxRotation); // Utilisez Mathf.Abs pour obtenir la valeur absolue

        float x = (startRotation.x + rotation) * rotationsAxes.x;
        float y = (startRotation.y + rotation) * rotationsAxes.y;
        float z = (startRotation.z + rotation) * rotationsAxes.z;

        if (rotationsAxes.x == 0) x = transform.eulerAngles.x;
        if (rotationsAxes.y == 0) y = transform.eulerAngles.y;
        if (rotationsAxes.z == 0) z = transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(x, y, z);
    }
}
