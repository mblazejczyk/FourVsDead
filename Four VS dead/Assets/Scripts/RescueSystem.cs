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
    public int timeLeft = 20;

    public bool knockedOut = false;
    public void KnockOut()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 1f, 0.6f, 0.24f);
        foreach(GameObject obj in disable)
        {
            obj.SetActive(true);
        }
        foreach(GameObject obj in toTurnOff)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        timeLeft = 20;
        knockedOut = true;
        StartCoroutine(knockoutTimer());
    }

    public bool isHelping = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player") { return; }
        if (collision.GetComponent<PlayerController>().isDead) { return; }
        if (!knockedOut) { return; }
        StartCoroutine(helpTimer());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (collision.GetComponent<PlayerController>().isDead) { return; }
        if (!knockedOut) { return; }

        isHelping = false;
    }

    IEnumerator helpTimer()
    {
        isHelping = true;
        yield return new WaitForSeconds(3);
        if (isHelping)
        {
            playerConnected.GetComponent<PhotonView>().RPC("RPC_Revived", RpcTarget.All);
            playerConnected.GetComponent<PlayerController>().isDead = false;
            foreach (GameObject obj in toTurnOff)
            {
                obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 1f, 0.6f, 0f);
            foreach (GameObject obj in disable)
            {
                obj.SetActive(false);
            }
            isHelping = false;
            knockedOut = false;
            timeLeft = 20;
        }
    }

    IEnumerator knockoutTimer()
    {
        yield return new WaitForSeconds(1);
        if (knockedOut)
        {
            timeText.text = "Time until dead: " + timeLeft + "s";
            timeLeft -= 1;
            if (timeLeft <= 0)
            {
                Killed();
            }
            else
            {
                StartCoroutine(knockoutTimer());
            }
        }
    }

    void Killed()
    {
        playerConnected.GetComponent<PlayerController>().isDead = true;
        foreach (GameObject obj in toTurnOff)
        {
            obj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        foreach (GameObject obj in disable)
        {
            obj.SetActive(false);
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f, 1f, 0.6f, 0f);
        playerConnected.GetComponent<PlayerController>().SetDeath();
    }
}