using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardSystem : MonoBehaviour
{
    public GameObject rewardPrompt;
    public TMP_Text rewardAmmount;
    void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("RewardSaver").Length > 0)
        {
            rewardPrompt.SetActive(true);
            int xp = PlayerPrefs.GetInt("NowXp");

            string sql = "UPDATE `accounts` SET `totalXp` = (`totalXp` + "
                + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted +
                ") WHERE `accounts`.`id` = " +
                GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;
            rewardAmmount.text = "You gained " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted + " xp from last game!";

            foreach (int i in GameObject.Find("Canvas").GetComponent<LoginProfileManager>().TotalXpRequired)
            {
                if(xp < i && xp + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted > i)
                {
                    sql =   "UPDATE `accounts` SET `totalXp` = (`totalXp` + "
                            + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted +
                            "), `UpgradePoints` = (`UpgradePoints` + 1) WHERE `accounts`.`id` = " +
                            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;
                    rewardAmmount.text = "You gained " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted + " xp from last game <color=green>AND LEVELED UP!</color>";
                }
            }
            gameObject.GetComponent<SqlController>().Send(sql, "id");
            
            Debug.Log("Sent new sql: " + sql);


            sql = "UPDATE `saves` SET `zombieKilled`=(`zombieKilled` + "+ GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().zombieKilled + ")," +
                "`coinsCollected`=(`coinsCollected` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().coinsCollected + ")," +
                "`dmgTaken`=(`dmgTaken` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgTaken + ")," +
                "`dmgGiven`=(`dmgGiven` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgGiven + ")," +
                "`deaths`=(`deaths` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().deaths + ")," +
                "`knockouts`=(`knockouts` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().knockouts + ")," +
                "`buys`=(`buys` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().buys + ")," +
                "`shots`=(`shots` + " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().shots + ")," +
                "`firstGame`= IF(`firstGame` = '0000-00-00', CURRENT_DATE(), `firstGame`) " +
                "WHERE `playerId` = " + GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

            Debug.Log("Sent new sql: " + sql);
            gameObject.GetComponent<SqlController>().Send(sql, "id");
            Destroy(GameObject.FindGameObjectWithTag("RewardSaver"));

        }
    }

}
