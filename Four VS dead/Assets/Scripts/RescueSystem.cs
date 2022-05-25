using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RescueSystem : MonoBehaviour
{
    public bool isOnTrigger = false;
    public GameObject playerConnected;
    public GameObject col;
    public GameObject[] toTurnOff;
    public GameObject[] disable;
    public TMP_Text timeText;

    private bool countdownStarted = false;
    private void Update()
    {
        if (this.isActiveAndEnabled && !countdownStarted)
        {
            countdownStarted = true;
            StartCoroutine(timeUntil());
        }
    }

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
        if (isOnTrigger && !playerConnected.GetComponent<PlayerController>().isDead)
        {
            timeLeft = 20;
            countdownStarted = false;
            playerConnected.GetComponent<PhotonView>().RPC("RPC_Revived", RpcTarget.All);
            StopAllCoroutines();
            playerConnected.GetComponent<PlayerController>().isDead = false;
            foreach (GameObject obj in toTurnOff)
            {
                obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 1f, 0.6f, 0.24f);
            foreach (GameObject obj in disable)
            {
                obj.SetActive(true);
            }
        }
    }

    public int timeLeft = 20;
    IEnumerator timeUntil()
    {
        yield return new WaitForSeconds(1f);
        timeLeft -= 1;
        if(timeLeft <= 0)
        {
            playerConnected.GetComponent<PlayerController>().isDead = true;
            foreach(GameObject obj in toTurnOff)
            {
                obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
            foreach (GameObject obj in disable)
            {
                obj.SetActive(false);
            }
            playerConnected.GetComponent<PlayerController>().SetDeath();
        }
        timeText.text = "Time until dead: " + timeLeft + "s";
        StartCoroutine(timeUntil());
    }
}
