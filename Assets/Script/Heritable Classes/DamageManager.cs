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
    
    [Serializable]
    public struct BaseVfxData
    {  
        [Range(1,3)]
        public int power;
        public List<GameObject> effects;
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
    }

    public virtual void ApplyExplosiveDamage(Vector3 position,float damage)
    {
        Debug.Log(transform + " "+damage);
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
