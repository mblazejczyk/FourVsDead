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

    public Vector2 speed = new Vector2(50, 50);
    public GameObject sprites;
    public GameObject[] PlayerUis;
    [Header("PlayerStats")]
    public float Hp = 100;
    


    private void Awake()
    {
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
    }
    private void Update()
    {
        if (!PV.IsMine) { return; }
        MoveAround();
        LookAround();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            gameObject.GetComponent<GunController>().Shoot(sprites);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            gameObject.GetComponent<GunController>().ChangeGun(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameObject.GetComponent<GunController>().ChangeGun(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameObject.GetComponent<GunController>().ChangeGun(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameObject.GetComponent<GunController>().ChangeGun(3);
        }
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
                if (!obj.active && obj.GetComponent<PlayerInfoController>().takenBy == "")
                {
                    obj.SetActive(true);
                    obj.GetComponent<PlayerInfoController>().SetPlayer((string)changedProps["UiName"], (bool)changedProps["UiHost"]);
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"]);
                    break;
                }
            }
            foreach (GameObject obj in PlayerUis)
            { 
                if(obj.GetComponent<PlayerInfoController>().takenBy == (string)changedProps["UiName"])
                {
                    obj.GetComponent<PlayerInfoController>().SetHp((float)changedProps["UiHp"]);
                }
            }
        }
    }
    void MoveAround()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement);
    }


    void LookAround()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2) transform.position).normalized;
        sprites.transform.up = direction;
    }
}
