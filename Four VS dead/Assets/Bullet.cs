using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    PhotonView PV;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (PV.IsMine) { 
            StartCoroutine(Dest()); 
        }
    }

    IEnumerator Dest()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }
}
