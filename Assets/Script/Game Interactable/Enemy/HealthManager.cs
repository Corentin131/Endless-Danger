using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Data")]
    public float maxHealth;

    [HideInInspector] public float currentHealth;

    void Update()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy Die");
        }
    }

    public virtual void ApplyBallDamage(float damage,Vector3 impactPosition)
    {
        currentHealth -= damage;
        Debug.Log($"Damage on a enemy of {damage} on {impactPosition}");
    }
}
