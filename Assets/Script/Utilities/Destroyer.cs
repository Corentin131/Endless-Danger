using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [Header("Data")]
    public float delay;
    public bool startCountdownOnStart = true;

    void Start()
    {
        if (startCountdownOnStart)
        {
            StartCoroutine(DelayDestroy());
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
