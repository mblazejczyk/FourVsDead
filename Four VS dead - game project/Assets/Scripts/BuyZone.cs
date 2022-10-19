using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class BuyZone : MonoBehaviour
{
    public enum typeToBuy { Gun, Armor}
    public typeToBuy buyType;

    public int TypeId = 0;
    private bool isOnTrigger = false;
    private Collider2D c2d;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag != "Player" || !collision.GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Press E to buy";

        isOnTrigger = true;
        c2d = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player" || !collision.GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        isOnTrigger = false;
        c2d = null;
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", false);
    }

    private void Update()
    {
        if(isOnTrigger == true && Input.GetKeyDown(KeyCode.E))
        {
            if (buyType == typeToBuy.Armor)
            {
                if (c2d.GetComponent<ArmorController>().armors[TypeId].Cost > c2d.GetComponent<PlayerController>().Coins)
                {
                    GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Not enaugh coins";
                }
                else
                {
                    c2d.GetComponent<ArmorController>().SetNewArmor(TypeId);
                    c2d.GetComponent<PlayerController>().ModifyCoins(0, c2d.GetComponent<ArmorController>().armors[TypeId].Cost);
                    GameObject.FindGameObjectWithTag("UiInfoBg").GetComponent<Animator>().SetTrigger("buy");
                    gameObject.GetComponent<AudioSource>().Play();
                }
            }
            else if (buyType == typeToBuy.Gun)
            {
                if (c2d.GetComponent<GunController>().Guns[TypeId].Cost > c2d.GetComponent<PlayerController>().Coins)
                {
                    GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Not enaugh coins";
                }
                else
                {
                    c2d.GetComponent<GunController>().ChangeGun(TypeId);
                    c2d.GetComponent<PlayerController>().ModifyCoins(0, c2d.GetComponent<GunController>().Guns[TypeId].Cost);
                    GameObject.FindGameObjectWithTag("UiInfoBg").GetComponent<Animator>().SetTrigger("buy");
                    gameObject.GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
