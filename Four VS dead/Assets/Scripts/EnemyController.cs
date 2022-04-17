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
    public ParticleSystem bloodParticle;

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

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        StartCoroutine(bleed());
        Debug.Log("took gamage: " + damage);
        float temp = MaxHp - damage;
        HpBar.GetComponent<Transform>().localScale = new Vector3(temp / MaxHp, 0.25f, 0f);
        MaxHp = MaxHp - damage;
        HpText.GetComponent<TextMesh>().text = MaxHp.ToString();
        if(MaxHp <= 0 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    IEnumerator bleed()
    {
        bloodParticle.Play();
        yield return new WaitForSeconds(0.2f);
        bloodParticle.Stop();
    }
}
