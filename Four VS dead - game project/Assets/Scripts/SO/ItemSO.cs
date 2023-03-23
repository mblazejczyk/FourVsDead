using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "GameScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public enum itemType { Badge }
    public itemType TypeOfItem;

    public int id;
    public Sprite ItemImg;
    public string ItemName;
    [TextArea]
    public string ItemDescription;
}