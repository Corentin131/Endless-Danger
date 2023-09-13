using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : CustomableObject
{

    [Header("Sources")]
    public Shooter shooter;

    [Header("Data")]
    public float delay;
    public float rotationToAdd = 90;
    public float shakePower;
    public float shakeDuration;
    bool canShoot = true;

    public override void Start()
    {
        PlayerActions.instance.onFireInputDawn += Shoot;
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
           shooter.Shoot();
           PrincipalCamera.instance.Shake(shakeDuration,shakePower);
           StartCoroutine(Delay());
           FireButton.instance.SubtractBullet(1);
        }
        
    }

    IEnumerator Delay()
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
