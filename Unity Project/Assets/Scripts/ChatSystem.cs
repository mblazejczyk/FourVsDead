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
    public TMP_Dropdown input_dp;
    public GameObject content;
    public PhotonView PV;
    public GameObject[] toActive;

    public bool isChatOpen = false;
    public bool isInGame = false;
    private void Awake()
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
                Send();
                foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (pl.GetComponent<PhotonView>().IsMine)
                    {
                        pl.GetComponent<PlayerController>().canMove = true;
                        break;
                    }
                }
                foreach (GameObject obj in toActive)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                isChatOpen = true;
                Cursor.visible = true;
                foreach(GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (pl.GetComponent<PhotonView>().IsMine)
                    {
                        pl.GetComponent<PlayerController>().canMove = false;
                        break;
                    }
                }
                foreach (GameObject obj in toActive)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

    public void Clear()
    {
        chat.text = "";
    }
    public void Send()
    {
        string textInp = input_dp.options[input_dp.value].text;
        string s = "";
        if(GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().goldenNick == 1)
        {
            s = "<color=yellow>" + 
                GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login + "</color>: " + textInp;
        }
        else
        {
            s = GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().login + ": " + textInp;
        }
        PV.RPC("RPC_SendChat", RpcTarget.All, s);
        Debug.Log("Sent");
    }

    public void SendSystemMessage(string msg)
    {
        string s = "<color=green><i>" + msg + "</color></i>";
        Debug.Log(s);
        PV = gameObject.GetComponent<PhotonView>();
        PV.RPC("RPC_SendChat", RpcTarget.All, s);
        Debug.Log("Sent system");
    }

    [PunRPC]
    void RPC_SendChat(string message)
    {
        Debug.Log("Recived");
        chat.text += "\n" + message;
    }
}
