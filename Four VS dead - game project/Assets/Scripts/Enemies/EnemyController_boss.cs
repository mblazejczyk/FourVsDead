using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Pathfinding;

public class EnemyController_boss : MonoBehaviour, IDamagable
{
    PhotonView PV;
    public float MaxHp;
    public float dodge;
    public int damage;

    public GameObject Target;
    public float speed = 200f;
    public float nextDistance = 3f;

    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;

    Seeker seeker;
    Rigidbody2D rb;

    public GameObject HpBar;
    public GameObject HpText;
    public GameObject bullet;
    public GameObject GunEnd;
    public ParticleSystem bloodParticle;
    [Space(20)]
    public AudioClip[] idleSounds;
    public AudioClip[] dmgSfx;
    public GameObject damageObj;

    private void Awake()
    {
        if (reachedEnd) { }
        PV = GetComponent<PhotonView>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(soundLoop());
        if (PhotonNetwork.IsMasterClient)
        {
            SetUp();
            FindNewPlayer();

            InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }

    public void SetUp()
    {
        MaxHp = 1;
        dodge = 1;
        damage = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().wave[
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().WaveNow-1].Dmg;
        speed = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().wave[
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().WaveNow-1].Speed*2;

        PV.RPC("RPC_Setup", RpcTarget.All, MaxHp, damage, dodge, speed);
        StartCoroutine(setFire());
        StartCoroutine(SetBoundries());
    }

    [PunRPC]
    void RPC_Setup(float HpSet, int dmgSet, float dodgeSet, float speedSet)
    {
        MaxHp = HpSet;
        dodge = dodgeSet;
        damage = dmgSet;
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUpgradesController>().freezingAmm)
        {
            speed = speedSet * 0.6f;
        }
        else
        {
            speed = speedSet;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, Target.transform.position, OnPathCompleate);
        }
    }

    void OnPathCompleate(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FindNewPlayer()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("OnlinePlayer");
        Target = Players[Random.Range(0, Players.Length)];
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(Target == null) { FindNewPlayer(); }
            if(path == null) { return; }

            while (Target.GetComponent<Referencer>().Reference.GetComponent<PlayerController>().isDead)
            {
                FindNewPlayer();
                return;
            }

            if(currentWaypoint >= path.vectorPath.Count)
            {
                reachedEnd = true;
                return;
            }
            else
            {
                reachedEnd = false;
            }

            

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * rb.mass * Time.deltaTime;
            transform.up = Target.transform.position - transform.position; 
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextDistance)
            {
                currentWaypoint++;
            }
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
        //Debug.Log("took gamage: " + damage);
        float temp = MaxHp - damage;
        HpBar.GetComponent<Transform>().localScale = new Vector3(temp / MaxHp, 0.25f, 0f);
        MaxHp = MaxHp - damage;
        HpText.GetComponent<TextMesh>().text = MaxHp.ToString();
        damageObj.GetComponent<AudioSource>().clip = dmgSfx[4]; //for now lock for best
        damageObj.GetComponent<AudioSource>().Play();
        if (MaxHp <= 0 && PhotonNetwork.IsMasterClient)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().SubstractEnemiesLeft();
            GameObject.FindGameObjectWithTag("RewardSaver").GetComponent<RewardSaver>().zombieKilled += 1;
            PhotonNetwork.Destroy(gameObject);
        }
    }

    IEnumerator bleed()
    {
        bloodParticle.Play();
        yield return new WaitForSeconds(0.2f);
        bloodParticle.Stop();
    }

    public GameObject firePrefab;
    public GameObject pointA;
    public GameObject pointB;
    IEnumerator setFire()
    {
        yield return new WaitForSeconds(1.1f);
        float lerpValue = 0;
        float distance = 0.1f;
        for (int i = 0; i < 10; i++)
        {
            lerpValue += distance;
            Vector2 instantiatePosition = pointA.transform.position + (pointB.transform.position - pointA.transform.position) * lerpValue;
            Debug.Log(instantiatePosition);
            Debug.Log(lerpValue);
            PV.RPC("RPC_SetFire", RpcTarget.All, instantiatePosition.x, instantiatePosition.y);
        }
        StartCoroutine(setFire());
    }

    [PunRPC]
    void RPC_SetFire(float posX, float posY)
    {
        Instantiate(firePrefab).transform.position = new Vector2(posX, posY);
    }

    IEnumerator SetBoundries()
    {
        yield return new WaitForSeconds(1.2f);
        Vector2 centerPosition = gameObject.transform.position;
        float radius = 7f;
        for (int i = 0; i < 360; i += 10)
        {

            Vector2 spawnPosition;

            spawnPosition.x = (radius * Mathf.Cos(i * Mathf.Deg2Rad)) + centerPosition.x;
            spawnPosition.y = (radius * Mathf.Sin(i * Mathf.Deg2Rad)) + centerPosition.y;

            PV.RPC("RPC_SetBoundries", RpcTarget.All, spawnPosition.x, spawnPosition.y);

        }
        StartCoroutine(SetBoundries());
    }
    [PunRPC]
    void RPC_SetBoundries(float posX, float posY)
    {
        Instantiate(firePrefab).transform.position = new Vector3(posX, posY, 0);
    }

    IEnumerator soundLoop()
    {
        yield return new WaitForSeconds(Random.Range(15, 35));
        gameObject.GetComponent<AudioSource>().clip = idleSounds[Random.Range(0, idleSounds.Length)];
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(soundLoop());
    }
}
