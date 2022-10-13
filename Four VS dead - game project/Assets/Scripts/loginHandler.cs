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
    public int startCoins = 0;
    public int startHp = 0;
    public int startSpeed = 0;
    public int startDodge = 0;
    public int goldenNick = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSave(string save)
    {
        HpAdded = 0;
        ArmorAdded = 0;
        DodgeAdded = 0;
        hpForWave = 0;
        coinsAdded = 0;
        XpForWin = 0;
        XpGame = 0;
        startCoins = 0;
        startHp = 0;
        startSpeed = 0;
        startDodge = 0;
        goldenNick = 0;
        Debug.Log(save);
        if(save[0] == '1')
        {
            //Start journy
        }
        if (save[1] == '1')
        {
            HpAdded += 2;
        }
        if (save[2] == '1')
        {
            DodgeAdded += 2;
        }
        if (save[3] == '1')
        {
            HpAdded += 2;
        }
        if (save[4] == '1')
        {
            HpAdded += 3;
        }
        if (save[5] == '1')
        {
            ArmorAdded += 2;
        }
        if (save[6] == '1')
        {
            hpForWave += 25;
        }
        if (save[7] == '1')
        {
            hpForWave += 25;
        }
        if (save[8] == '1')
        {
            hpForWave += 50;
        }
        if (save[9] == '1')
        {
            XpGame += 5;
        }
        if (save[10] == '1')
        {
            XpForWin += 20;
        }
        if (save[11] == '1')
        {
            XpGame += 5;
        }
        if (save[12] == '1')
        {
            XpForWin += 30;
        }
        if (save[13] == '1')
        {
            XpGame += 5;
        }
        if (save[14] == '1')
        {
            XpForWin += 20;
        }
        if (save[15] == '1')
        {
            XpGame += 10;
        }
        if (save[16] == '1')
        {
            XpForWin += 30;
            XpGame += 15;
        }
        if (save[17] == '1')
        {
            coinsAdded += 5;
        }
        if (save[18] == '1')
        {
            coinsAdded += 5;
        }
        if (save[19] == '1')
        {
            coinsAdded += 10;
        }
        if (save[20] == '1')
        {
            startCoins += 100;
        }
        if (save[21] == '1')
        {
            startHp += 100;
        }
        if (save[22] == '1')
        {
            startSpeed += 20;
        }
        if (save[23] == '1')
        {
            startDodge += 4;
        }
        if (save[24] == '1')
        {
            goldenNick++;
        }
    }
}
