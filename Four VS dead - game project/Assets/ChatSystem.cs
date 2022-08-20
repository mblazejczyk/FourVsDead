using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class ChatSystem : MonoBehaviourPunCallbacks
{
    public TMP_Text chat;
    public TMP_InputField input;
    public GameObject content;
    public PhotonView PV;
    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }
    public void Send()
    {
        if(input.text == "") { return; }
        string s = GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login + ": " + input.text;
        PV.RPC("RPC_SendChat", RpcTarget.All, s);
        input.text = "";
    }

    [PunRPC]
    void RPC_SendChat(string message)
    {
        chat.text += "\n" + message;
    }
}
