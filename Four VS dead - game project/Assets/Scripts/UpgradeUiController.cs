using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUiController : MonoBehaviour
{
    public Image stUpgrade;
    public Image ndUpgrade;
    public Sprite[] upgradesIcon;
    public Sprite noUpgrade;

    public void BuyUpgrade(int upgradeId)
    {
        if(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().upgradesBought > 0)
        {
            gameObject.GetComponent<Animator>().SetTrigger("upgraded");
            if(stUpgrade.sprite == noUpgrade)
            {
                stUpgrade.sprite = upgradesIcon[upgradeId];
            }
            else
            {
                ndUpgrade.sprite = upgradesIcon[upgradeId];
            }
        }
    }
}
