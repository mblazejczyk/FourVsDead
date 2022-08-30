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
    public GameObject[] toActive;

    public bool isChatOpen = false;
    public bool isInGame = false;
    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isInGame)
        {
            if (isChatOpen)
            {
                isChatOpen = false;
                Cursor.visible = false;
                foreach (GameObject obj in toActive)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                isChatOpen = true;
                Cursor.visible = true;
                foreach (GameObject obj in toActive)
                {
                    obj.SetActive(true);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send();
        }
    }

    public void Clear()
    {
        chat.text = "";
    }
    public void Send()
    {
        if(input.text == "") { return; }
        string s = GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login + ": " + input.text;
        PV.RPC("RPC_SendChat", RpcTarget.All, s);
        Debug.Log("Sent");
        input.text = "";
    }

    [PunRPC]
    void RPC_SendChat(string message)
    {
        Debug.Log("Recived");
        chat.text += "\n" + message;
    }
}
