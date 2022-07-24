using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    public GameObject HostLeftPanel;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            HostLeftPanel = GameObject.Find("HostLeftPanel");
            HostLeftPanel.SetActive(false);
        }
    }

    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameController"), Vector3.zero, Quaternion.identity);
        }
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PV.IsMine)
        {
            StartCoroutine(StopGame());
        }
    }
    IEnumerator StopGame()
    {
        HostLeftPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        GameObject.FindGameObjectWithTag("CursorController").GetComponent<CursorController>().LeaveGame();
    }
}
