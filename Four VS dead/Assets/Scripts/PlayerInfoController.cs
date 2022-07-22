using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    public string takenBy = "";
    [SerializeField] Image HpImg;
    [SerializeField] Text HpText;
    [SerializeField] GameObject KnockedImg;
    [SerializeField] GameObject DeadImg;
    [SerializeField] Image GunIcon;
    [SerializeField] Image ArmorIcon;
    [SerializeField] Text AmmoText;
    [SerializeField] Text PlayerName;
    [SerializeField] Text IsHostText;
    [SerializeField] Text CoinText;

    public void SetPlayer(string Name, bool isHost)
    {
        takenBy = Name;
        PlayerName.text = Name;
        if (!isHost) { IsHostText.text = ""; }
    }

    public void SetHp(float hp, float maxHp)
    {
        HpText.text = hp + "/" + maxHp;
        HpImg.fillAmount = hp / maxHp;
        if(hp == 0)
        {
            setKnocked(true);
        }
        else
        {
            setKnocked(false);
        }
    }

    public void SetGun(Sprite gunIcon)
    {
        GunIcon.sprite = gunIcon;
    }

    public void SetArmor(Sprite armorIcon)
    {
        ArmorIcon.sprite = armorIcon;
    }

    public void SetAmmo(int AmmoAmmount, int MaxAmmo)
    {
        AmmoText.text = AmmoAmmount + "/" + MaxAmmo;
    }

    public void SetCoin(int CoinAmmount)
    {
        CoinText.text = "$" + CoinAmmount;
    }

    public void setDeath(bool isDead)
    {
        if (isDead)
        {
            DeadImg.SetActive(true);
            setKnocked(false);
        }
        else
        {
            DeadImg.SetActive(false);
        }
    }

    public void setKnocked(bool isKnocked)
    {
        if (isKnocked)
        {
            KnockedImg.SetActive(true);
        }
        else
        {
            KnockedImg.SetActive(false);
        }
    }
}
