using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerListitem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    public GameObject kickButton;
    public GameObject reportObj;
    public Image level;
    public Image badge;
    private int ThisPlayerId;
    Player player;
    public void SetUp(Player _player)
    {
        player = _player;
        if(_player.NickName == "admin")
        {
            text.text = "<color=red>" +  _player.NickName + "</color>";
        }
        else
        {
            text.text = _player.NickName;
        }
        int _playerXp = (int)_player.CustomProperties["Level"];
        ThisPlayerId = (int)_player.CustomProperties["pId"];
        LoginProfileManager lpm;
        if(GameObject.Find("LPM").GetComponent<LoginProfileManager>().Ranks[0] != null){
            lpm = GameObject.Find("LPM").GetComponent<LoginProfileManager>();
        }else{
            lpm = GameObject.FindGameObjectWithTag("Canvas").GetComponent<LoginProfileManager>();
        }
        for (int i = 0; i < lpm.Ranks.Length; i++)
        {
            if (lpm.TotalXpRequired[i] <= _playerXp)
            {
                level.sprite = lpm.Ranks[i];
            }
            else
            {
                break;
            }
        }

        if (_player.CustomProperties["Badge"] != null)
        {
            EqSystem es = GameObject.FindGameObjectWithTag("Canvas").GetComponent<EqSystem>();
            for (int i = 0; i < es.allItems.Length; i++)
            {
                if (es.allItems[i].id == (int)_player.CustomProperties["Badge"])
                {
                    badge.sprite = es.allItems[i].ItemImg;
                    badge.gameObject.GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = es.allItems[i].ItemName;
                    break;
                }
            }
        }
        else
        {
            Destroy(badge.gameObject);
        }

        if (!PhotonNetwork.IsMasterClient || PhotonNetwork.NickName == _player.NickName)
        {
            Destroy(kickButton);
        }
        if(PhotonNetwork.NickName == _player.NickName)
        {
            Destroy(reportObj);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public void KickPlayer()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1){
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<PhotonView>().RPC("RPC_KickPlayer", RpcTarget.All, player.NickName);
        }else{
            GameObject.FindGameObjectWithTag("GameController").GetComponent<PhotonView>().RPC("RPC_KickPlayer", RpcTarget.All, player.NickName);
        }
    }

    public void Report()
    {
        GameObject.Find("Report").GetComponent<ReportSystem>().OpenReport(ThisPlayerId);
    }
}
