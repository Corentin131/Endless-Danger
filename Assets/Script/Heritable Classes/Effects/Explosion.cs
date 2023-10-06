using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Source")]
    public float radius;

    [Header("Parameters")]
    public bool executeOperationOnStart;

    [Header("Data")]
    public float maxDamage;
    public float shakeDelay;
    public float shakePower;
    public float explosionForce;

    [Header("Out")]
    public List<GameObject> listOfHitObjects;


    Vector3 targetTouch;

    // Start is called before the first frame update
    void Start()
    {
        if (executeOperationOnStart)
        {
            ExecuteOperation();
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }

    public virtual void ExecuteOperation()
    {
        PrincipalCamera.instance.Shake(shakeDelay,shakePower);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(var collider in colliders) {
            listOfHitObjects.Add(collider.gameObject);

            DamageManager damageManager = collider.GetComponent<DamageManager>();

            if (damageManager != null)
            {
                float distanceTargetAndExplosion = Vector3.Distance(collider.transform.position,transform.position);
                targetTouch = collider.transform.position;
                float damage = maxDamage/distanceTargetAndExplosion;
                try
                {
                    damageManager.ApplyExplosiveDamage(transform.position,damage,explosionForce,radius);
                }catch(Exception e)
                {
                    Debug.Log(e);
                }
                
            }

            

        }
        
    }

}
