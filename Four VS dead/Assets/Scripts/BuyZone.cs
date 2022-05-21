using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyZone : MonoBehaviour
{
    public int WeaponToBuyId = 0;
    private bool isOnTrigger = false;
    private Collider2D c2d;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag != "Player")
        {
            return;
        }
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetTrigger("Open");
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Press E to buy";
        Debug.Log("Triggered");
        isOnTrigger = true;
        c2d = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            return;
        }
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetTrigger("Close");
        isOnTrigger = false;
        c2d = null;
    }

    private void Update()
    {
        if(isOnTrigger == true && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(c2d.GetComponent<GunController>().Guns[WeaponToBuyId].Cost);
            Debug.Log(c2d.GetComponent<PlayerController>().Coins);
            if(c2d.GetComponent<GunController>().Guns[WeaponToBuyId].Cost > c2d.GetComponent<PlayerController>().Coins)
            {
                GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Not enaugh coins";
            }
            else
            {
                c2d.GetComponent<GunController>().ChangeGun(WeaponToBuyId);
                c2d.GetComponent<PlayerController>().ModifyCoins(0, c2d.GetComponent<GunController>().Guns[WeaponToBuyId].Cost);
            }
        }
    }
}
