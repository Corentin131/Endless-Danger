using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool shootByRayCast = true;

    [Header("Sources")]
    public GameObject ball;
    public List<ParticleSystem> onShootParticles;
    public List<Animator> onShootAnimations;

    [Header("Data")]
    public float ballSpeed;
    public float distanceBeforeDestroy;

    Vector3 direction;

    void Update()
    {
        direction = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position,direction*distanceBeforeDestroy,Color.red);
    }

    public void Shoot()
    {
        if (shootByRayCast)
        {
            ShootByRaycast();
        }
    }

    void ShootByRaycast()
    {
        foreach(ParticleSystem particleSystem in onShootParticles)
        {
            particleSystem.Play();
        }

        foreach(Animator animator in onShootAnimations)
        {
            animator.SetTrigger("Shoot");
        }
    
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


}
