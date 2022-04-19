using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MatchController : MonoBehaviourPunCallbacks
{
    public Transform[] SpawnPoints;
    private GameObject HostLeftPanel;

    

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnNew());
        }
    }

    IEnumerator SpawnNew()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyController"), Vector3.zero, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnNew());
        }
    }

    
}
