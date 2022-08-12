using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginProfileManager : MonoBehaviour
{
    public TMP_Text LoginAsText;
    public TMP_Text XpOwned;
    void Start()
    {
        LoginAsText.text = "Logged in as: " + GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login;

        string sql = "SELECT `totalXp` FROM `accounts` WHERE `id` = " +
            GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId;

        gameObject.GetComponent<SqlController>().Send(sql, "totalXp");
    }

}
