using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RescueSystem : MonoBehaviour
{
    public bool isOnTrigger = false;
    public GameObject playerConnected;
    public GameObject col;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player") { return; }
        if (collision.GetComponent<PhotonView>().IsMine && collision.GetComponent<PlayerController>().Hp != 0)
        {
            isOnTrigger = true;
            col = collision.gameObject;
            StartCoroutine(countdown());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (collision.GetComponent<PhotonView>().IsMine && collision.GetComponent<PlayerController>().Hp != 0)
        {
            isOnTrigger = false;
            col = null;
            StopAllCoroutines();
        }
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(5f);
        if (isOnTrigger)
        {
            playerConnected.GetComponent<PhotonView>().RPC("RPC_Revived", RpcTarget.All);
        }
    }

    
}
