using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PingAndFps : MonoBehaviour
{
    public TMP_Text Fps;
    public TMP_Text Ping;

    private float FPSs = 0;
    private int howMany = 0;

    private void Start()
    {
        StartCoroutine(FpsSmoothner());
    }
    private void Update()
    {
        Ping.text = PhotonNetwork.GetPing() + " ms";
        FPSs += 1.0f / Time.deltaTime;
        howMany++;
    }

    IEnumerator FpsSmoothner()
    {
        yield return new WaitForSeconds(0.5f);
        Fps.text = (int)(FPSs / howMany) + " FPS";
        howMany = 0;
        FPSs = 0;
        StartCoroutine(FpsSmoothner());
    }
}
