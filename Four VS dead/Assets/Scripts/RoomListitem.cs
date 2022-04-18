using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;

public class RoomListitem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text playerCount;
    [SerializeField] GameObject lockImg;
    private string password = "";
    public RoomInfo info;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        
        if (_info.Name.Contains("+PRIVATE_PASSWORD:"))
        {
            lockImg.SetActive(true);
            string replaced = _info.Name.Replace("+PRIVATE_PASSWORD:", "▓");
            string[] splited = replaced.Split('▓');
            text.text = splited[0];
            password = splited[1];
        }
        else
        {
            lockImg.SetActive(false);
            text.text = _info.Name;
        }
        playerCount.text = _info.PlayerCount + "/" + _info.MaxPlayers;
    }

    public void OnClick()
    {
        if (password != null)
        {
            Launcher.Instance.AskForPassword(info, password);
        }
        else
        {
            Launcher.Instance.JoinRoom(info);
        }
    }
}
