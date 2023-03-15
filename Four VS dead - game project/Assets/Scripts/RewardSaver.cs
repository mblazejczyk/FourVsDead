using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSaver : MonoBehaviour
{
    public int xpGranted;
    [Header("Info for profile")]
    public int zombieKilled;
    public int coinsCollected; //
    public int dmgTaken; //
    public int dmgGiven; //
    public int deaths; //
    public int knockouts; //
    public int buys; //
    public int shots; //

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
