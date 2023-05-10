using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CursorController : MonoBehaviour
{
    public bool isLeaving = false;
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isPaused) { return; }
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu();
        }
    }

    public void LeaveGame()
    {
        StartCoroutine(HostLeftQuit());
    }

    public GameObject pausemenuobj;
    public bool isPaused = false;
    public void PauseMenu()
    {
        if (isPaused)
        {
            Cursor.visible = false;
            isPaused = false;
            pausemenuobj.SetActive(false);
        }
        else
        {
            Cursor.visible = true;
            isPaused = true;
            pausemenuobj.SetActive(true);
        }
    }

    public void Quit()
    {
        StartCoroutine(quitGame());
    }


    IEnumerator HostLeftQuit()
    {
        isLeaving = true;
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        Destroy(GameObject.FindGameObjectWithTag("RoomManager"));
        SceneManager.LoadScene(1);
    }

    IEnumerator quitGame()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        Destroy(GameObject.FindGameObjectWithTag("RoomManager"));
        SceneManager.LoadScene(1);
    }

    public Transform PlayerListContainter;
    public GameObject newPlayerListItemObj;
    public void ShowPlayers(){
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(newPlayerListItemObj, PlayerListContainter).GetComponent<PlayerListitem>().SetUp(players[i]);
        }
    }
}
