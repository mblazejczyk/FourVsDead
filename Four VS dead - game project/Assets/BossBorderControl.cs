using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class BossBorderControl : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                collision.GetComponent<PlayerController>().ModifyHp(true, 1, 2);
            }
        }
    }
}
