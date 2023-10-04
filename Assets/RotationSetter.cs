using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetter : MonoBehaviour
{
    [SerializeField] Vector3 rotation;
    void Update()
    {
        transform.eulerAngles = rotation;
    }
}
