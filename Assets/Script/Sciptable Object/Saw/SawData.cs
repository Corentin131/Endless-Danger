using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SawData", menuName = "PlayerCustomization/SawScriptableObject", order = 2)]
public class SawData : ScriptableObject
{
    [Header("Presentation Data")]
    public new string name;
    public int cost;

    [Header("Graphics data")]
    public GameObject prefab;
    public Sprite icon;

    [Header("Stats Data")]
    public float damage;
    public float adhesion;
}
