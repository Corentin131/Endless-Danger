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
    [HideInInspector]public bool canShoot = true;
    [HideInInspector]public int index = 0;

    public override void Start()
    {
        PlayerActions.instance.onFireInputHolding += Shoot;
        base.Start();
    }

    public virtual void RotateTo(Quaternion rotation)
    {
        transform.rotation = rotation;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y+rotationToAdd, 0);
    }

    public virtual void Shoot()
    {
        if (canShoot &&  FireButton.instance.isCharging == false)
        {
            
           shooters[index].Shoot();
           PrincipalCamera.instance.Shake(shakeDuration,shakePower);
           StartCoroutine(Delay());
           FireButton.instance.SubtractBullet(1);
        }
        
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
}
