using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginProfileManager : MonoBehaviour
{
    public TMP_Text LoginAsText;
    void Start()
    {
        LoginAsText.text = "Logged in as: " + GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login;
    }

}
