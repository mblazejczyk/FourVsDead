using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loginHandler : MonoBehaviour
{
    public string loginId = "";
    public string login = "";
    public int UpgradePoints = 0;
    [Space(20)]
    public int HpAdded = 0;
    public int ArmorAdded = 0;
    public int DodgeAdded = 0;
    public int hpForWave = 0;
    public int coinsAdded = 0;
    public int XpForWin = 0;
    public int XpGame = 0;
    [Space(10)]
    public int startCoint = 0;
    public int startHp = 0;
    public int startSpeed = 0;
    public int startDodge = 0;
    public int goldenNick = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
