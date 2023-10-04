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
    public List<PowerData> powerDatas = new List<PowerData>(){};
    public GameObject powerSelectorBar;
    public Transform powerSelector;

    //public GameObject prefabBackgroundEffect;
    //public Transform prefabBackgroundEffectHeader;
    public GameObject loaderBar;

    [HideInInspector]public int power;
    [HideInInspector]public Action<int> onPowerChange;
    [HideInInspector]public Action OnStartRecharge;
    [HideInInspector]public Action OnStopRecharge;

    GameObject currentPowerImage;
    CurrentState currentState;

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
        currentState = new ChoosingPowerState(this);

        PlayerActions.instance.onScroll += ChangePowerBySroll;
        SetValue(1);
        StartCoroutine(Shaking());
        //StartCoroutine(BackgroundEffect());
    }

    void Update()
    {
        currentState.ExecuteState();
    }

    [Serializable]
    public class PowerData
    {
        public GameObject powerImage;
        public TextManager textManager;
        public float sizeOnSelect;
        public int numberOfShoot;
    }

    public void ChangePowerBySroll(float direction)
    {
        int newPower = power+Convert.ToInt32(direction);
        if (newPower > 3) newPower = 1;
        if (newPower < 1) newPower = 3;

        if (currentState.canChange == true)
        {
            StartCoroutine(WaitBeforeChange());
            SetValue(newPower);
        }
    }

    public void SetValue(int value)
    {
        currentState.SetValue(value);
    }

    public void Recharge(float time)
    {
        currentState.GetOut();
        currentState = new RechargeState(this,time);
    }

    public void SetNbOfShoot(int power , int nbOfShoot)
    {
        powerDatas[power-1].numberOfShoot = nbOfShoot;
        powerDatas[power-1].textManager.SetTextInt(nbOfShoot);
    }

    IEnumerator WaitBeforeChange()
    {
        currentState.canChange = false;
        yield return new WaitForSeconds(0.2f);
        currentState.canChange = true;
    }

    IEnumerator Shaking()
    {
        while (true)
        {
            if (power == 3)
            {
                StartCoroutine(Utilities.ShakeINumerator(0.2f,1.5f,currentPowerImage.transform,transform.localPosition));
                yield return new WaitForSeconds(0.2f);
            }
            yield return null;
        }
    }
    /*
    IEnumerator BackgroundEffect()
    {
        while (true)
        {
            Instantiate(prefabBackgroundEffect,currentPowerImage.transform.position,currentPowerImage.transform.rotation,prefabBackgroundEffectHeader);
            yield return new WaitForSeconds(0.5f);
        }
    }
    */

    class CurrentState
    {
        public bool canChange = true;
        public PowerBar powerBar;
        public virtual void ExecuteState()
        {

        }

        public virtual void SetValue(int value)
        {
        }

        public virtual void ChangePowerBySroll(float direction)
        {

        }

        public virtual void GetOut()
        {

        }
        public virtual void GetIn()
        {

        }
    }

    class ChoosingPowerState : CurrentState
    {

        AnimationPack animationPack;

        public ChoosingPowerState(PowerBar powerBar)
        {
            this.powerBar = powerBar;
            animationPack = powerBar.powerSelectorBar.GetComponent<AnimationPack>();

            if (powerBar.currentState != null)
            {
                animationPack.MoveTo(new Vector3(0,0,0),800,0.2f,LeanTweenType.easeInQuad);
            }
        }

        public override void ExecuteState()
        {
            Vector3 targetPosition = new Vector3(powerBar.currentPowerImage.transform.position.x,powerBar.powerSelector.transform.position.y,powerBar.powerSelector.transform.position.z);
            powerBar.powerSelector.position = Vector3.Lerp(powerBar.powerSelector.transform.position,targetPosition,0.1f);
            base.ExecuteState();
        }

        public override void GetOut()
        {
            animationPack.MoveTo(new Vector3(0,-120,0),800,0.2f,LeanTweenType.easeInQuad);

        }

        public override void SetValue(int value)
        {
            GameObject targetPowerImage = null;
            AnimationPack animationPackForNewImage;
            AnimationPack animationPackForOldImage;
            float size = 0.3f;

            if (value != 0)
            {
                PowerData powerData = powerBar.powerDatas[value-1];
                size = powerData.sizeOnSelect;
                targetPowerImage = powerData.powerImage;
                powerData.textManager.SetTextInt(powerData.numberOfShoot);
            }

            if (targetPowerImage != null && powerBar.currentPowerImage != targetPowerImage)
            {
                animationPackForNewImage = targetPowerImage.GetComponent<AnimationPack>();
                
                Vector3 scaleLevel = new Vector3(size,size,size);

                animationPackForNewImage.ScaleUp(scaleLevel,0.1f);

                if (powerBar.currentPowerImage != powerBar.gameObject)
                {
                    animationPackForOldImage = powerBar.currentPowerImage.GetComponent<AnimationPack>();
                    animationPackForOldImage.ScaleToOrigin(0.1f);
                }

                powerBar.power = value;
                powerBar.currentPowerImage = targetPowerImage;
                
                if (powerBar.onPowerChange != null)
                {
                    powerBar.onPowerChange(value);
                }
                //Instantiate(prefabBackgroundEffect,currentPowerImage.transform.position,currentPowerImage.transform.rotation,prefabBackgroundEffectHeader);
            }
        }
    }

    class RechargeState : CurrentState
    {
        Loader loader;
        AnimationPack animationPack;
        float time;
        float animationTime = 0.2f;

        public RechargeState(PowerBar powerBar,float time)
        {
            this.powerBar = powerBar;
            this.time = time;
            this.loader = powerBar.loaderBar.GetComponent<Loader>();
            this.animationPack = powerBar.loaderBar.GetComponent<AnimationPack>();
            animationPack.MoveTo(new Vector3(0,0,0),800,animationTime);
            loader.SetValue(0);
            loader.Charge(100,time);
            Utilities.VerifyIfNull(powerBar.OnStartRecharge);
        }

        public override void ExecuteState()
        {
            if (loader.isCharging == false)
            {
                GetOut();
                powerBar.currentState = new ChoosingPowerState(powerBar);
                
                Utilities.VerifyIfNull(powerBar.OnStopRecharge);
            }
        }

        public override void GetOut()
        {
            animationPack.MoveTo(new Vector3(0,-85,0),800,animationTime);
        }
    }


}
