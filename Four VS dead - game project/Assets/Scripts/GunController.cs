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

    public GameObject Player_ani_sprite;

    
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
                    
                    if(PistolHit.collider.gameObject.tag == "Enemy")
                    {
                        AddXpForHit(5);
                        gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                    }
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
                        
                        if (rc2d.collider.gameObject.tag == "Enemy")
                        {
                            AddXpForHit(3);
                            gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                        }
                    }
                }
                break;
            case 2:
                UziHit = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (UziHit)
                {
                    UziHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                    
                    if (UziHit.collider.gameObject.tag == "Enemy")
                    {
                        AddXpForHit(1);
                        gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                    }
                }
                break;
            case 3:
                BFG = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (BFG)
                {
                    BFG.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                    
                    if (BFG.collider.gameObject.tag == "Enemy")
                    {
                        AddXpForHit(15);
                        gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                    }
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

    void AddXpForHit(int ammount)
    {
        if (GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().XpGame > 0)
        {
            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted += ammount +
                (int)((float)ammount * ((float)GameObject.FindGameObjectWithTag("LoginHandler").GetComponent<loginHandler>().XpGame / 100));
        }
        else
        {
            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().xpGranted += ammount;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PV.Owner)
        {
            if (changedProps["NewShoot"] != null) {
                StartCoroutine(ShootAni());
            }
            if(changedProps["GunChange"] != null)
            {
                CurrentGunId = (int)changedProps["GunChange"];
                gameObject.GetComponent<PlayerController>().UpdateGunInfo(CurrentGunId);
            }
            
        }
    }

    IEnumerator Cooldown(float time)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(time);
        isOnCooldown = false;
    }

    IEnumerator ShootAni()
    {
        Player_ani_sprite.GetComponent<Animator>().SetBool("isShooting", true);
        yield return new WaitForSeconds(0.4f);
        Player_ani_sprite.GetComponent<Animator>().SetBool("isShooting", false);
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
