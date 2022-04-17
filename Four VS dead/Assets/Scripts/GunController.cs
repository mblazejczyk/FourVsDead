using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class GunController : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    [Header("Gun options")]
    public int CurrentGunId = 0;
    public Gun[] Guns;
    public bool isOnCooldown = false;

    public GameObject FlashObj;

    
    public void Shoot(GameObject sprites)
    {
        if(isOnCooldown == true) { return; } else { isOnCooldown = true; }
        RaycastHit2D PistolHit;
        RaycastHit2D UziHit;
        
        RaycastHit2D BFG;
        int CurrentGun = 0;
        for (int i = 0; i < Guns.Length; i++)
        {
            if (Guns[i].Id == CurrentGunId)
            {
                CurrentGun = i;
                break;
            }
        }
        switch (CurrentGunId)
        {
            case 0:
                PistolHit = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (PistolHit)
                {
                    PistolHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                }
                break;
            case 1:
                RaycastHit2D[] ShotgunHit = { Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f),
                    Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(new Vector2(0.1f, 1)) * 5f),
                    Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(new Vector2(-0.1f, 1)) * 5f),
                    Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(new Vector2(0.2f, 1)) * 5f),
                    Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(new Vector2(-0.2f, 1)) * 5f) };
                foreach(RaycastHit2D rc2d in ShotgunHit)
                {
                    if (rc2d)
                    {
                        rc2d.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                    }
                }
                break;
            case 2:
                UziHit = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (UziHit)
                {
                    UziHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                }
                break;
            case 3:
                BFG = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (BFG)
                {
                    BFG.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                }
                break;
            default:
                //hit = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 0f);
                break;
        }
        StartCoroutine(Cooldown(Guns[CurrentGun].ShootSpeed));
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("NewShoot", CurrentGunId);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PV.Owner)
        {
            if (changedProps["NewShoot"] != null) {
                switch ((int)changedProps["NewShoot"])
                {
                    case 0:
                        StartCoroutine(Flash(0.2f));
                        break;
                    case 1:
                        StartCoroutine(Flash(0.2f));
                        break;
                    case 2:
                        StartCoroutine(Flash(0.1f));
                        break;
                    case 3:
                        StartCoroutine(Flash(0.1f));
                        break;
                }
            }
            if(changedProps["GunChange"] != null)
            {
                CurrentGunId = (int)changedProps["GunChange"];
            }
        }
    }

    IEnumerator Cooldown(float time)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(time);
        isOnCooldown = false;
    }

    IEnumerator Flash(float speed)
    {
        FlashObj.SetActive(true);
        yield return new WaitForSeconds(speed);
        FlashObj.SetActive(false);
    }

    public void ChangeGun(int newGun)
    {
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("GunChange", newGun);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        //CurrentGunId = newGun;
    }
}
