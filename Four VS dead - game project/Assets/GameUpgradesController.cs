using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameUpgradesController : MonoBehaviourPunCallbacks
{
    public GameObject[] buyUpgradesPoints;
    public bool fasterRate = false; //id 0
    public bool hpRegain = false; //id 1
    public bool freezingAmm = false; //id 2
    public bool zombieTrack = false; //id 3
    //refill id 4, kill zombies id 5
    public int upgradesBought = 0;
    public int[] CostsById;


    public void BuyUpgrade(int upgradeId)
    {
        if(upgradesBought >= 2 && upgradeId < 4) { return; }
        gameObject.GetComponent<PhotonView>().RPC("RPC_BuyUpgrade", RpcTarget.All, upgradeId);
        if (fasterRate) { upgradesBought++; }
        if (hpRegain) { upgradesBought++; }
        if (freezingAmm) { upgradesBought++; }
        if (zombieTrack) { upgradesBought++; }
    }

    [PunRPC]
    void RPC_BuyUpgrade(int upgradeId)
    {
        switch (upgradeId)
        {
            case 0:
                fasterRate = true;
                break;
            case 1:
                hpRegain = true;
                StartCoroutine(regainOverTime());
                break;
            case 2:
                freezingAmm = true;
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    obj.GetComponent<EnemyController>().speed *= 0.6f;
                }
                break;
            case 3:
                zombieTrack = true;
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (obj.GetComponent<PhotonView>().IsMine)
                    {
                        obj.GetComponent<PlayerController>().tracker.SetActive(true);
                        break;
                    }
                }
                break;
            case 4:
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    obj.GetComponent<PlayerController>().ModifyHp(false, 9999999);
                }
                break;
            case 5:
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    obj.GetComponent<EnemyController>().TakeDamage(9999999);
                }
                break;
        }
    }

    IEnumerator regainOverTime()
    {
        if (!hpRegain) { StopCoroutine(regainOverTime()); }
        yield return new WaitForSeconds(5);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            obj.GetComponent<PlayerController>().ModifyHp(false, 5);
        }
        StartCoroutine(regainOverTime());
    }
}
