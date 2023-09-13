using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("Source")]
    public List<GameObject> gunImpactEffects;
    public List<GameObject> baseDamageEffects;


    [Header("Data")]
    public float shakingTime;
    public float shakingPower;
    
    public virtual void ApplyGunDamage(Vector3 position)
    {
        if (gunImpactEffects != null)
        {
            foreach(GameObject effect in gunImpactEffects)
            {
                Instantiate(effect,position,transform.rotation);
            }

            PrincipalCamera.instance.Shake(shakingTime,shakingPower);

           
        }  
    }

    public virtual void ApplyBaseDamage(Vector3 position,Quaternion rotation ,[Optional]List<GameObject> effectToSpawn)
    {
        List<GameObject> effects;
        
        if (effectToSpawn != null)
        {
            effects = effectToSpawn;
        }else
        {
            effects = baseDamageEffects;
            Debug.Log("Hit");
        }

        foreach (GameObject objectToSpawn in effects)
        {
            Instantiate(objectToSpawn,position,rotation);
        }
    }
}
