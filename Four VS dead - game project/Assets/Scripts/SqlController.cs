using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class SqlController : MonoBehaviour
{
    public string restults = "";
    public void Send(string sql, string toPrint)
    {
        StartCoroutine(Upload(sql, toPrint));
    }

    IEnumerator Upload(string sql, string toPrint)
    {
        restults = "Downloading...";
        WWWForm form = new WWWForm();
        form.AddField("sqlPos", sql);
        form.AddField("toPrint", toPrint);
        using (UnityWebRequest www = UnityWebRequest.Post("https://yellowsink.pl/fourvsdead/sqlImport.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                restults = www.downloadHandler.text;
                if(toPrint == "totalXp")
                {
                    gameObject.GetComponent<LoginProfileManager>().XpOwned.text = "Xp owned: " + restults;
                    gameObject.GetComponent<LoginProfileManager>().Xp = int.Parse(restults);
                    gameObject.GetComponent<LoginProfileManager>().UpdateRank();
                }
            }
        }
    }
}
