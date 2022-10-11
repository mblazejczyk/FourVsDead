using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenager : MonoBehaviour
{
    public GameObject[] UpgradeButtons;
    public GameObject purchaseInProgress;
    public TMP_Text pointsInfo;
    public GameObject upgradeReady;
    public string UpgradeSave = "0000000000000000000000000";

    private void Awake()
    {
        string sql1 = "SELECT `UpgradePoints` FROM `accounts` WHERE `id` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

        gameObject.GetComponent<SqlController>().Send(sql1, "UpgradePoints");

        string sql = "SELECT `UpgradesSave` FROM `accounts` WHERE `id` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

        gameObject.GetComponent<SqlController>().Send(sql, "UpgradesSave");
    }

    public void SaveUpdated()
    {
        Debug.Log("Upgrades changed");
        for (int i = 0; i < UpgradeButtons.Length; i++)
        {
            if (IsUnlocked(i))
            {
                UpgradeButtons[i].GetComponent<Referencer>().Reference.GetComponent<Image>().color = new Color(0, 0.25f, 0, 0.5f);
            }
            else if (IsAvaliable(i))
            {
                UpgradeButtons[i].GetComponent<Referencer>().Reference.GetComponent<Image>().color = new Color(0.25f, 0, 0, 0f);
            }
            else
            {
                UpgradeButtons[i].GetComponent<Referencer>().Reference.GetComponent<Image>().color = new Color(0.25f, 0, 0, 0.5f);
            }
        }
    }

    public void PointsUpdate()
    {
        int points = GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().UpgradePoints;
        if (points > 0)
        {
            upgradeReady.SetActive(true);
            pointsInfo.text = "Upgrade points available: <color=green>" + points + "</color>";
        }
        else
        {
            upgradeReady.SetActive(false);
            pointsInfo.text = "Upgrade points available: " + points;
        }
    }

    public void BuyUpgrade(int upgradeId)
    {
        if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().UpgradePoints > 0 &&
            IsAvaliable(upgradeId) && !IsUnlocked(upgradeId))
        {
            purchaseInProgress.SetActive(true);
            UpgradeSave = UpgradeSave.Remove(upgradeId, 1);
            UpgradeSave = UpgradeSave.Insert(upgradeId, "1");
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().UpgradePoints--;

            //Update upgrade save
            string sql = "UPDATE `accounts` SET `UpgradesSave`= '" + UpgradeSave + "', `UpgradePoints` = (`UpgradePoints` - 1) WHERE `id` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

            gameObject.GetComponent<SqlController>().Send(sql, "id");
            StartCoroutine(buyInProgress());
        }
    }

    IEnumerator buyInProgress()
    {
        yield return new WaitForSeconds(2);
        Awake();
        yield return new WaitForSeconds(2);
        purchaseInProgress.SetActive(false);
    }

    public bool IsAvaliable(int upgradeId)
    {
        switch (upgradeId)
        {
            case 0:
                return true;
            case 1:
                if (IsUnlocked(0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 2:
                if (IsUnlocked(1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 3:
                if (IsUnlocked(1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 4:
                if (IsUnlocked(2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 5:
                if (IsUnlocked(3))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 6:
                if (IsUnlocked(5) && IsUnlocked(4))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 7:
                if (IsUnlocked(6))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 8:
                if (IsUnlocked(7))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 9:
                if (IsUnlocked(0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 10:
                if (IsUnlocked(9))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 11:
                if (IsUnlocked(9))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 12:
                if (IsUnlocked(10))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 13:
                if (IsUnlocked(11))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 14:
                if (IsUnlocked(12))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 15:
                if (IsUnlocked(13))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 16:
                if (IsUnlocked(14) && IsUnlocked(15))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 17:
                if (IsUnlocked(0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 18:
                if (IsUnlocked(17))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 19:
                if (IsUnlocked(18))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 20:
                if (IsUnlocked(19) && IsUnlocked(16) && IsUnlocked(8))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 21:
                if (IsUnlocked(20))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 22:
                if (IsUnlocked(21))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 23:
                if (IsUnlocked(22))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case 24:
                if (IsUnlocked(23))
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    public bool IsUnlocked(int upgradeId)
    {
        if(UpgradeSave[upgradeId] == '0')
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
