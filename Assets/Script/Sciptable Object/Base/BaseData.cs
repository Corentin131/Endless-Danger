using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BaseData", menuName = "PlayerCustomization/BaseScriptableObject", order = 1)]
public class BaseData : ScriptableObject
{
    [Header("Presentation Data")]
    public new string name;
    public int cost;

    [Header("Graphics Data")]
    public GameObject prefab;
    public Sprite icon;

    [Header("Playing Data")]
    public float powerMaxDistance = 20;
}
