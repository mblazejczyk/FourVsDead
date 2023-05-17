using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Pathfinding;

public class EnemyController_archer : MonoBehaviour, IDamagable
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
    public GameObject ShootLine;

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
        MaxHp = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().wave[
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().WaveNow-1].Hp;
        dodge = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().wave[
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().WaveNow-1].Dodge;
        damage = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().wave[
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().WaveNow-1].Dmg;
        speed = GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().wave[
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MatchController>().WaveNow-1].Speed;

        PV.RPC("RPC_Setup", RpcTarget.All, MaxHp, damage, dodge, speed);
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

            RaycastHit2D[] ShotgunHit = { Physics2D.Raycast(GunEnd.transform.position, transform.TransformDirection(Vector2.up) * 10f),
                    Physics2D.Raycast(GunEnd.transform.position, transform.TransformDirection(new Vector2(0.1f, 1)) * 5f),
                    Physics2D.Raycast(GunEnd.transform.position, transform.TransformDirection(new Vector2(-0.1f, 1)) * 5f),
                    Physics2D.Raycast(GunEnd.transform.position, transform.TransformDirection(new Vector2(0.2f, 1)) * 5f),
                    Physics2D.Raycast(GunEnd.transform.position, transform.TransformDirection(new Vector2(-0.2f, 1)) * 5f) };
            foreach (RaycastHit2D rc2d in ShotgunHit)
            {
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.TransformDirection(Vector2.up) * 10f);
                if (rc2d)
                {
                    if (rc2d.collider.gameObject.tag == "OnlinePlayer")
                    {
                        rb.velocity = Vector3.zero;
                        TryShoot(rc2d.collider.gameObject.GetComponent<Referencer>().Reference);
                        return;
                    }
                }
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

    private float damageCooldown = 3;
    private bool canDamage = true;
    private void TryShoot(GameObject player)
    {
        Debug.Log("trying to shoot");
        if (canDamage)
        {
            canDamage = false;
            PV.RPC("RPC_ShotMade", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
            StartCoroutine(damageCooldownTimer());
            PV.RPC("RPC_DrawShoot", RpcTarget.All, gameObject.transform.position, player.transform.position);
        }
    }

    [PunRPC]
    void RPC_ShotMade(int playerShot)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(obj.GetComponent<PhotonView>().ViewID == playerShot && obj.GetComponent<PhotonView>().IsMine)
            {
                obj.GetComponent<PlayerController>().ModifyHp(true, damage);
                break;
            }
        }
    }

    [PunRPC]
    void RPC_DrawShoot(Vector3 startPos, Vector3 endPos)
    {
        Instantiate(ShootLine).GetComponent<ShootLineController>().SetupLine(startPos, endPos, false);
    }

    IEnumerator damageCooldownTimer()
    {
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }

    IEnumerator soundLoop()
    {
        yield return new WaitForSeconds(Random.Range(15, 35));
        gameObject.GetComponent<AudioSource>().clip = idleSounds[Random.Range(0, idleSounds.Length)];
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(soundLoop());
    }
}
