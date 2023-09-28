using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Loader : MonoBehaviour
{
    [Header("Source")]
    public Slider slider;

    [HideInInspector]public bool isCharging;

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void Charge(float to , float time)
    {
        StartCoroutine(Charging(to,time));
    }

    IEnumerator Charging(float to , float time)
    {
        float updateRechargeTime = 0.01f;
        float toAdd = to/(time/updateRechargeTime);
        isCharging = true;

        while (slider.value <= to-1)
        {
            slider.value += toAdd;;
            yield return new WaitForSeconds(updateRechargeTime);
        }
        isCharging = false;
    }
}
