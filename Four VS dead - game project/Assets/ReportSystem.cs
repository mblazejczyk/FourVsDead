using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class ReportSystem : MonoBehaviour
{
    public int playerToReport;
    public GameObject reportPanel;

    public void OpenReport(int PlayerId)
    {
        playerToReport = PlayerId;
        reportPanel.SetActive(true);
    }

    public void Report(TMP_Dropdown reason)
    {
        string chatlog = GameObject.Find("Chat").GetComponent<ChatSystem>().chat.text;
        string sql = "INSERT INTO `reports` (`id`, `reportedUserId`, `reason`, `chatLog`) VALUES (NULL, '" + playerToReport + "', '"+reason.options[reason.value].text+"', '" + chatlog + "');";
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<SqlController>().Send(sql, "");
        StartCoroutine(Infobox());
        reportPanel.SetActive(false);
    }

    IEnumerator Infobox()
    {
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMPro.TMP_Text>().text = "<color=white>Player reported</color>";
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
        yield return new WaitForSeconds(3);
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", false);
    }
}
