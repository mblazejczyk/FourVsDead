using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxPlayerCheck : MonoBehaviour
{
    public void Check()
    {
        if(byte.Parse(gameObject.GetComponent<TMP_InputField>().text) < 1)
        {
            gameObject.GetComponent<TMP_InputField>().text = "1";
        }
        else if (byte.Parse(gameObject.GetComponent<TMP_InputField>().text) > 4)
        {
            gameObject.GetComponent<TMP_InputField>().text = "4";
        }else if (byte.Parse(gameObject.GetComponent<TMP_InputField>().text) <= 4 &&
            byte.Parse(gameObject.GetComponent<TMP_InputField>().text) >= 1)
        {

        }
        else
        {
            gameObject.GetComponent<TMP_InputField>().text = "4";
        }
    }
}
