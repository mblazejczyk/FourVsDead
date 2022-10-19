using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "GameScriptableObjects/Gun")]
public class Gun : ScriptableObject
{
    public int Id;
    public string Name;

    public int Damage;
    public float ShootSpeed;
    public int Cost;
    public int CoinReward;

    public Sprite gunIcon;
    public AudioClip shootSound;
}
