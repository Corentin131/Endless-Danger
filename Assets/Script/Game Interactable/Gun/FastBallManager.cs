using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FastBallManager : MonoBehaviour
{
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float destroyDistance;
    [HideInInspector] public float speed;

    void Update()
    {
        transform.Translate(transform.TransformDirection(-Vector3.forward)*speed*Time.deltaTime);

        float distance = Vector3.Distance(transform.position,startPosition);

        if (distance > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
