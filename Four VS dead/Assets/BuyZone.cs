using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyZone : MonoBehaviour
{
    public int WeaponToBuyId = 0;
    private bool isOnTrigger = false;
    private Collider2D c2d;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name != "TriggerAndReferencer")
        {
            return;
        }
        Debug.Log("Triggered");
        isOnTrigger = true;
        c2d = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name != "TriggerAndReferencer")
        {
            return;
        }
        isOnTrigger = false;
        c2d = null;
    }

    private void Update()
    {
        if(isOnTrigger == true && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(c2d.GetComponent<Referencer>().Reference.GetComponent<GunController>().Guns[WeaponToBuyId].Cost);
            Debug.Log(c2d.GetComponent<Referencer>().Reference.GetComponent<PlayerController>().Coins);
            if(c2d.GetComponent<Referencer>().Reference.GetComponent<GunController>().Guns[WeaponToBuyId].Cost > c2d.GetComponent<Referencer>().Reference.GetComponent<PlayerController>().Coins)
            {
                Debug.Log("Not enaugh money");
            }
            else
            {
                c2d.GetComponent<Referencer>().Reference.GetComponent<GunController>().ChangeGun(WeaponToBuyId);
                c2d.GetComponent<Referencer>().Reference.GetComponent<PlayerController>().ModifyCoins(0, c2d.GetComponent<Referencer>().Reference.GetComponent<GunController>().Guns[WeaponToBuyId].Cost);
            }
        }
    }
}
