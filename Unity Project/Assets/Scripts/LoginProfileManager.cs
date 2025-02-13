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
    public TMP_Text[] accountInfo;

    void Start()
    {
        Invoke("LateStart", 3);
    }

    void LateStart()
    {
        if(LoginAsText == null || LoginAsText.text == null || GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login == null) { Invoke("LateStart", 3); return; }
        LoginAsText.text = "Logged in as: " + GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login;

        string sql = "SELECT `totalXp` FROM `accounts` WHERE `id` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

        gameObject.GetComponent<SqlController>().Send(sql, "totalXp");

        sql = "SELECT GROUP_CONCAT(`zombieKilled`, ';', `coinsCollected`,';', `dmgTaken`,';', `dmgGiven`,';'" +
            ", `deaths`,';', `knockouts`,';', `buys`,';', `shots`,';', `firstGame`) AS 'save_res' FROM `saves` WHERE `playerId` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;
        gameObject.GetComponent<SqlController>().Send(sql, "save_res");
    }

    public GameObject TutorialPrompt;
    public void UpdateProfileDetails(string res)
    {
        string[] saved = res.Split(';');
        if(saved[saved.Length -1] == "0000-00-00")
        {
            saved[saved.Length - 1] = "never played before";
            TutorialPrompt.SetActive(true);
        }
        for (int i = 0; i < saved.Length; i++)
        {
            accountInfo[i].text = saved[i];
        }
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
            int x = TotalXpRequired[maxed + 1] - TotalXpRequired[maxed];
            XpBar.fillAmount = (float)y / (float)x;
        }
    }
}
