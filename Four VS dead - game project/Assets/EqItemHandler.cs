using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EqItemHandler : MonoBehaviour
{
    public int id;
    public Image ItemIcon;

    public void Select()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<EqSystem>().Select(id);
    }
}
