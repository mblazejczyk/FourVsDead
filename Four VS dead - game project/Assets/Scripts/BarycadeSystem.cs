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
    public bool isActivated = false;

    public Sprite[] DestroyLevel;
    
    public int Hp = 5;
    public AudioClip[] destroySfx;
    public AudioClip[] repairSfx;

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
        if (collision.tag == "Enemy" && isDestroing == false)
        {
            isDestroing = true;
            StartCoroutine(DestroyTimer());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PhotonView>().IsMine)
        {
            isRepairing = true;
            StartCoroutine(RepairTimer(collision));
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
    IEnumerator RepairTimer(Collider2D player)
    {
        yield return new WaitForSeconds(1);
        if (isDestroing != true && Hp < 6 && isRepairing == true)
        {
            ChangeHp(false, 1);
            player.GetComponent<PlayerController>().ModifyCoins(1, 15);
        }
        StartCoroutine(RepairTimer(player));
    }

    private bool isAlreadyDes = false;
    IEnumerator DestroyTimer()
    {
        if (isAlreadyDes) { yield break; }
        isAlreadyDes = true;
        yield return new WaitForSeconds(1);
        if(isDestroing == true && Hp > 0)
        {
            ChangeHp(true, 1);
        }
        isAlreadyDes = false;
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
            gameObject.GetComponent<AudioSource>().clip = destroySfx[Random.Range(0, destroySfx.Length)];
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            Hp += dmg;
            gameObject.GetComponent<AudioSource>().clip = repairSfx[Random.Range(0, repairSfx.Length)];
            gameObject.GetComponent<AudioSource>().Play();
        }
        HpChecker();
    }

    void HpChecker()
    {
        if(Hp < 0) { Hp = 0; }
        if(Hp > 6) { Hp = 6; }
        if(Hp == 0)
        {
            gameObject.GetComponent<Referencer>().Reference.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            gameObject.GetComponent<Referencer>().Reference.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        gameObject.GetComponent<Referencer>().Reference.GetComponent<SpriteRenderer>().sprite = DestroyLevel[Hp];
    }
}
