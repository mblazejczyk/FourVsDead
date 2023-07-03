using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(destroyme());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<PlayerController>().ModifyHp(true, 3, 2, false);
            }
        }
    }

    IEnumerator destroyme()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
