using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "PlayerCustomization/GunScriptableObject", order = 3)]
public class GunData : ScriptableObject
{
    [Header("Presentation Data")]
    public new string name;
    public int cost;

    [Header("Graphics data")]
    public GameObject prefab;
    public Sprite icon;

    [Header("Stats Data")]
    public float damage;
    public int numberOfBullet;
    public float timeToRecharge;
}
