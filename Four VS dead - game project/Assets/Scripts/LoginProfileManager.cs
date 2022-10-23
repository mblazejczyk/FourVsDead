using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginProfileManager : MonoBehaviour
{
    public TMP_Text LoginAsText;
    public TMP_Text XpOwned;
    public int Xp = 0;

    public Image rank;
    public Image XpBar;
    public int[] TotalXpRequired;
    public Sprite[] Ranks;
    void Start()
    {
        LoginAsText.text = "Logged in as: " + GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login;

        string sql = "SELECT `totalXp` FROM `accounts` WHERE `id` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

        gameObject.GetComponent<SqlController>().Send(sql, "totalXp");
    }
    public void UpdateRank()
    {
        int maxed = 0;
        for (int i = 0; i < Ranks.Length; i++)
        {
            if (TotalXpRequired[i] <= Xp)
            {
                rank.sprite = Ranks[i];
                maxed = i;
            }
            else
            {
                break;
            }
        }

        if (maxed == Ranks.Length - 1)
        {
            XpBar.fillAmount = 1;
        }
        else
        {
            int y = Xp - TotalXpRequired[maxed];
            int x = TotalXpRequired[maxed + 1];
            XpBar.fillAmount = (float)y / (float)x;
        }
    }
}
