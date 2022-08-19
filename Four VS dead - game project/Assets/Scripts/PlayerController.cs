using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public float speed = 10;
    public Vector2 movement;
    public Rigidbody2D rb;
    public GameObject TriggerAndReferencer;
    public GameObject deadTrigger;
    public GameObject player_ani_sprite;

    [Header("Uis")]
    public GameObject sprites;
    public GameObject[] PlayerUis;

    [Header("PlayerStats")]
    public float Hp = 100;
    public float MaxHp = 100;
    public int Coins = 0;
    public bool isDead = false;
    public float dmgReductionProc = 0;
    public float dodgeChance = 0;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PlayerUis[0] = GameObject.Find("Player1");
            PlayerUis[1] = GameObject.Find("Player2");
            PlayerUis[2] = GameObject.Find("Player3");
            PlayerUis[3] = GameObject.Find("Player4");
            foreach(GameObject obj in PlayerUis)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            Destroy(TriggerAndReferencer);
        }
    }
    private void Update()
    {
        if (!PV.IsMine) { return; }
        LookAround();
        if (Input.GetKey(KeyCode.Mouse0) && !isDead)
        {
            gameObject.GetComponent<GunController>().Shoot(sprites);
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) { return; }
        if(Hp == 0) { return; }
        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            player_ani_sprite.GetComponent<Animator>().SetBool("isWalking", false);
            return;
        }
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MoveAround(movement);
    }

    private void Start()
    {
        if (!PV.IsMine) {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
        else
        {
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps["UiName"] != null && PV.IsMine)
        {
            foreach(GameObject obj in PlayerUis)
            {
                if(obj.GetComponent<PlayerInfoController>().takenBy == (string)changedProps["UiName"])
                {
                    break;
                }
                if (!obj.active && obj.GetComponent<PlayerInfoController>().takenBy == "")
                {
                    obj.SetActive(true);
                    obj.GetComponent<PlayerInfoController>().SetPlayer((string)changedProps["UiName"], (bool)changedProps["UiHost"]);
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"], (float)changedProps["UiMaxHp"]);
                    obj.GetComponent<PlayerInfoController>().SetCoin((int)changedProps["UiCoin"]);
                    obj.GetComponent<PlayerInfoController>().SetArmor(gameObject.GetComponent<ArmorController>().armors[(int)changedProps["UiArmor"]].armorIcon);
                    obj.GetComponent<PlayerInfoController>().SetGun(gameObject.GetComponent<GunController>().Guns[(int)changedProps["UiGun"]].gunIcon);
                    if ((bool)changedProps["isDead"])
                    {
                        obj.GetComponent<PlayerInfoController>().setDeath(true);
                    }
                    else
                    {
                        obj.GetComponent<PlayerInfoController>().setDeath(false);
                    }
                    break;
                }
            }
            foreach (GameObject obj in PlayerUis)
            { 
                if(obj.GetComponent<PlayerInfoController>().takenBy == (string)changedProps["UiName"])
                {
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"], (float)changedProps["UiMaxHp"]);
                    obj.GetComponent<PlayerInfoController>().SetCoin((int)changedProps["UiCoin"]);
                    if ((bool)changedProps["isDead"])
                    {
                        obj.GetComponent<PlayerInfoController>().setDeath(true);
                    }
                    else
                    {
                        obj.GetComponent<PlayerInfoController>().setDeath(false);
                    }
                    obj.GetComponent<PlayerInfoController>().SetGun(gameObject.GetComponent<GunController>().Guns[(int)changedProps["UiGun"]].gunIcon);
                    obj.GetComponent<PlayerInfoController>().SetArmor(gameObject.GetComponent<ArmorController>().armors[(int)changedProps["UiArmor"]].armorIcon);
                }
            }
        }
    }
    void MoveAround(Vector2 direction)
    {
        player_ani_sprite.GetComponent<Animator>().SetBool("isWalking", true);
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }


    void LookAround()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2) transform.position).normalized;
        sprites.transform.up = direction;
    }

    public void ModifyCoins(int addOrRemove, int Ammount)
    {
        if (PV.IsMine)
        {
            if (addOrRemove == 1)
            {
                Coins += Ammount;
                GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted += Ammount;
            }
            else
            {
                Coins -= Ammount;
                GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted -= Ammount;
            }
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void UpdateGunInfo(int GunId)
    {
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", GunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void UpdateArmorInfo(int ArmorId)
    {
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", ArmorId);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void SetDeath()
    {
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    [PunRPC]
    void RPC_Revived()
    {
        if (PV.IsMine)
        {
            ModifyHp(false, 50);
        }
    }

    public void ModifyHp(bool isDamaging, int hpChanged)
    {
        if (PV.IsMine)
        {
            if (isDamaging)
            {
                if (Random.value > dodgeChance)
                {
                    hpChanged -= (int)(hpChanged * (dmgReductionProc * 0.01f));
                    Hp -= hpChanged;
                    Debug.Log(hpChanged);
                }
                else
                {
                    Debug.Log("dodged");
                }
            }
            else
            {
                Hp += hpChanged;
            }
            if(Hp < 0) { Hp = 0; }
            if(Hp > MaxHp) { Hp = MaxHp; }
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            PV.RPC("RPC_changePlayerHp", RpcTarget.All, isDamaging, hpChanged);
            if(Hp == 0)
            {
                PV.RPC("RPC_KnockOut", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void RPC_changePlayerHp(bool isTaking, int dmg)
    {
        if (PV.IsMine) { return; }
        if (isTaking)
        {
            Hp -= dmg;
        }
        else
        {
            Hp += dmg;
        }
    }

    [PunRPC]
    void RPC_KnockOut()
    {
        if(deadTrigger.GetComponent<RescueSystem>().knockedOut || isDead) { return; }
        deadTrigger.GetComponent<RescueSystem>().KnockOut();
    }
}
