using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MatchController : MonoBehaviourPunCallbacks
{
    public GameObject[] SpawnPoints;
    

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            StartCoroutine(SpawnNew());
        }
    }

    GameObject ActiveSpawn()
    {
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
    }
    IEnumerator SpawnNew()
    {
        yield return new WaitForSeconds(10);

        GameObject obj = ActiveSpawn();
        while (!obj.GetComponent<Referencer>().Reference.GetComponent<BarycadeSystem>().isActivated)
        {
            obj = ActiveSpawn();
        }

        Vector3 spawn = obj.GetComponent<Transform>().position;

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyController"), spawn, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnNew());
        }
    }
}
