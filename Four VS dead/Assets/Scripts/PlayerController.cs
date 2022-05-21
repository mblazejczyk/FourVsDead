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

    [Header("Uis")]
    public GameObject sprites;
    public GameObject[] PlayerUis;

    [Header("PlayerStats")]
    public float Hp = 100;
    public int Coins = 0;


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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            gameObject.GetComponent<GunController>().Shoot(sprites);
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) { return; }
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
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
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
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"]);
                    obj.GetComponent<PlayerInfoController>().SetCoin((int)changedProps["UiCoin"]);
                    obj.GetComponent<PlayerInfoController>().SetGun(gameObject.GetComponent<GunController>().Guns[(int)changedProps["UiGun"]].gunIcon);
                    break;
                }
            }
            foreach (GameObject obj in PlayerUis)
            { 
                if(obj.GetComponent<PlayerInfoController>().takenBy == (string)changedProps["UiName"])
                {
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"]);
                    obj.GetComponent<PlayerInfoController>().SetCoin((int)changedProps["UiCoin"]);
                    obj.GetComponent<PlayerInfoController>().SetGun(gameObject.GetComponent<GunController>().Guns[(int)changedProps["UiGun"]].gunIcon);
                }
            }
        }
    }
    void MoveAround(Vector2 direction)
    {
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
            }
            else
            {
                Coins -= Ammount;
            }
            Hashtable hash = new Hashtable();
            hash.Add("UiName", PhotonNetwork.NickName);
            hash.Add("UiHp", Hp);
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", gameObject.GetComponent<GunController>().CurrentGunId);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
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
            hash.Add("UiCoin", Coins);
            hash.Add("UiGun", GunId);
            hash.Add("UiHost", PhotonNetwork.IsMasterClient);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
}
