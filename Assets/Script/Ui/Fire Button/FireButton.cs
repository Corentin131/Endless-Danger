using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class FireButton : MonoBehaviour
{
    public static FireButton instance;

    [Header("Source")]
    public Image rechargeBar;

    [HideInInspector]public float currentBullet;
    [HideInInspector]public int maxBullet;
    [HideInInspector]public float timeToRecharge;
    [HideInInspector]public bool isCharging = false;

    [HideInInspector]public bool shoot = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Debug.Log("Instance of PlayerStats is already instantiate");
        }
    }

    private void Start() 
    {
        PlayerStats.instance.onCurrentGunChange += OnGunChange;
        PlayerActions.instance.onFireInputDawn +=  StartFire;
        PlayerActions.instance.onFireInputUp += StopFire;
        StartCoroutine(ActivateShooting());
    }

    void Update()
    {
        rechargeBar.fillAmount = currentBullet/maxBullet;
        /*
        if (isCharging == false && currentBullet <= 0)
        {
            Recharge();
        }
        */
    }

    public void SubtractBullet(int howMany)
    {
        currentBullet -= howMany;
    }

    public void StartFire()
    {
        shoot = true;
    }

    public void StopFire()
    {
        shoot = false;
    }

    public void Recharge()
    {
        StartCoroutine(Recharging());
    }


    void OnGunChange(GunData gunData)
    {
        maxBullet = gunData.numberOfBullet;
        currentBullet = maxBullet;
        timeToRecharge = gunData.timeToRecharge;
    }

    IEnumerator ActivateShooting()
    {
        while (true)
        {
            if (shoot == true)
            {
                PlayerActions.instance.Fire();
            }
            yield return null;
        }
    }

    IEnumerator Recharging()
    {
        float updateRechargeTime = 0.01f;
        float toAdd = maxBullet/(timeToRecharge/updateRechargeTime);
        isCharging = true;

        while (currentBullet <= maxBullet)
        {
            currentBullet += toAdd;
            yield return new WaitForSeconds(updateRechargeTime);
        }

        isCharging = false;
        currentBullet = Convert.ToInt32(currentBullet);
    }
}
