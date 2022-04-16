using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EnemyController : MonoBehaviour, IDamagable
{
    PhotonView PV;
    public float MaxHp;
    public GameObject Target;
    public GameObject HpBar;
    public GameObject HpText;
    public GameObject bullet;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FindNewPlayer();
        }
    }

    

    void FindNewPlayer()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("OnlinePlayer");
        Target = Players[Random.Range(0, Players.Length)];
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            transform.up = Target.transform.position - transform.position;
            gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up;
        }
    }

    

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }
    public void PrintBullet(float SrcX, float SrcY, float SrcZ, float RotX, float RotY, float RotZ, float RotW)
    {
        PV.RPC("RPC_PrintBullet", RpcTarget.All, SrcX, SrcY, SrcZ, RotX, RotY, RotZ, RotW);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        Debug.Log("took gamage: " + damage);
        float temp = MaxHp - damage;
        HpBar.GetComponent<Transform>().localScale = new Vector3(temp / MaxHp, 0.25f, 0f);
        MaxHp = MaxHp - damage;
        HpText.GetComponent<TextMesh>().text = MaxHp.ToString();
        if(MaxHp == 0 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    
}
