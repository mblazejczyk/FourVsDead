using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    public bool isLeaving = false;
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isLeaving) { return; }
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;

        if (Input.GetKeyDown(KeyCode.T)) {
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(quitGame()); }
    }

    public void LeaveGame()
    {
        StartCoroutine(HostLeftQuit());
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
        yield return new WaitForSeconds(3);
        if (Input.GetKey(KeyCode.Escape)) {
            isLeaving = true;
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
            Destroy(GameObject.FindGameObjectWithTag("RoomManager"));
            SceneManager.LoadScene(1);
        }
    }

}
