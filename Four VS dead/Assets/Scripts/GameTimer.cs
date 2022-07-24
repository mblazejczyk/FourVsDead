using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    int Seconds = 0;
    void Start()
    {
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        Seconds++;
        UpdateText();
        StartCoroutine(Tick());
    }

    void UpdateText()
    {
        string Final = "";
        if(Mathf.FloorToInt(Seconds/60) > 9)
        {
            Final = Mathf.FloorToInt(Seconds / 60) + ":";
        }
        else
        {
            Final = "0"+ Mathf.FloorToInt(Seconds / 60) + ":";
        }

        if (Seconds - Mathf.FloorToInt(Seconds / 60) * 60 > 9)
        {
            Final = Final + (Seconds - Mathf.FloorToInt(Seconds / 60) * 60);
        }
        else
        {
            Final = Final + "0" + (Seconds - Mathf.FloorToInt(Seconds / 60) * 60);
        }
        gameObject.GetComponent<TMP_Text>().text = Final;
    }
}
