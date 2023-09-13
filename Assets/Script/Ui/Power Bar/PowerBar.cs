using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public static PowerBar instance;

    [Header("Sources")]
    public GameObject lowPowerImage;
    public GameObject middlePowerImage;
    public GameObject maxPowerImage;
    public Transform powerSelector;
    public GameObject prefabBackgroundEffect;
    public Transform prefabBackgroundEffectHeader;

    [HideInInspector]public int power;
    [HideInInspector]public Action<int> onPowerChange;

    GameObject currentPowerImage;
    bool canChange = true;
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
    
    
    void Start()
    {
        currentPowerImage = gameObject;
        PlayerActions.instance.onScroll += ChangePowerBySroll;
        SetValue(1);
        StartCoroutine(Shaking());
        //StartCoroutine(BackgroundEffect());
    }

    void Update()
    {
        Vector3 targetPosition = new Vector3(currentPowerImage.transform.position.x,powerSelector.transform.position.y,powerSelector.transform.position.z);
        powerSelector.position = Vector3.Lerp(powerSelector.transform.position,targetPosition,0.1f);
    }

    public void ChangePowerBySroll(float direction)
    {
        int newPower = power+Convert.ToInt32(direction);
        if (newPower > 3) newPower = 1;
        if (newPower < 1) newPower = 3;

        if (canChange == true)
        {
            StartCoroutine(WaitBeforeChange());
            SetValue(newPower);
        }
    }

    public void SetValue(int value)
    {
        GameObject targetPowerImage = null;
        AnimationPack animationPackForNewImage;
        AnimationPack animationPackForOldImage;
        float size = 0.5f;
        
        if (value == 1)
        {
            targetPowerImage =  lowPowerImage;
            size = 0.3f;
        }
        else if (value == 2)
        {
            targetPowerImage =  middlePowerImage; 
            size = 0.3f;
        }
        else if (value == 3)
        {
            targetPowerImage = maxPowerImage;
            size = 0.5f;
        }


        if (targetPowerImage != null && currentPowerImage != targetPowerImage)
        {
            animationPackForNewImage = targetPowerImage.GetComponent<AnimationPack>();
            
            Vector3 scaleLevel = new Vector3(size,size,size);

            animationPackForNewImage.ScaleUp(scaleLevel,0.1f);

            if (currentPowerImage != gameObject)
            {
                animationPackForOldImage = currentPowerImage.GetComponent<AnimationPack>();
                animationPackForOldImage.ScaleToOrigin(0.1f);
            }

            power = value;
            currentPowerImage = targetPowerImage;
            
            if (onPowerChange != null)
            {
                onPowerChange(value);
            }
            //Instantiate(prefabBackgroundEffect,currentPowerImage.transform.position,currentPowerImage.transform.rotation,prefabBackgroundEffectHeader);
        }
    }


    IEnumerator WaitBeforeChange()
    {
        canChange = false;
        yield return new WaitForSeconds(0.2f);
        canChange = true;
    }

    IEnumerator Shaking()
    {
        while (true)
        {
            if (currentPowerImage == maxPowerImage)
            {
                StartCoroutine(Utilities.ShakeINumerator(0.2f,1.5f,maxPowerImage.transform));
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
        }
    }

    IEnumerator BackgroundEffect()
    {
        while (true)
        {
            Instantiate(prefabBackgroundEffect,currentPowerImage.transform.position,currentPowerImage.transform.rotation,prefabBackgroundEffectHeader);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
