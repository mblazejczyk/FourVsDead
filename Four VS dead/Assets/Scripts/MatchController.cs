using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchController : MonoBehaviourPunCallbacks
{
    public GameObject[] SpawnPoints;
    public Waves[] wave;

    public float TimeBetweenSpawns;
    public int WaveNow = 0;
    int EnemiesLeft = 0;
    int EnemiesToSpawn = 0;
    public TMP_Text waveText;
    public TMP_Text enemiesLeftText;

    void Start()
    {
        waveText = GameObject.FindGameObjectWithTag("WaveText_info").GetComponent<TMP_Text>();
        enemiesLeftText = GameObject.FindGameObjectWithTag("EnemiesLeft_info").GetComponent<TMP_Text>();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            StartNewWave();
        }
    }

    void StartNewWave()
    {
        WaveNow++;
        if(wave.Length >= WaveNow)
        {
            EnemiesLeft = wave[WaveNow-1].HowManyEnemies;
            EnemiesToSpawn = EnemiesLeft;
            waveText.text = "Wave " + WaveNow;
            TimeBetweenSpawns = wave[WaveNow - 1].TimeBetweenSpawns;
            UpdateText(WaveNow, EnemiesLeft);
            StartCoroutine(SpawnNew());
        }
        else
        {
            waveText.text = "Game ended";
            Debug.Log("GameEnded");
        }
    }

    public void SubstractEnemiesLeft()
    {
        EnemiesLeft--;
        UpdateText(WaveNow, EnemiesLeft);
    }

    public void UpdateText(int CurrentWave, int EnemiesToKill)
    {
        gameObject.GetComponent<PhotonView>().RPC("RPC_UpdateText", RpcTarget.All, CurrentWave, EnemiesToKill);
    }

    [PunRPC]
    void RPC_UpdateText(int CurrentWave, int EnemiesToKill)
    {
        waveText.text = "Wave " + CurrentWave;
        enemiesLeftText.text = "Enemies left: " + EnemiesToKill;
    }

    GameObject ActiveSpawn()
    {
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
    }
    IEnumerator SpawnNew()
    {
        yield return new WaitForSeconds(TimeBetweenSpawns);

        GameObject obj = ActiveSpawn();
        while (!obj.GetComponent<Referencer>().Reference.GetComponent<BarycadeSystem>().isActivated)
        {
            obj = ActiveSpawn();
        }

        Vector3 spawn = obj.GetComponent<Transform>().position;

        if (PhotonNetwork.IsMasterClient && EnemiesToSpawn != 0)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyController"), spawn, Quaternion.identity);
            EnemiesToSpawn--;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (EnemiesToSpawn == 0 && EnemiesLeft == 0)
            {
                StartNewWave();
            }
            else
            {
                StartCoroutine(SpawnNew());
            }
        }
    }
}
