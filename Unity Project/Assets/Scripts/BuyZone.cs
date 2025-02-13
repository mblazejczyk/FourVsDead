using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class BuyZone : MonoBehaviour
{
    public enum typeToBuy { Gun, Armor, Upgrade}
    public typeToBuy buyType;

    public int TypeId = 0;
    private bool isOnTrigger = false;
    private Collider2D c2d;
    [Space(20)]
    public bool ShouldUpgradeBeDestroyed = false;

    [TextAreaAttribute]
    public string Description;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag != "Player" || !collision.GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
        GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Press E to buy";

        GameObject.Find("ShopInfo").GetComponent<Animator>().SetBool("ShouldBeOpen", true);
        GameObject.Find("ShopInfo").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = Description;

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
        GameObject.Find("ShopInfo").GetComponent<Animator>().SetBool("ShouldBeOpen", false);
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
                    GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().buys += 1;
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
                    GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().buys += 1;
                    c2d.GetComponent<PlayerController>().ModifyCoins(0, c2d.GetComponent<GunController>().Guns[TypeId].Cost);
                    GameObject.FindGameObjectWithTag("UiInfoBg").GetComponent<Animator>().SetTrigger("buy");
                    gameObject.GetComponent<AudioSource>().Play();
                }
            }else if(buyType == typeToBuy.Upgrade)
            {
                int currentCost = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().CostsById[TypeId];
                if (currentCost > c2d.GetComponent<PlayerController>().Coins)
                {
                    GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "Not enaugh coins";
                }
                else
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().BuyUpgrade(TypeId);
                    c2d.GetComponent<PlayerController>().ModifyCoins(0, currentCost);
                    GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().buys += 1;
                    GameObject.FindGameObjectWithTag("UiInfoBg").GetComponent<Animator>().SetTrigger("buy");
                    gameObject.GetComponent<AudioSource>().Play();
                    if (ShouldUpgradeBeDestroyed)
                    {
                        PhotonView PV = gameObject.GetComponent<PhotonView>();
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }
        }
    }
}
