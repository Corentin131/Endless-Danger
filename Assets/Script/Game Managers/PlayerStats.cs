using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Unlocked data")]
    public BaseData[] unlockedBase;
    public SawData[] unlockedSaw;
    public GunData[] unlockedGun;

    [Header("Info")]
    public BaseData currentBase;
    public SawData currentSaw;
    public GunData currentGun;

    public Action<BaseData> onCurrentBaseChange;
    public Action<SawData> onCurrentSawChange;
    public Action<GunData> onCurrentGunChange;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }else
        {
            Debug.Log("Instance of PlayerStats is already instantiate");
        }
    }

    void Start()
    {
        ChangeBase(currentBase);
        ChangeSaw(currentSaw);
        ChangeGun(currentGun);
    }

    public void ChangeBase(BaseData baseData)
    {
        currentBase = baseData;
        if (onCurrentBaseChange != null)
        {
            onCurrentBaseChange(baseData);
        }
    }

    public void ChangeSaw(SawData sawData)
    {
        currentSaw = sawData;
        if (onCurrentSawChange != null)
        {
            onCurrentSawChange(sawData);
        }
    }

    public void ChangeGun(GunData gunData)
    {
        currentGun = gunData;

        if(onCurrentGunChange != null)
        {
            onCurrentGunChange(gunData);
        }
    }

}
