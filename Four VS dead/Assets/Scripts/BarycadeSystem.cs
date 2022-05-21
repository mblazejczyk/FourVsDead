using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using TMPro;

public class BarycadeSystem : MonoBehaviour, IBarycadeDmg
{
    PhotonView PV;

    public bool isDestroing = false;
    public bool isRepairing = false;

    public TMP_Text hpText;

    public int Hp = 5;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", true);
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "You're repairing barycade";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            isRepairing = true;
            StartCoroutine(RepairTimer());
        }
        else if(collision.tag == "Enemy"){
            isDestroing = true;
            StartCoroutine(DestroyTimer());
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Animator>().SetBool("isOpen", false);
            GameObject.FindGameObjectWithTag("InfoBox").GetComponent<Referencer>().Reference.GetComponent<TMP_Text>().text = "---";
            isRepairing = false;
        }
        else if (collision.tag == "Enemy")
        {
            isDestroing = false;
        }
        else
        {
            return;
        }
    }

    IEnumerator RepairTimer()
    {
        yield return new WaitForSeconds(1);
        if (isDestroing != true && Hp < 6 && isRepairing == true)
        {
            ChangeHp(false, 1);
        }
        StartCoroutine(RepairTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(1);
        if(isDestroing == true && Hp > 0)
        {
            ChangeHp(true, 1);
        }
        StartCoroutine(DestroyTimer());
    }

    public void ChangeHp(bool isLoosing, int dmg)
    {
        PV.RPC("RPC_ChangeHp", RpcTarget.All, isLoosing, dmg);
    }

    [PunRPC]
    void RPC_ChangeHp(bool isLoosing, int dmg)
    {
        if (isLoosing)
        {
            Hp -= dmg;
        }
        else
        {
            Hp += dmg;
        }
        HpChecker();
    }

    void HpChecker()
    {
        if(Hp == 0)
        {
            gameObject.GetComponent<Referencer>().Reference.GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<Referencer>().Reference.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<Referencer>().Reference.GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.GetComponent<Referencer>().Reference.GetComponent<SpriteRenderer>().color = new Color(0.76f, 0.42f, 0);
        }
        hpText.text = Hp + "/6";
    }
}
