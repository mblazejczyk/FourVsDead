using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public Vector2 speed = new Vector2(50, 50);
    public GameObject sprites;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!PV.IsMine) { return; }
        MoveAround();
        LookAround();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            gameObject.GetComponent<GunController>().Shoot(sprites);
        }
    }

    private void Start()
    {
        if (!PV.IsMine) {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }

    }
    void MoveAround()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement);
    }


    void LookAround()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseScreenPosition - (Vector2) transform.position).normalized;
        sprites.transform.up = direction;
    }
}
