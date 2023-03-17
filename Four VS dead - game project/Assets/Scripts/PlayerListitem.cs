using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class PlayerListitem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    public GameObject kickButton;
    public GameObject addFriendButton;

    Player player;
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
        if (!PhotonNetwork.IsMasterClient || PhotonNetwork.NickName == _player.NickName)
        {
            Destroy(kickButton);
        }
        if(PhotonNetwork.NickName == _player.NickName)
        {
            Destroy(addFriendButton);
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
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<PhotonView>().RPC("RPC_KickPlayer", RpcTarget.All, player.NickName);
    }

    

    public void AddFriend()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<PhotonView>().RPC("RPC_FriendRequest", RpcTarget.All, player.NickName, PhotonNetwork.NickName);
    }
}
