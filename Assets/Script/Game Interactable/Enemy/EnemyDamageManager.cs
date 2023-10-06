using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class EnemyDamageManager : DamageManager
{
    [Header("Enemy source")]
    [SerializeField] Rigidbody rbEnemy;
    [SerializeField] List<SpawnDerbiesInfo> prefabsToSpawnOnDeath;
    [SerializeField] List<GameObject> textsToSpawn;

    [Header("Enemy data")]
    public float maxHealth;
    [SerializeField] float ballImpactForceMultiplier;
    [SerializeField] float baseImpactForceMultiplayer;
    [SerializeField] float delayOnReceiveDamage;
    [SerializeField] float forceOnExplosionDeathMultiplier;

    [Header("Out")]
    public float currentHealth;

    Enemy enemy;
    int lastTypeDamage;
    Vector3 lastDirection;
    System.Random r;

    [Serializable]
    struct SpawnDerbiesInfo
    {
        public GameObject gameObject;
        public Vector3 posToAddToCurrentPos;
        public float force;
    }

    private void Start() 
    {
        currentHealth = maxHealth;
        r = new System.Random();
        enemy = GetComponent<Enemy>();
    }

    private void Update() 
    {
        if (currentHealth <= 0)
        {
            SpawnDerbies(prefabsToSpawnOnDeath,lastDirection);
            
            Destroy(gameObject);
        }
    }

    void SpawnDerbies(List<SpawnDerbiesInfo> prefabs ,[Optional] Vector3 direction)
    {

        foreach(SpawnDerbiesInfo spawnDerbiesInfo in prefabs)
        {
            GameObject derbies;

            if (spawnDerbiesInfo.gameObject.activeInHierarchy)
            {
                spawnDerbiesInfo.gameObject.transform.parent = null;
                spawnDerbiesInfo.gameObject.SetActive(true);
                derbies = spawnDerbiesInfo.gameObject;

            }else
            {
                Vector3 position = transform.position+spawnDerbiesInfo.posToAddToCurrentPos;
                derbies = Instantiate(spawnDerbiesInfo.gameObject,position,transform.rotation);
            }

            Rigidbody rb = derbies.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                if (lastTypeDamage != 2)
                {
                    
                    rb.velocity = direction*spawnDerbiesInfo.force;
                }else
                {
                    rb.AddExplosionForce(spawnDerbiesInfo.force*forceOnExplosionDeathMultiplier,transform.position,10);
                }
                //rb.AddExplosionForce(force,transform.position,10);
            }
        }
    }

    public override void ApplyGunDamage(float damage,Vector3 position, Vector3 direction)
    {
        base.ApplyGunDamage(damage,position ,direction);

        damage = damage+r.Next(0,5);

        currentHealth -= damage;

        Vector3 force = direction*damage*ballImpactForceMultiplier;
        Debug.Log(force);
        enemy.ReceiveDamage(delayOnReceiveDamage);
        rbEnemy.velocity = force;

        lastTypeDamage = 0;
        lastDirection = direction;

        SpawnText(damage,direction);

        Debug.Log($"Damage on a enemy of {damage} on {direction}; Life left : {currentHealth}");
    }

    public override void ApplyBaseDamage(float damage,Vector3 position, Quaternion EffectSpawnRotation, Vector3 direction, [Optional] List<GameObject> effectToSpawn)
    {
        base.ApplyBaseDamage(damage,position, EffectSpawnRotation, direction, effectToSpawn);
        
        damage = damage+r.Next(-5,5);
        
        currentHealth -= damage;

        Vector3 force = -direction*damage*baseImpactForceMultiplayer;
        Debug.Log($"DIrection 3 :  {direction}");
        enemy.ReceiveDamage(delayOnReceiveDamage);
        rbEnemy.velocity = force;

        lastTypeDamage = 1;
        lastDirection = -direction;

        SpawnText(damage,direction);

        Debug.Log($"Damage on a enemy of {damage} on {direction}; Life left : {currentHealth}");
    }

    public override void ApplyExplosiveDamage(Vector3 position, float damage, float force,float radius)
    {
        base.ApplyExplosiveDamage(position, damage, force,radius);
        enemy.ReceiveDamage(2f);
        rbEnemy.AddExplosionForce(force,position,radius);
        //lastDirection = new Vector3(r.Next(-1,1),r.Next(-1,1),r.Next(-1,1));
        lastTypeDamage = 2;
        currentHealth -= damage;
        SpawnText(damage);

        Debug.Log("ForceAdded");
    }

    void SpawnText(float damage,[Optional]Vector3 direction)
    {
        foreach (GameObject textObject in textsToSpawn)
        {
            Vector3 spawnPosition = transform.position;

            if (direction != null)
            {
                spawnPosition = transform.position - direction*r.Next(1,2);
            }

            GameObject spawnedObject = Instantiate(textObject , spawnPosition , transform.rotation);
            spawnedObject.transform.eulerAngles = new Vector3(0,0,0);

            TextManager text = textObject.GetComponent<TextManager>();
            text.SetTextInt(damage);
        }
    }
}
