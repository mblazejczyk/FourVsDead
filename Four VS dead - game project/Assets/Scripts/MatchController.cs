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
            UpdateText(WaveNow, EnemiesLeft, true);
            StartCoroutine(SpawnNew());
        }
        else
        {
            waveText.text = "Game ended";
            UpdateText(WaveNow, EnemiesLeft, false);
            Debug.Log("GameEnded");
        }
    }

    public void SubstractEnemiesLeft()
    {
        EnemiesLeft--;
        UpdateText(WaveNow, EnemiesLeft, false);
    }

    public void UpdateText(int CurrentWave, int EnemiesToKill, bool isNewWave)
    {
        gameObject.GetComponent<PhotonView>().RPC("RPC_UpdateText", RpcTarget.All, CurrentWave, EnemiesToKill, isNewWave);
    }

    [PunRPC]
    void RPC_UpdateText(int CurrentWave, int EnemiesToKill, bool isNewWave)
    {
        waveText.text = "Wave " + CurrentWave;
        enemiesLeftText.text = "Enemies left: " + EnemiesToKill;

        if(waveText.text == "Game ended")
        {
            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted += 
                (int)((float)GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted * 
                ((float)GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().XpForWin / 100));

            GameObject.FindGameObjectWithTag("CursorController").GetComponent<CursorController>().LeaveGame();
        }

        if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().hpForWave > 0 && isNewWave)
        {
            Debug.Log("End wave heal");
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj.GetComponent<PhotonView>().IsMine)
                {
                    float multiplaier = (float)GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().hpForWave / 100;
                    float inalHp = obj.GetComponent<PlayerController>().MaxHp * multiplaier;
                    obj.GetComponent<PlayerController>().ModifyHp(false, (int)inalHp);
                }
            }
        }
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
