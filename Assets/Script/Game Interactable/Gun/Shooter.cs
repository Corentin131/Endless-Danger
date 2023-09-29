using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Serializable]
    public enum TypeOfShoot
    {
        FastShoot,
        PhysicShoot
    }
    public TypeOfShoot typeOfShoot;

    [Header("Sources")]
    public GameObject ball;
    public List<ParticleSystem> onShootParticles;
    public List<Animator> onShootAnimations;

    [Header("Data")]
    public float ballSpeed;
    public float distanceBeforeDestroy;

    Vector3 direction;
    PhysicBall currentBall;

    void Start()
    {
        if (typeOfShoot == TypeOfShoot.PhysicShoot)
        {
            ChargePhysicObject();
        }
    }

    void Update()
    {
        direction = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position,direction*distanceBeforeDestroy,Color.red);
    }

    public void Shoot()
    {
        switch (typeOfShoot)
        {
            case TypeOfShoot.FastShoot:
                ShootByRaycast();
                 break;

            case TypeOfShoot.PhysicShoot:
                ShootPhysicObject();
                break;
        }
    }

    void ShootByRaycast()
    {
        PlayVfx();
        RaycastHit hit;
        DamageManager damageManager;

        GameObject ballInstantiated = Instantiate(ball,transform.position,transform.rotation);
        FastBallManager fastBallManager = ballInstantiated.GetComponent<FastBallManager>();

        fastBallManager.speed = ballSpeed;
        fastBallManager.startPosition = transform.position;

        if (Physics.Raycast(transform.position, direction, out hit, distanceBeforeDestroy))
        {
            damageManager = hit.transform.gameObject.GetComponent<DamageManager>();

            if (damageManager != null)
            {
                damageManager.ApplyGunDamage(hit.point);
            }

            fastBallManager.destroyDistance = Vector3.Distance(transform.position,hit.point);
        }
        else
        {
            fastBallManager.destroyDistance = distanceBeforeDestroy;
        }
    }

    void PlayVfx()
    {
        foreach(ParticleSystem particleSystem in onShootParticles)
        {
            particleSystem.Play();
            
        }

        foreach(Animator animator in onShootAnimations)
        {
            animator.SetTrigger("Shoot");
        }
    }
    void ShootPhysicObject()
    {
        PlayVfx();
        currentBall.Shoot();
    }

    public void ChargePhysicObject()
    {
        currentBall = Instantiate(ball,transform.position,transform.rotation,transform.parent).GetComponent<PhysicBall>();
        currentBall.Charge();
    }


}
