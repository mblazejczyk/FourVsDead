using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New armor", menuName = "GameScriptableObjects/Armor")]
public class ArmorSO : ScriptableObject
{
    public Sprite armorIcon;

    public float DmgReduction;
    public float DodgeChance;
    public float SpeedAdded;
    public int HpAdd;
    public int Cost;
}