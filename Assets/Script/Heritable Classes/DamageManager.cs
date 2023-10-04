using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("Source")]
    public List<GameObject> gunImpactEffects;

    [Header("Data")]
    public List<BaseVfxData> baseDamageEffects;
    [SerializeField] List<AnimationData> animationToPlayOnDamage;
    [SerializeField] float cameraShakePower;
    [SerializeField] float cameraShakeTime;
    
    [Serializable]
    public struct BaseVfxData
    {  
        [Range(1,3)]
        public int power;
        public List<GameObject> effects;
    }

    [Serializable]
    struct AnimationData
    {
        public Animator animator;
        public bool isTrigger;
        public bool boolValue;
        public string animationName;
    }

    public virtual void ApplyGunDamage(float damage,Vector3 position,Vector3 direction)
    {
        if (gunImpactEffects != null)
        {
            foreach(GameObject effect in gunImpactEffects)
            {
                Instantiate(effect,position,transform.rotation);
            }
        }
        PrincipalCamera.instance.Shake(cameraShakeTime,cameraShakePower);
        PlayAnimations();
    }

    public virtual void ApplyBaseDamage(float damage,Vector3 position,Quaternion EffectSpawnRotation ,Vector3 direction,[Optional]List<GameObject> effectToSpawn)
    {
        List<GameObject> effects;

        if (effectToSpawn != null)
        {
            effects = effectToSpawn;
        }
        else
        {
            effects = GetVfxData(PowerBar.instance.power).effects;
        }

        if (effects != null)
        {
            foreach (GameObject objectToSpawn in effects)
            {
                Instantiate(objectToSpawn,position,EffectSpawnRotation);
            }
        }
        PrincipalCamera.instance.Shake(cameraShakeTime,cameraShakePower);
        PlayAnimations();
    }

    public virtual void ApplyExplosiveDamage(Vector3 position,float damage,float force,float radius)
    {
        PrincipalCamera.instance.Shake(cameraShakeTime,cameraShakePower);
        
        Debug.Log("Hit by an explosion ");
    }

    public void PlayAnimations()
    {
        foreach (AnimationData animationData in animationToPlayOnDamage)
        {
            if (animationData.isTrigger == true)
            {
                animationData.animator.SetTrigger(animationData.animationName);
            }else
            {
                animationData.animator.SetBool(animationData.animationName,animationData.boolValue);
            }
        }
    }

    BaseVfxData GetVfxData(int power)
    {
        foreach (BaseVfxData vfxData in baseDamageEffects)
        {
            if (vfxData.power == power)
            {
                return vfxData;
            }
        }

        return new BaseVfxData();
    }
}
