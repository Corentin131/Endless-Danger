using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Base : CustomableObject
{
    
    [HideInInspector] public int power;

    [Header("Sources")]
    public Transform transitVfxHeader;
    public List<PowerData> powerDatas;
    
    [Header("Data")]
    public float rotationToAdd;

    [Header("Fight Parameters")]
    public float powerMaxDistance;
    
    PowerData currentPowerData;
    
    [Serializable]
    public struct PowerData
    {
        public int power;
        public float shakingTime;
        public float shakingPower;
        public float machineSpeed;
        public float cameraSpeed;
        public List<Transform> vfxToSwitch;

        public List<GameObject> objectsToActivate;
    }

    public override void Start()
    {
        PlayerActions.instance.onTargeterInputHolding += OnTargeterInputHolding;

        powerMaxDistance = PlayerStats.instance.currentBase.powerMaxDistance;

        base.Start();

        currentPowerData = GetPowerData(1);
    }

    public override void OnTargeterInputHolding(Vector3 mousePosition)
    {
        Vector3 playerPosition = transform.position;
        Vector3 direction = Utilities.CalculateDirection(transform.position , mousePosition);

        float distance = Vector3.Distance(playerPosition,mousePosition);

        float calculatedPower = (distance/powerMaxDistance)*100;
        
        if (calculatedPower <= 30)
        {
            power = 1;
        }
        else if (calculatedPower <= 60)
        {
            power = 2;
        }
        else if (calculatedPower <= 100)
        {
            power = 3;
        }

        //PowerBar.instance.SetValue(power);
    }

    public override void OnStartTransitState()
    {
        Utilities.VFXSwitch(transitVfxHeader,true);
        PowerBar.instance.SetValue(0);
    }

    public override void OnStopTransitState()
    {
        Utilities.VFXSwitch(transitVfxHeader,false);
    }

    public override void OnPowerChange(int power)
    {
        foreach(PowerData powerData in powerDatas)
        {
            bool Switch = false;

            if (powerData.power == power)
            {
                Switch = true;
                playerMovementScript.speed = powerData.machineSpeed;
                currentPowerData = powerData;
            }
            else
            {
                Switch = false;
            }

            foreach (Transform transform in powerData.vfxToSwitch)
            {
                Utilities.VFXSwitch(transform,Switch);

                foreach (GameObject gameObject in powerData.objectsToActivate)
                {
                    gameObject.SetActive(Switch);
                }
                
            }

            if (power == 3)
            {
                PrincipalCamera.instance.Shake(powerData.shakingTime,powerData.shakingPower);
            }

        }
    }

    public override void OnHitTransform(Transform transform,Vector3 playerPosition, Vector3 vfxPosition,Vector3 direction)
    {
        DamageManager damageManagerScript = transform.GetComponent<DamageManager>();

        if (damageManagerScript != null)
        {
            damageManagerScript.ApplyBaseDamage(vfxPosition,Quaternion.LookRotation(direction, Vector3.up));
        }

        PrincipalCamera.instance.Shake(currentPowerData.shakingTime,currentPowerData.shakingPower);

        
        PrincipalCamera.instance.MoveFast(playerPosition,currentPowerData.cameraSpeed);
    }
    
    public void RotateTo(Quaternion rotation)
    {
        transform.rotation = rotation;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y+rotationToAdd, 0);
    }

    PowerData GetPowerData(int power)
    {
        foreach (PowerData powerData in powerDatas)
        {
            if (powerData.power == power)
            {
                return powerData;
            }
        }
        return new PowerData();
    }

}
