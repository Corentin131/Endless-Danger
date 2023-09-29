using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [Header("Spawn Data")]
    public Transform header;
    public Transform gunSpawnPosition;

    [HideInInspector] public GameObject currentBaseObject;
    [HideInInspector] public GameObject currentSawObject;
    [HideInInspector] public GameObject currentGunObject;

    [HideInInspector] public Base currentBaseScript;
    [HideInInspector] public Saw currentSawScript;
    [HideInInspector] public Gun currentGunScript;

    [HideInInspector]public BaseData currentBaseData;
    [HideInInspector]public SawData currentSawData;
    [HideInInspector]public GunData currentGunData;

    PlayerMovement playerMovementScript;

    void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        BaseData baseData = PlayerStats.instance.currentBase;
        SawData sawData = PlayerStats.instance.currentSaw;
        GunData gunData = PlayerStats.instance.currentGun;

        if (sawData != currentSawData)
        {
            //Initialize Saw
            if (currentSawObject != null)
            {
                currentSawScript.DestroyObject();
                Destroy(currentSawObject);
            }
            currentSawData = sawData;
            currentSawObject = Instantiate(sawData.prefab,transform.position,Quaternion.identity,header);
            
            currentSawScript = currentSawObject.GetComponent<Saw>();
            currentSawScript.playerMovementScript = playerMovementScript;
            currentSawScript.skinManagerScript = this;
        }

        if (gunData != currentGunData)
        {
            //Initialize Gun
            if (currentGunObject != null)
            {
                currentGunScript.DestroyObject();
                Destroy(currentGunObject);
            }

            currentGunData = gunData;
            currentGunObject = Instantiate(gunData.prefab,gunSpawnPosition.position,Quaternion.identity,header);

            currentGunScript = currentGunObject.GetComponent<Gun>();
            currentGunScript.playerMovementScript = playerMovementScript;
            currentGunScript.skinManagerScript = this;
        }

        if (baseData != currentBaseData)
        {
            //Initialize Base
            if (currentBaseObject != null)
            {
                currentBaseScript.DestroyObject();
                Destroy(currentBaseObject);
            }

            currentBaseData = baseData;

            currentBaseObject = Instantiate(baseData.prefab,transform.position,Quaternion.identity,header);

            currentBaseScript = currentBaseObject.GetComponent<Base>();
            currentBaseScript.playerMovementScript = playerMovementScript;
            currentBaseScript.skinManagerScript = this;
        }
    }
}
