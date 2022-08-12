using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    public string takenBy = "";
    [SerializeField] Image HpImg;
    [SerializeField] Text HpText;
    [SerializeField] Image GunIcon;
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

    public void SetHp(float hp)
    {
        HpText.text = hp + "/100";
        HpImg.fillAmount = hp / 100;
    }

    public void SetGun(Sprite gunIcon)
    {
        GunIcon.sprite = gunIcon;
    }

    public void SetAmmo(int AmmoAmmount, int MaxAmmo)
    {
        AmmoText.text = AmmoAmmount + "/" + MaxAmmo;
    }

    public void SetCoin(int CoinAmmount)
    {
        CoinText.text = "$" + CoinAmmount;
    }
}
