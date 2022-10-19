using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class DoorSystem : MonoBehaviour
{
    PhotonView PV;
    public GameObject doors;
    public GameObject[] spawnersToActive;

    private void Awake()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Hold E to open";
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E pressed");
                if (collision.GetComponent<PlayerController>().Coins >= 100)
                {
                    Debug.Log("Bought");
                    collision.GetComponent<PlayerController>().ModifyCoins(0, 100);
                    GameObject.FindGameObjectWithTag("UiInfoBg").GetComponent<Animator>().SetTrigger("buy");
                    DestroyDoors();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Not enaugh coins";
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", false);
        }
    }

    public void DestroyDoors()
    {
        PV.RPC("RPC_DoorDestroy", RpcTarget.All);
    }

    [PunRPC]
    void RPC_DoorDestroy()
    {
        foreach(GameObject obj in spawnersToActive)
        {
            obj.GetComponent<BarycadeSystem>().isActivated = true;
        }
        GameObject.FindGameObjectWithTag("MidScreenText").GetComponent<TMP_Text>().text = "New area <color=red>unlocked</color>";
        GameObject.FindGameObjectWithTag("MidScreenText").GetComponent<Animator>().SetTrigger("Open");
        GameObject.FindGameObjectWithTag("GameSoundSource").GetComponent<MatchAudioController>().PlaySound(1);
        Destroy(doors);
    }
}
