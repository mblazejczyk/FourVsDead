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

    public GameObject FlashObj;


    public void Shoot(GameObject sprites)
    {
        RaycastHit2D hit = Physics2D.Raycast(sprites.transform.position, sprites.transform.TransformDirection(Vector2.up) * 10f);
        if (hit)
        {
            int CurrentGun = 0;
            for (int i = 0; i < Guns.Length; i++)
            {
                if(Guns[i].Id == CurrentGunId)
                {
                    CurrentGun = i;
                    break;
                }
            }
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Guns[CurrentGun].Damage);
        }

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("NewShoot", CurrentGunId);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == PV.Owner && (int)changedProps["NewShoot"] == 0)
        {
            StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        FlashObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        FlashObj.SetActive(false);
    }
}
