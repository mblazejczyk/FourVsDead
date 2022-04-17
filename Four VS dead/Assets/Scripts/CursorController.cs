using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    private bool isLeaving = false;
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isLeaving) { return; }
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
        if (Input.GetKeyDown(KeyCode.Escape)) { StartCoroutine(quitGame()); }
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
            SceneManager.LoadScene(0);
        }
    }
}
