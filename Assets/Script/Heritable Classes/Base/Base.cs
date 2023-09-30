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
    public float startRotation;
    public float rotationToAdd;

    [Header("Fight Parameters")]
    public float powerMaxDistance;
    
    PowerData currentPowerData;
    
    [Serializable]
    public class PowerData
    {
        public int power;
        public float damage;
        public float shakingTime;
        public float shakingPower;
        public float machineSpeed;
        public float cameraSpeed;
        public float delayToRecharge;
        public int nbOfShoot;
        public List<Transform> vfxToSwitch;
        public List<GameObject> objectsToActivate;
        public int currentNbOfShoot;
    }

    public override void Start()
    {
        PlayerActions.instance.onTargeterInputHolding += OnTargeterInputHolding;

        powerMaxDistance = PlayerStats.instance.currentBase.powerMaxDistance;

        base.Start();

        currentPowerData = GetPowerData(1);
        float rotation = startRotation+(skinManagerScript.currentGunScript.transform.eulerAngles.y);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,rotation,transform.eulerAngles.y);

        //Initialize de nb of shoot of the PowerBar
        foreach(PowerData powerData in powerDatas)
        {
            PowerBar.instance.SetNbOfShoot(powerData.power,powerData.nbOfShoot);
            powerData.currentNbOfShoot = powerData.nbOfShoot;
        }
    }

    public override void DestroyObject()
    {
        base.DestroyObject();
        PlayerActions.instance.onTargeterInputHolding -= OnTargeterInputHolding;
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

        PowerData powerData = GetPowerData(PowerBar.instance.power);
        powerData.currentNbOfShoot --;

        if (powerData.currentNbOfShoot <= 0)
        {
            PowerBar.instance.Recharge(GetPowerData( PowerBar.instance.power).delayToRecharge);
            PowerBar.instance.SetNbOfShoot(powerData.power,powerData.nbOfShoot);

            OnPowerChange(0);
            powerData.currentNbOfShoot = powerData.nbOfShoot;

            playerMovementScript.Freeze();
        }else
        {
            PowerBar.instance.SetNbOfShoot(powerData.power,powerData.currentNbOfShoot);
        }
    }

    public override void OnStopRecharge()
    {
        OnPowerChange(PowerBar.instance.power);
        playerMovementScript.Unfreeze();
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

            }

            if (power != 0)
            {
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
        Debug.Log($"Direction 2 : {direction}");
        if (damageManagerScript != null)
        {
            damageManagerScript.ApplyBaseDamage(GetPowerData(PowerBar.instance.power).damage,vfxPosition,Quaternion.LookRotation(direction, Vector3.up),direction);
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
