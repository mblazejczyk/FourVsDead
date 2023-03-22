using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loginSystem : MonoBehaviour
{
    public TMP_InputField login;
    public TMP_InputField password;
    public Toggle rememberToggle;
    public TMP_Text error;
    public GameObject bannedMsgBox;

    private void Start()
    {
        if(PlayerPrefs.GetString("isSaved") == "true")
        {
            login.text = PlayerPrefs.GetString("loginStr");
            password.text = PlayerPrefs.GetString("passStr");
        }
    }
    public void Login()
    {
        login.interactable = false;
        password.interactable = false;
        StartCoroutine(Upload());
    }

    public void Register()
    {
        Application.OpenURL("https://yellowsink.pl/fourvsdead/register.html");
    }

    public void LoginTest(int whichOne)
    {
        if(whichOne == 1)
        {
            login.text = "test1";
            password.text = "test1";
        }
        else
        {
            login.text = "test2";
            password.text = "test2";
        }
        login.interactable = false;
        password.interactable = false;
        error.text = "Connecting...";
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login.text);
        form.AddField("password", password.text);
        using (UnityWebRequest www = UnityWebRequest.Post("https://yellowsink.pl/fourvsdead/sql.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                error.text = www.error;
                login.interactable = true;
                password.interactable = true;
            }
            else
            {
                if(www.downloadHandler.text == "error" || www.downloadHandler.text == "")
                {
                    error.text = "Login failed";
                    login.interactable = true;
                    password.interactable = true;
                }
                else
                {
                    if(www.downloadHandler.text.Split('|')[0] == "banned")
                    {
                        bannedMsgBox.SetActive(true);
                        bannedMsgBox.GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "<color=white>This account has been banned</color>\nYou have been banned for:<color=red>\n" + www.downloadHandler.text.Split('|')[1] + "</color>\nand will be unable to play till:<color=red>\n" + www.downloadHandler.text.Split('|')[2]+"</color>";
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId = www.downloadHandler.text.Split('|')[0];
                        GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().SetSave(www.downloadHandler.text.Split('|')[1]);
                        GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login = login.text;
                        if (rememberToggle.isOn)
                        {
                            PlayerPrefs.SetString("loginStr", login.text);
                            PlayerPrefs.SetString("passStr", password.text);
                            PlayerPrefs.SetString("isSaved", "true");
                        }
                        else
                        {
                            PlayerPrefs.SetString("isSaved", "false");
                        }
                        SceneManager.LoadScene(1);
                    }
                }
            }
        }
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
