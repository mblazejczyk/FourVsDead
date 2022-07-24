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
            rewardAmmount.text = "You gained " + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted + " xp from last game!";
            string sql = "UPDATE `accounts` SET `totalXp` = (`totalXp` + " 
                + GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted + 
                ") WHERE `accounts`.`id` = " + 
                GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;
            gameObject.GetComponent<SqlController>().Send(sql, "id");
            Destroy(GameObject.FindGameObjectWithTag("RewardSaver"));
        }
    }

}
