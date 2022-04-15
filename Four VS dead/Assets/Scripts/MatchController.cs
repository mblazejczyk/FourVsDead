using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MatchController : MonoBehaviour
{
    public Transform[] SpawnPoints;

    // Start is called before the first frame update
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
