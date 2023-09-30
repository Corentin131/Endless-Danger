using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : CustomableObject
{

    [Header("Sources")]
    public List<Shooter> shooters;

    [Header("Data")]
    public float delay;
    public float rotationToAdd = 90;
    public float shakePower;
    public float shakeDuration;
    public bool interactWithFireButton = true;
    [HideInInspector]public bool canShoot = true;
    [HideInInspector]public int index = 0;
    [HideInInspector]public bool isCharging;
    public float maxBullet;
    public float rechargeTime;
    [HideInInspector]public float currentBullet;
    

    public override void Start()
    {
        if (interactWithFireButton == true)
        {
            PlayerActions.instance.onFireInputHolding += Shoot;
        
            FireButton.instance.maxBullet = skinManagerScript.currentGunData.numberOfBullet;
            FireButton.instance.currentBullet = skinManagerScript.currentGunData.numberOfBullet;
            FireButton.instance.timeToRecharge = skinManagerScript.currentGunData.timeToRecharge;
        
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,skinManagerScript.currentBaseScript.transform.eulerAngles.y,transform.eulerAngles.z);
            base.Start();
            maxBullet = skinManagerScript.currentGunData.numberOfBullet;
            rechargeTime = skinManagerScript.currentGunData.timeToRecharge;
        }
        currentBullet = maxBullet;
    }

    public virtual void RotateTo(Quaternion rotation)
    {
        transform.rotation = rotation;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y+rotationToAdd, 0);
    }

    public virtual void Shoot()
    {
        if (canShoot &&  isCharging == false)
        {
                
            shooters[index].Shoot();
            PrincipalCamera.instance.Shake(shakeDuration,shakePower);

            StartCoroutine(Delay());

            currentBullet -= 1;

            if (interactWithFireButton == true)
            {
                FireButton.instance.SubtractBullet(1);
            }
        }

        if (currentBullet <= 0)
        {
            StartCoroutine(Recharging());

        }
        
        
    }

    public override void DestroyObject()
    {
        base.DestroyObject();
        PlayerActions.instance.onFireInputHolding -= Shoot;
    }

    IEnumerator Delay()
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);

        index +=1;

        if (index > shooters.Count-1)
        {
            index = 0;
        }
        canShoot = true;
    }

    IEnumerator Recharging()
    {
        float updateRechargeTime = 0.01f;
        float toAdd = maxBullet/(rechargeTime/updateRechargeTime);
        canShoot = false;

        if (interactWithFireButton == true){}
            FireButton.instance.isCharging = true;

        isCharging = true;

        while (currentBullet <= maxBullet)
        {
            currentBullet += toAdd;
            FireButton.instance.currentBullet += toAdd;
            yield return new WaitForSeconds(updateRechargeTime);
        }
        
        canShoot = true;
        index = 0;
        currentBullet = (float)Math.Floor(currentBullet);
        if (interactWithFireButton == true){}
            FireButton.instance.isCharging = false;

        isCharging = false;
    }
}
