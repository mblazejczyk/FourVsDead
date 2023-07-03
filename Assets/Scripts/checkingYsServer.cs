using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkingYsServer : MonoBehaviour
{
    public TMP_Text text;
    void Start()
    {
        string sql = "SELECT * FROM `connectionCheck`";
        gameObject.GetComponent<SqlController>().Send(sql, "why");
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<SqlController>().restults == "open")
        {
            Destroy(gameObject);
        }else if(gameObject.GetComponent<SqlController>().restults == "")
        {
            return;
        }
        else
        {
            text.text = gameObject.GetComponent<SqlController>().restults;
        }
    }
}
