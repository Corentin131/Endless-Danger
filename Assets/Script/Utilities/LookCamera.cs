using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation =Quaternion.Euler(Camera.main.transform.eulerAngles.x,transform.eulerAngles.y,Camera.main.transform.eulerAngles.z);
    }
}
