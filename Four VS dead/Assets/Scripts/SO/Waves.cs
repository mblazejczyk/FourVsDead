using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New wave", menuName = "GameScriptableObjects/Waves")]
public class Waves : ScriptableObject
{
    public int WaveOrder;
    public float TimeBetweenSpawns;
    public int HowManyEnemies;

    [Header("Enemy stats")]
    public float Hp;
    public int Dmg;
    public float Dodge;
    public float Speed;
}
