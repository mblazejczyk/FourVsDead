using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class loginSystem : MonoBehaviour
{
    public TMP_InputField login;
    public TMP_InputField password;
    public TMP_Text error;
    public void Login()
    {
        login.interactable = false;
        password.interactable = false;
        StartCoroutine(Upload());
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
                if(www.downloadHandler.text == "error")
                {
                    error.text = "Login failed";
                    login.interactable = true;
                    password.interactable = true;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().loginId = www.downloadHandler.text;
                    GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login = login.text;
                    SceneManager.LoadScene(1);
                }
            }
        }
    }
}
