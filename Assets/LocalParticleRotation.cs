using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalParticleRotation : MonoBehaviour
{
    ParticleSystem particleSystem;
    ParticleSystem.MainModule main;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        main = particleSystem.main;
    }

    // Update is called once per frame
    void Update()
    {
        main.startRotation = gameObject.transform.rotation.eulerAngles.y+270;
        Debug.Log( gameObject.transform.rotation.eulerAngles.y+270);
    }
}
