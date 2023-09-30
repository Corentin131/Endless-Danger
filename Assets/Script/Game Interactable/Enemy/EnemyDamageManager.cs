using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyDamageManager : DamageManager
{
    [Header("Enemy source")]
    [SerializeField] Rigidbody rbEnemy;

    [Header("Enemy data")]
    public float maxHealth;
    [SerializeField] float ballImpactForceMultiplier;
    [SerializeField] float delayOnReceiveDamage;

    [Header("Out")]
    public float currentHealth;

    Enemy enemy;
    private void Start() 
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
    }

    public override void ApplyGunDamage(float damage,Vector3 position, Vector3 direction)
    {
        base.ApplyGunDamage(damage,position ,direction);
        currentHealth -= damage;

        Vector3 force = direction*damage*ballImpactForceMultiplier*Time.deltaTime;
        Debug.Log(force);
        enemy.ReceiveDamage(delayOnReceiveDamage);
        rbEnemy.AddForce(force);

        Debug.Log($"Damage on a enemy of {damage} on {direction}; Life left : {currentHealth}");
    }

    public override void ApplyBaseDamage(float damage,Vector3 position, Quaternion EffectSpawnRotation, Vector3 direction, [Optional] List<GameObject> effectToSpawn)
    {
        base.ApplyBaseDamage(damage,position, EffectSpawnRotation, direction, effectToSpawn);

        Vector3 force = -direction*damage*ballImpactForceMultiplier*Time.deltaTime;
        Debug.Log($"DIrection 3 :  {direction}");
        enemy.ReceiveDamage(delayOnReceiveDamage);
        rbEnemy.AddForce(force);

        Debug.Log($"Damage on a enemy of {damage} on {direction}; Life left : {currentHealth}");
    }
}
