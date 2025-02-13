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
    public int CurrentAmmo = 0;

    public GameObject Player_ani_sprite;
    public GameObject ShootOb;

    public GameObject ShootLine;
    public GameObject shootPoint;

    private void Start()
    {
        CurrentAmmo = Guns[CurrentGunId].maxAmmo;
    }

    public void Shoot(GameObject sprites)
    {
        if (GetComponent<PlayerController>().Hp == 0) { return; }
        if (isOnCooldown == true) { return; } else { isOnCooldown = true; }
        GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().shots += 1;
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
        if(CurrentAmmo <= 0){ isOnCooldown = false; return; } //no ammo
        CurrentAmmo--;
        ShootInfo(CurrentGun);
        switch (CurrentGunId)
        {
            case 0:
                PistolHit = Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (PistolHit)
                {
                    Debug.DrawRay(gameObject.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);

                    if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().fasterRate)
                    {
                        PistolHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage * 1.5f);
                    }
                    else
                    {
                        PistolHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                    }
                    if (PistolHit.collider.gameObject.tag == "Enemy")
                    {
                        AddXpForHit(5);
                        GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgGiven += Guns[CurrentGun].Damage;
                        gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                        DrawShoot(shootPoint.transform.position, PistolHit.collider.gameObject.transform.position, false);
                    }
                    else
                    {
                        DrawShoot(shootPoint.transform.position, GameObject.FindGameObjectWithTag("CursorController").transform.position, true);
                    }
                }
                break;
            case 1:
                RaycastHit2D[] ShotgunHit = { Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f),
                    Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(new Vector2(0.1f, 1)) * 5f),
                    Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(new Vector2(-0.1f, 1)) * 5f),
                    Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(new Vector2(0.2f, 1)) * 5f),
                    Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(new Vector2(-0.2f, 1)) * 5f) };
                foreach (RaycastHit2D rc2d in ShotgunHit)
                {
                    if (rc2d)
                    {
                        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().fasterRate)
                        {
                            rc2d.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage * 1.5f);
                        }
                        else
                        {
                            rc2d.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                        }


                        if (rc2d.collider.gameObject.tag == "Enemy")
                        {
                            AddXpForHit(3);
                            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgGiven += Guns[CurrentGun].Damage;
                            gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                            DrawShoot(shootPoint.transform.position, rc2d.collider.gameObject.transform.position, false);
                        }
                        else
                        {
                            DrawShoot(shootPoint.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f, true);
                        }
                    }
                }
                break;
            case 2:
                UziHit = Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (UziHit)
                {
                    if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().fasterRate)
                    {
                        UziHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage * 1.5f);
                    }
                    else
                    {
                        UziHit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                    }


                    if (UziHit.collider.gameObject.tag == "Enemy")
                    {
                        AddXpForHit(1);
                        GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgGiven += Guns[CurrentGun].Damage;
                        gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                        DrawShoot(shootPoint.transform.position, UziHit.collider.gameObject.transform.position, false);
                    }
                    else
                    {
                        DrawShoot(shootPoint.transform.position, GameObject.FindGameObjectWithTag("CursorController").transform.position, true);
                    }
                }
                break;
            case 3:
                BFG = Physics2D.Raycast(shootPoint.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
                if (BFG)
                {
                    if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().fasterRate)
                    {
                        BFG.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage * 1.5f);
                    }
                    else
                    {
                        BFG.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
                    }


                    if (BFG.collider.gameObject.tag == "Enemy")
                    {
                        AddXpForHit(15);
                        GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().dmgGiven += Guns[CurrentGun].Damage;
                        gameObject.GetComponent<PlayerController>().ModifyCoins(1, Guns[CurrentGun].CoinReward);
                        DrawShoot(shootPoint.transform.position, BFG.collider.gameObject.transform.position, false);
                    }
                    else
                    {
                        DrawShoot(shootPoint.transform.position, GameObject.FindGameObjectWithTag("CursorController").transform.position, true);
                    }
                }
                break;
            default:
                //hit = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 0f);
                break;
        }
        gameObject.GetComponent<PlayerController>().UpdateGunInfo(CurrentGunId);
        StartCoroutine(Cooldown(Guns[CurrentGun].ShootSpeed));
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("NewShoot", CurrentGunId);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    void DrawShoot(Vector3 startPos, Vector3 endPos, bool isMissed)
    {
        PV.RPC("RPC_DrawShoot", RpcTarget.All, startPos, endPos, isMissed);
    }

    [PunRPC]
    void RPC_DrawShoot(Vector3 startPos, Vector3 endPos, bool isMissed)
    {
        Instantiate(ShootLine).GetComponent<ShootLineController>().SetupLine(startPos, endPos, isMissed);
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
            if (changedProps["NewShoot"] != null)
            {
                StartCoroutine(ShootAni());
            }
            if (changedProps["GunChange"] != null)
            {
                CurrentGunId = (int)changedProps["GunChange"];
                gameObject.GetComponent<PlayerController>().UpdateGunInfo(CurrentGunId);
            }

        }
    }

    IEnumerator Cooldown(float time)
    {
        isOnCooldown = true;
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().fasterRate)
        {
            yield return new WaitForSeconds(time * 0.75f);
        }
        else
        {
            yield return new WaitForSeconds(time);
        }
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
            if(CurrentGunId == newGun)
            {
                CurrentAmmo = Guns[newGun].maxAmmo;
                gameObject.GetComponent<PlayerController>().UpdateGunInfo(CurrentGunId);
            }
            else
            {
                CurrentAmmo = Guns[newGun].maxAmmo;
                gameObject.GetComponent<AudioSource>().Play();
                Hashtable hash = new Hashtable();
                hash.Add("GunChange", newGun);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }
        //CurrentGunId = newGun;
    }
    public GameObject spr;
    public void ShootInfo(int currentGun)
    {
        gameObject.GetComponent<PhotonView>().RPC("RPC_ShootInfo", RpcTarget.All, currentGun);
    }

    [PunRPC]
    void RPC_ShootInfo(int currentGun)
    {
        GameObject g = Instantiate(ShootOb);
        g.GetComponent<DestroyMe>().toDestroy = true;
        g.GetComponent<DestroyMe>().Start();
        g.transform.SetParent(spr.transform);
        g.transform.position = ShootOb.transform.position;
        g.transform.localScale = ShootOb.transform.localScale;
        g.transform.rotation = ShootOb.transform.rotation;
        g.GetComponent<AudioSource>().clip = Guns[currentGun].shootSound;
        g.GetComponent<AudioSource>().Play();
    }
}
