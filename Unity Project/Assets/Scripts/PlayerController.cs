using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public float speed = 10;
    public Vector2 movement;
    public Rigidbody2D rb;
    public GameObject TriggerAndReferencer;
    public GameObject deadTrigger;
    public GameObject player_ani_sprite;
    public GameObject tracker;
    public bool canMove = true;

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

    [Header("sounds")]
    public AudioClip[] playerDmg;

    [Header("Hair control")]
    public Color[] hairColors;
    public SpriteRenderer hair;
    public int hairChosen = 0;


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
        if (Input.GetKey(KeyCode.Mouse0) && !isDead && Hp != 0)
        {
            if (GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatSystem>().isChatOpen ||
                GameObject.FindGameObjectWithTag("CursorController").GetComponent<CursorController>().isPaused) { return; }
            gameObject.GetComponent<GunController>().Shoot(sprites);
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) { return; }
        if(Hp == 0) { return; }
        if(!canMove) { return; }
        if(GameObject.FindGameObjectWithTag("Chat").GetComponent<ChatSystem>().isChatOpen ||
            GameObject.FindGameObjectWithTag("CursorController").GetComponent<CursorController>().isPaused) { return; }
        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            player_ani_sprite.GetComponent<Animator>().SetBool("isWalking", false);
            rb.velocity = Vector3.zero;
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
            if(GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startCoins > 0)
            {
                Coins = GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startCoins;
            }
            if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startHp > 0)
            {
                Hp += GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startHp;
                MaxHp += GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startHp;
            }
            if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startSpeed > 0)
            {
                speed += speed * GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startSpeed / 100;
            }
            if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startDodge > 0)
            {
                dodgeChance *= GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().startDodge / 100;
            }
            hairChosen = Random.Range(0, hairColors.Length);

            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            hash.Add("hairColor", hairChosen);
            hash.Add("curAmmo", gameObject.GetComponent<GunController>().CurrentAmmo);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            SetColor();
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
                if (!obj.activeSelf && obj.GetComponent<PlayerInfoController>().takenBy == "")
                {
                    obj.SetActive(true);
                    obj.GetComponent<PlayerInfoController>().SetPlayer((string)changedProps["UiName"], (bool)changedProps["UiHost"]);
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"], (float)changedProps["UiMaxHp"]);
                    obj.GetComponent<PlayerInfoController>().SetCoin((int)changedProps["UiCoin"]);
                    obj.GetComponent<PlayerInfoController>().setColor(hairColors[(int)changedProps["hairColor"]]);
                    obj.GetComponent<PlayerInfoController>().SetArmor(gameObject.GetComponent<ArmorController>().armors[(int)changedProps["UiArmor"]].armorIcon);
                    obj.GetComponent<PlayerInfoController>().SetGun(gameObject.GetComponent<GunController>().Guns[(int)changedProps["UiGun"]].gunIcon);
                    obj.GetComponent<PlayerInfoController>().SetAmmo((int)changedProps["curAmmo"], gameObject.GetComponent<GunController>().Guns[gameObject.GetComponent<GunController>().CurrentGunId].maxAmmo);

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
                    obj.GetComponent<PlayerInfoController>().setColor(hairColors[(int)changedProps["hairColor"]]);
                    obj.GetComponent<PlayerInfoController>().SetAmmo((int)changedProps["curAmmo"], gameObject.GetComponent<GunController>().Guns[gameObject.GetComponent<GunController>().CurrentGunId].maxAmmo);

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
                if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().coinsAdded > 0)
                {
                    Ammount += Ammount * GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().coinsAdded / 100;
                }
                Coins += Ammount;
                GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().coinsCollected += Ammount;
            }
            else
            {
                Coins -= Ammount;
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
            hash.Add("hairColor", hairChosen);
            hash.Add("curAmmo", gameObject.GetComponent<GunController>().CurrentAmmo);
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
            hash.Add("hairColor", hairChosen);
            hash.Add("curAmmo", gameObject.GetComponent<GunController>().CurrentAmmo);
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
            hash.Add("hairColor", hairChosen);
            hash.Add("curAmmo", gameObject.GetComponent<GunController>().CurrentAmmo);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void SetDeath()
    {
        if (PV.IsMine)
        {
            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().deaths += 1;
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiMaxHp", MaxHp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiArmor", gameObject.GetComponent<ArmorController>().currentArmor);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            hash.Add("isDead", isDead);
            hash.Add("hairColor", hairChosen);
            hash.Add("curAmmo", gameObject.GetComponent<GunController>().CurrentAmmo);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    [PunRPC]
    void RPC_Revived()
    {
        if (PV.IsMine)
        {
            ModifyHp(false, 50, 0, true);
        }
    }

    public Sprite[] damageUis;
    public void ModifyHp(bool isDamaging, int hpChanged, int couse, bool isReviving)
    {
        if (PV.IsMine)
        {
            if (isDamaging)
            {
                if (Random.value * 100 > dodgeChance)
                {
                    hpChanged -= (int)(hpChanged * (dmgReductionProc * 0.01f));
                    Hp -= hpChanged;
                    GameObject.FindGameObjectWithTag("UiInfoBg").GetComponent<Animator>().SetTrigger("dmg");
                    player_ani_sprite.GetComponent<AudioSource>().clip = playerDmg[Random.Range(0, playerDmg.Length)];
                    player_ani_sprite.GetComponent<AudioSource>().Play();
                    GameObject dmgUi = GameObject.FindGameObjectsWithTag("damageUi")[Random.Range(0, GameObject.FindGameObjectsWithTag("damageUi").Length)];
                    dmgUi.GetComponent<Image>().sprite = damageUis[couse];
                    dmgUi.GetComponent<Animator>().SetTrigger("Dmg");
                    GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgTaken += hpChanged;
                }
                else
                {
                    Debug.Log("dodged");
                }
            }
            else
            {
                if (isReviving || Hp > 0)
                {
                    Hp += hpChanged;
                }
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
            hash.Add("hairColor", hairChosen);
            hash.Add("curAmmo", gameObject.GetComponent<GunController>().CurrentAmmo);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            PV.RPC("RPC_changePlayerHp", RpcTarget.All, isDamaging, hpChanged);
            if(Hp == 0)
            {
                GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().knockouts += 1;
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

    void SetColor()
    {
        Debug.Log("Send set color: " + hairChosen);
        PV.RPC("RPC_setColor", RpcTarget.All, hairChosen);
    }

    [PunRPC]
    void RPC_setColor(int co)
    {
        hair.color = hairColors[co];
    }
}
