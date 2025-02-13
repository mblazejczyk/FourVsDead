using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MatchController : MonoBehaviourPunCallbacks
{
    public GameObject[] SpawnPoints;
    public Waves[] wave;
    public Waves[] tutorialWaves;

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
            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                StartNewWave();
            }
        }
    }

    public void StartNewWave()
    {
        WaveNow++;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            StartNewWaveTut();
            return;
        }
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

    public void StartNewWaveTut()
    {
        if (tutorialWaves.Length >= WaveNow)
        {
            EnemiesLeft = tutorialWaves[WaveNow - 1].HowManyEnemies;
            EnemiesToSpawn = EnemiesLeft;
            waveText.text = "Wave " + WaveNow;
            TimeBetweenSpawns = tutorialWaves[WaveNow - 1].TimeBetweenSpawns;
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
        if(waveText == null)
        {
            waveText = GameObject.FindGameObjectWithTag("WaveText_info").GetComponent<TMP_Text>();
            enemiesLeftText = GameObject.FindGameObjectWithTag("EnemiesLeft_info").GetComponent<TMP_Text>();
        }
        waveText.text = "Wave " + CurrentWave;
        enemiesLeftText.text = "Enemies left: " + EnemiesToKill;

        if (isNewWave)
        {
            GameObject.FindGameObjectWithTag("MidScreenText").GetComponent<TMP_Text>().text = "Wave <color=red>" + CurrentWave + "</color>";
            GameObject.FindGameObjectWithTag("MidScreenText").GetComponent<Animator>().SetTrigger("Open");
            GameObject.FindGameObjectWithTag("GameSoundSource").GetComponent<MatchAudioController>().PlaySound(0);
            if(WaveNow == 10)
            {
                GameObject.FindGameObjectWithTag("MidScreenText").GetComponent<TMP_Text>().text = "BOSS!!! <color=red>GET TO GARDEN!</color>";
            }
        }

        if (CurrentWave > wave.Length || (CurrentWave > tutorialWaves.Length && SceneManager.GetActiveScene().name == "Tutorial"))
        {
            waveText.text = "Game ended";
            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted += 
                (int)((float)GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted * 
                ((float)GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().XpForWin / 100));
            StartCoroutine(spawnFirework());
            StartCoroutine(GameEnding());
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
                    obj.GetComponent<PlayerController>().ModifyHp(false, (int)inalHp, 0, true);
                }
                if(obj.GetComponent<PlayerController>().isDead || obj.GetComponent<PlayerController>().Hp == 0)
                {
                    obj.GetComponent<Referencer>().Reference.GetComponent<RescueSystem>().Ressurect();
                }
            }
        }
    }

    public void TutorialEnd()
    {
        Debug.Log("Tutorial finished");
        waveText.text = "Game ended";
        GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted +=
            (int)((float)GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted *
            ((float)GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().XpForWin / 100));
        StartCoroutine(spawnFirework());
        StartCoroutine(GameEnding());
    }

    public GameObject Firework;
    IEnumerator spawnFirework()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(Firework, GameObject.FindGameObjectsWithTag("FireworkSpawn")[Random.Range(0, GameObject.FindGameObjectsWithTag("FireworkSpawn").Length)].transform);
        }
        StartCoroutine(spawnFirework());
    }

    IEnumerator GameEnding()
    {
        yield return new WaitForSeconds(10f);
        GameObject.FindGameObjectWithTag("CursorController").GetComponent<CursorController>().LeaveGame();
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
            if (WaveNow == 10)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Enemy_boss"), new Vector3(19, -1, 0), Quaternion.identity);
                EnemiesToSpawn--;
            }
            else
            {
                float ran = Random.value;
                if (ran < .33)
                {
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Enemy_archer"), spawn, Quaternion.identity);
                }
                else if (ran > .66)
                {
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "EnemyController"), spawn, Quaternion.identity);
                }
                else
                {
                    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Enemy_firerunner"), spawn, Quaternion.identity);
                }
                EnemiesToSpawn--;
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (EnemiesToSpawn == 0 && EnemiesLeft == 0 && SceneManager.GetActiveScene().name != "Tutorial")
            {
                StartNewWave();
            }
            else
            {
                StartCoroutine(SpawnNew());
            }
        }
    }

    [PunRPC]
    void RPC_KickPlayer(string nick)
    {
        if (PhotonNetwork.NickName == nick)
        {
            GameObject.FindGameObjectWithTag("CursorController").GetComponent<CursorController>().Quit();
        }
    }
}
